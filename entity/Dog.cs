using CrazyZoo.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyZoo.entity
{
    public class Dog : Animal, ICrazyAction
    {
        public override string MakeSound() => "Auh!";
        public void ActCrazy()
        {
            string sound = string.Join(" ", Enumerable.Repeat("Auh!", 5));
            LogAction($"{Name} haukus: {sound}");
        }
        private void LogAction(string msg) => MainWindow.Log(msg);
    }
}
