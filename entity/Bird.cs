using CrazyZoo.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyZoo.entity
{
    public class Bird : Animal, IFlyable, ICrazyAction
    {
        public bool IsFlying { get; private set; }

        public override string MakeSound() => "Tširp!";
        public void Fly()
        {
            IsFlying = !IsFlying;
            LogAction($"{Name} {(IsFlying ? "lendab" : "ei lenda")}!");
        }
        public void ActCrazy()
        {
            Fly();
            LogAction($"{Name} karjus: CHIRP!!!");
        }
        private void LogAction(string msg) => MainWindow.Log(msg);
    }
}
