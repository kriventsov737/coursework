using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursach
{
    internal class Ticket_management: IAddRemChanShow
    {

        public void CreateNewTicket()
        {
            Ticket ticket = new Ticket();
            tickets.Add(ticket);
        }

        public void EditTicket()
        {

        }
    }
}
