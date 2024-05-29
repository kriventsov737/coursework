using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursach.classes_from_interfaces {
    internal class Notification {
        public void Notificate(string str) {
            ConsolePrinter cp = new ConsolePrinter();
            cp.Print(str);
        }
    }
}
