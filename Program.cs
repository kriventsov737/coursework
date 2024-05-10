namespace Kursach {
    internal class Program {
        static void Main(string[] args) {
        }
    }
    internal class Bibliography
    {
        // статическая переменная = ссылка на конкретный экз класса
        private static Bibliography? bibliography;
        // приватный конструктор для предотвращения его вызова извне
        private Bibliography() { }
        // метод для получения единств экз-а
        public static Bibliography getInstance() 
        {
            if (bibliography == null)
            {
                bibliography = new Bibliography();
                return bibliography;
            }
            else
                throw new Exception("Попытка создать более одного экземпляра  класса Библеографии");
        }
    }

}
