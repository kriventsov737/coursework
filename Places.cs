using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursach {
    internal class Places {
        public int hall_ID { get; }
        public int place_category { get; }
        // если true - место доступно к продаже / false - недоступно
        public bool place_state { get; } = true;
        public void change_state()
        {
            //если за компом сидит админи (роль)
            // то позволяем менять состояние

            // вывести, что состояние места теперь ТАКОЕ
        }
    }
}
