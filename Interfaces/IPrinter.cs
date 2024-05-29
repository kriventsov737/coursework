using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursach {
    public interface IPrinter {
        void Print(string str) { }

        string Read() { string? str = null; return str; }
    }
}
