using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CrazyZoo.infrastructure.logging
{
    public class XmlLogger : ILogger, IDisposable
    {
        private readonly string _path;
        private readonly BlockingCollection<string> _logQueue = new();
        private readonly CancellationTokenSource _cts = new();
        private readonly Task _workerTask;

        public XmlLogger(string path = "log.xml")
        {
            _path = path;

            _workerTask = Task.Run(WorkerLoop);
        }

        public void Log(string message)
        {
            string xmlEntry = new XElement("LogEntry",
                new XElement("Time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new XElement("Message", message)).ToString();

            _logQueue.Add(xmlEntry);
        }

        private async Task WorkerLoop()
        {
            while (!_cts.IsCancellationRequested)
            {
                try
                {
                    string? entry = null;

                    if (!_logQueue.TryTake(out entry, Timeout.Infinite, _cts.Token))
                        continue;

                    if (entry == null) continue;

                    await Task.Run(() =>
                    {
                        lock (_path)
                        {
                            XDocument doc;
                            if (File.Exists(_path))
                            {
                                using var stream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                                doc = XDocument.Load(stream);
                            }
                            else
                            {
                                doc = new XDocument(new XElement("Logs"));
                            }

                            doc.Root!.Add(XElement.Parse(entry));

                            using var outStream = new FileStream(_path, FileMode.Create, FileAccess.Write, FileShare.None);
                            doc.Save(outStream);
                        }
                    });
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[XmlLogger error] {ex.Message}");
                }
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            _logQueue.CompleteAdding();
            try { _workerTask.Wait(1000); } catch { }
            _cts.Dispose();
        }
    }
}
