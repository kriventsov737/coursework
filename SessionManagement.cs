using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursach {
    // класс описывает работу с сессиями
    internal class SessionManagement {
       List<Session> sessions;
        // статическая переменная = ссылка на конкретный экз класса
        private static SessionManagement? SessionManagement;
        // приватный конструктор для предотвращения его вызова извне
        private SessionManagement() { }
        // метод для получения единств экз-а
        public static SessionManagement getInstance()
        {
            // может убрать этот функционал в конструктор????????????
            if (bibliography == null)
            {
                bibliography = new Bibliography();
                return bibliography;
            }
            else
                throw new Exception("Попытка создать более одного экземпляра  класса Библеографии");
        }
        public void CreateNewSession() { 
            // создание новой сессии (допустим так)
            Session session = new Session();
            // добавление новой сессии в лист
            sessions.Add(session);
       }
       public void EditSession() {
        // реализовать итератор для поиска нужной сессии

        // изменение найденной сессии или же вывод ошибки
       }
    }
}
