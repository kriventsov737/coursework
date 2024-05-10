using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursach {
    // класс описывает конкретную сессию
    internal class Session {
        // общая переменная для подсчета кол-ва сессий
        private static int id = 0;
        public Session() { id++; }
        // конструктор сессии со всеми параметрами
        public Session(int hall_ID, int movie_ID, DateTime session_date) {
            this.hall_ID = hall_ID;
            this.movie_ID = movie_ID;
            this.session_date = session_date;
            this.session_ID = id;
            Places = 
            Tickets = tickets;
            // переход к следующему id
            id++;
        }

        public int hall_ID{get;} // только для чтения извне
        public int movie_ID { get;}
        public DateTime session_date { get;}
        public int session_ID { get; }
        // список мест на эту сессию
        public List<Places> Places {get;} 
        // список билетов на эту сессию
        public List<Ticket> Tickets { get;} 

        // установка id фильма
        // установка id зала
        // установка id 
    }

}