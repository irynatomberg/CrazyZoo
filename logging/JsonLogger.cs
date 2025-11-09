using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CrazyZoo.logging
{
    public class JsonLogger : ILogger, IDisposable
    {
        private readonly string _path;
        private readonly BlockingCollection<Dictionary<string, string>> _queue = new();
        private readonly CancellationTokenSource _cts = new();
        private readonly Task _workerTask;

        public JsonLogger(string path = "log.json")
        {
            _path = path;
            _workerTask = Task.Run(WorkerLoop);
        }

        public void Log(string message)
        {
            var entry = new Dictionary<string, string>
            {
                ["Time"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                ["Message"] = message
            };

            _queue.Add(entry);
        }

        private async Task WorkerLoop()
        {
            while (!_cts.IsCancellationRequested)
            {
                try
                {
                    if (!_queue.TryTake(out var entry, Timeout.Infinite, _cts.Token))
                        continue;

                    if (entry == null) continue;

                    await Task.Run(() =>
                    {
                        lock (_path)
                        {
                            List<Dictionary<string, string>> logs = new();

                            if (File.Exists(_path))
                            {
                                try
                                {
                                    using var stream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                                    logs = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(stream) ?? new();
                                }
                                catch
                                {
                                    logs = new();
                                }
                            }

                            logs.Add(entry);

                            using var writeStream = new FileStream(_path, FileMode.Create, FileAccess.Write, FileShare.None);
                            var options = new JsonSerializerOptions { WriteIndented = true };
                            JsonSerializer.Serialize(writeStream, logs, options);
                        }
                    });
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[JsonLogger error] {ex.Message}");
                }
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            _queue.CompleteAdding();
            try { _workerTask.Wait(1000); } catch { }
            _cts.Dispose();
        }
    }
}
