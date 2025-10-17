using CrazyZoo.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyZoo.entity
{
    public class Cat : Animal, ICrazyAction
    {
        public override string MakeSound() => "Mjäu!";
        public void ActCrazy() => LogAction($"{Name} varastas köögist juustu.");
        private void LogAction(string msg) => MainWindow.Log(msg);
    }

}
