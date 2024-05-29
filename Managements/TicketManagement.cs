using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursach {
    internal class TicketManagement :CinemaContext ,IAddChanShow{
        CinemaContext db = new CinemaContext();
        ConsolePrinter printer = new ConsolePrinter();
        public void Add() {
            CinemaContext db = new CinemaContext();
            ConsolePrinter printer = new ConsolePrinter();

            bool TicketState = true;
            int TicketId = printer.Read<int>("Введите ID билета: ");
            int TicketCost = printer.Read<int>("Введите стоимость билета: ");
            int SeanceId = printer.Read<int>("Введите ID сеанса: ");
            int HallID = printer.Read<int>("Введите ID зала: ");
            int PlaceID = printer.Read<int>("Введите ID места:");
            try {
                Hall? halltmp = new Hall();
                halltmp = db.Halls.FirstOrDefault(p => p.HallId == HallID);
                if (halltmp != null && halltmp.HallState == true) {
                    Ticket ticket = new Ticket(TicketId, TicketCost, TicketState, SeanceId, HallID, PlaceID);

                    db.Tickets.Add(ticket);
                    db.SaveChanges();
                    RaiseNotify("Таблица билетов успешно обновлена");
                }
                else printer.Print("Возникла проблема с залом");
            }
            catch (Exception ex) {
                printer.Print("Ошибка при создании объекта: " + ex.Message);
            }
        }
        public void Change() {
            int TicketId = printer.Read<int>("Введите ID билета для изменения: ");
            Ticket? ticket = db.Tickets.FirstOrDefault(p=>p.TicketId == TicketId);
            if (ticket == null) {
                printer.Print("Неверный ID билета");
                return;
            }
            ticket.TicketCost = printer.Read<int>("Введите новую стоимость билета");
            db.Tickets.Update(ticket);
            db.SaveChanges();
            RaiseNotify("Билет успешно изменен");
        }
        public void ShowAll() {
            int id = printer.Read<int>("Введите id сеанса, билеты на который хотите увидеть");
            Seance? seance = db.Seances.FirstOrDefault(s => s.SeanceId == id);
            if (seance == null) {
                printer.Print("Вы ввели неверный id");
                return;
            }
            foreach (Ticket i in db.Tickets) {
                if (i.SeanceId == id) {
                    printer.Print($" ID билета = {i.TicketId} ");
                    printer.Print($" Стоимость билета = {i.TicketCost}");
                    printer.Print($" Свободен ли билет = {i.TicketState}");
                    printer.Print($" ID сеанса = {i.SeanceId}");
                    printer.Print($" ID зала = {i.HallId}");
                    printer.Print($" ID места = {i.PlaceId}\n");
                }
            }
        }
        public void Show() {
            int id = printer.Read<int>("Введите id билета, который хотите вывести на экран");
            Ticket? ticket = db.Tickets.FirstOrDefault(p=>p.TicketId == id);
            if (ticket == null) {
                printer.Print("Вы ввели неверный id");
                return;
            }
            printer.Print($" ID билета = {ticket.TicketId} ");
            printer.Print($" Стоимость билета = {ticket.TicketCost}");
            printer.Print($" Свободен ли билет = {ticket.TicketState}");
            printer.Print($" ID сеанса = {ticket.SeanceId}");
            printer.Print($" ID зала = {ticket.HallId}");
            printer.Print($" ID места = {ticket.PlaceId} \n");
        }
		public void CntFreeTicket() {
            int seance_id = printer.Read<int>("Введите ID сеанса для которого хотите узнать число свободных билетов");
            Seance? seance = db.Seances.FirstOrDefault(p => p.SeanceId == seance_id);
            if (seance == null) { printer.Print("Вы ввели неверный ID сеанса"); return; }
            bool check = false;
            int cnt = 0;
            foreach (Ticket t in db.Tickets) {
                if((t.TicketState==true) && (seance_id==t.SeanceId))
                    cnt++;
                check = true;
            }
            if (check) {
                printer.Print("На сеанс с ID = "+seance_id+" непроданных билетов осталось: "+cnt);
                return;
            }
            printer.Print("Билеты на данный сеанс распроданы");
        }
    }
}
