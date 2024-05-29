using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace Kursach {
    internal class TicketOffice {
    public TicketOffice() { }
    public void MoneyRefund(int seanceID) {
            // логика по возвращению денег клиентам, купившим билеты на сеанс с seanceID
            ConsolePrinter printer = new ConsolePrinter();
            printer.Print("Деньги клиентам возвращены");
        }
    }
}
