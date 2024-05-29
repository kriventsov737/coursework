using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kursach
{
    internal class ConsolePrinter : IPrinter {
        public void Print<T>(T str)  {
            Console.WriteLine(str);
        }

        public T Read<T>() where T : IConvertible {
            T value;
            string? str = null;
            while (str == null) {
                str = Console.ReadLine();
            }
            while (true) {
                try {
                    value = (T)Convert.ChangeType(str, typeof(T));
                    break;
                }
                catch (Exception) {
                    Console.WriteLine("Неверный ввод. Попробуйте снова.");
                    str = Console.ReadLine();
                }
            }
            return value;
        }

        public T Read<T>(string message) where T : IConvertible {
            Console.WriteLine(message);
            T value;
            string? str = null;
            while (str == null) {
                str = Console.ReadLine();
            }
            while (true) {
                try {
                    value = (T)Convert.ChangeType(str, typeof(T));
                    break;
                }
                catch (Exception) {
                    Console.WriteLine("Неверный ввод. Попробуйте снова.");
                    str = Console.ReadLine();
                }
            }
            return value;
        }
        public void Clear() {
            Console.Clear();
        }
        public void Wait() {
            // Ожидание нажатия клавиши
            Print("Нажмите любую клавишу для продолжения");
            ConsoleKeyInfo keyInfo = Console.ReadKey();
        }
    }
}
