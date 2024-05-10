using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursach
{
    internal class Ticket
    {
        public decimal ticket_cost { get; }
        public int hall_ID { get; }
        public int session_ID { get; }
        public int ticket_ID { get; }
        public string ticket_state { get; }
    }

}
