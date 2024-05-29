using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach.classes_from_interfaces;
using Microsoft.EntityFrameworkCore;

namespace Kursach.Managements
{
    internal class HallsManagement : CinemaContext, IAddChanShow
    {
        CinemaContext db = new CinemaContext();
        ConsolePrinter printer = new ConsolePrinter();
        public void Add()
        {
            //добавление обьекта
            int ID = printer.Read<int>("Введите ID: ");
            int capacity = printer.Read<int>("Введите вместимость зала: ");
            bool state = printer.Read<bool>("Введите состояние зала (true - доступен ,false - недоступен): ");
            Hall hall = new Hall(ID, printer.Read<string>("Введите название зала: "), state, capacity);
            db.Halls.Add(hall);
            db.SaveChanges();
            RaiseNotify("Таблица залов успешно обновлена");
        }
        public void Change() {
            int HallId = printer.Read<int>("Введите ID зала для изменения:");
            // поиск введенного сеанса
            Hall? hall = db.Halls.FirstOrDefault(p => p.HallId == HallId);
            // проверка, что сеанс точно найден
            if (hall == null) {
                printer.Print("Вы ввели неверный ID");
                return;// выход из метода
            }
            hall.HallName = printer.Read<string>("Введите новое имя зала: ");
            db.Halls.Update(hall);
            db.SaveChanges();
            RaiseNotify("Зал успешно изменён");
        }
        public void ShowAll() {
            foreach (Hall i in db.Halls) {
                printer.Print($" ID зала = {i.HallId} ");
                printer.Print($" Название зала = {i.HallName}");
                printer.Print($" Вместимость = {i.HallCapacity}");
                printer.Print($" Состояние = {i.HallState} \n");
            }
        }
        public void Show() {
            int id = printer.Read<int>("Введите id зала, который хотите вывести на экран");
            Hall? hall = db.Halls.FirstOrDefault(p => p.HallId == id);
            if (hall == null) {
                printer.Print("Вы ввели неверный id");
                return;
            }
            printer.Print($" ID зала = {hall.HallId} ");
            printer.Print($" Название зала = {hall.HallName}");
            printer.Print($" Вместимость = {hall.HallCapacity}");
            printer.Print($" Состояние = {hall.HallState}\n");
        }
        public void ChangeState() {
            int hall_id = printer.Read<int>("Введите id зала для изменения его состояния: ");
            // поиск зала по id и взятие нужного зала
            Hall? hall = db.Halls.FirstOrDefault(p => p.HallId == hall_id);
            if (hall == null) {
                printer.Print("Зал с таким id не найден ");
                return; // выход из метода
            }
            else if (!hall.HallState) { // если состояние зала было true, т.е. открыт
                Seance? seance = new Seance();
                // проверяем есть ли сеанс, проходящей в этом зале
                if ((seance = db.Seances.FirstOrDefault(p => p.HallId == hall_id)) != null) {
                    // нужно проверить дату сеанса, если она прошла, то просто закрыть зал
                    if (seance.SeanceDatetime < DateTime.Now) hall.HallState = false;
                    else {
                        // если дата не прошла еще, будем пытаться заменить зал на другой
                        Hall? tmp = new Hall(); // переменная для хранения зала с >= вместимостью
                        // если найден зал с такой же вместимостью или более
                        if ((tmp = db.Halls.FirstOrDefault(p => p.HallCapacity >= hall.HallCapacity)) != null) {
                            // то заменяем id зала в нужном сеансе на новый id зала
                            seance.HallId = tmp.HallId;
                        }
                        else {// если зал с >= вместимостью не найден, то возвращаем деньги
                              // вызов метода из класса кассы для возвращения клиентам денег за сеанс
                            TicketOffice ticketOffice = new TicketOffice();
                            ticketOffice.MoneyRefund(seance.SeanceId);
                            db.Seances.Remove(seance);
                            RaiseNotify("Сеанс, использующий данный зал, был удален ");
                        }
                    }
                }
                else {// если не найден сеанс использующий данный зал
                    hall.HallState = false;
                }
            }
            else // если зал был закрыт, просто открываем его
                hall.HallState = true;
            db.SaveChanges();
            RaiseNotify("Состояние зала с ID = " + hall_id + " было изменено");
        }

    }
}
