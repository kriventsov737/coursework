using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursach
{
    class ConsolePrinter
    {// сверится с метанитом, 3 принцип solid
        public static void Print(string format, object arg)
        {
            Console.Write(format, arg);
        }
    }
}
