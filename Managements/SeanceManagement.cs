using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursach {
    // класс описывает работу с сессиями
    internal class SeanceManagement : CinemaContext ,IAddChanShow {
        CinemaContext db = new CinemaContext();
        ConsolePrinter printer = new ConsolePrinter();
        public void Add() {
            //добавление обьекта
            int SeanceId = printer.Read<int>("Введите ID сеанса: ");
            int HallID = printer.Read<int>("Введите ID зала: ");
            int MovieId = printer.Read<int>("Введите ID фильма: ");
            DateTime SeanceDateTime = printer.Read<DateTime>("Введите дату и время сенса: ");

            Hall? halltmp = new Hall();
            var seance = new Seance();
            // проверка добавления сеанса с учетом проверки id
            try {
                // нужна еще проверка, что у выбранного зала state == true
                Seance tmp = new Seance(SeanceId, HallID, MovieId, SeanceDateTime, halltmp.HallCapacity);
                halltmp = db.Halls.FirstOrDefault(p => p.HallId == HallID);
                if (halltmp == null) {
                    printer.Print("Не найден зал с ID = " + HallID);
                    return;// выход из метода
                }
                if (halltmp.HallState == true) {
                    seance = tmp;
                    db.Seances.Add(seance);
                    db.SaveChanges();
                    RaiseNotify("Таблица сеансов успешно обновлена");
                }
                else {
                    printer.Print("Выбранный зал занят!");
                }
            }
            catch (Exception ex) {
                printer.Print("Ошибка при создании объекта: " + ex.Message);
            }
        }
        public void Remove() {
            int SeanceId = printer.Read<int>("Введите ID сеанса для удаления");
            Seance? seance = db.Seances.FirstOrDefault(p=>p.SeanceId == SeanceId);
            if (seance == null) {
                printer.Print("Данного сеанса не существует");
                return;
            }
            Hall? hall = db.Halls.FirstOrDefault(p => p.HallId == seance.HallId);
            hall.HallState = true;
            foreach (Ticket i in db.Tickets) {
                if (i.SeanceId==SeanceId) {
                    db.Tickets.Remove(i);
                }
            }
            db.Seances.Remove(seance);
            db.SaveChanges();
            RaiseNotify("Сеанс и билеты на него успешно удалены");
        }
        public void Change() {
            int SeanceId = printer.Read<int>("Введите ID сеанса для изменения:");
            // поиск введенного сеанса
            Seance? seance = db.Seances.FirstOrDefault(p=>p.SeanceId== SeanceId);
            // проверка, что сеанс точно найден
            if(seance ==null) {
                printer.Print("Вы ввели неверный ID");
                return;// выход из метода
            }
            int HallID = printer.Read<int>("Введите новый ID зала: ");
            int MovieId = printer.Read<int>("Введите новый ID фильма: ");
            DateTime SeanceDateTime = printer.Read<DateTime>("Введите новую дату и время сенса: ");
            
            Hall? hall = db.Halls.FirstOrDefault(p=>p.HallId == HallID);
            if ((hall == null) || !hall.HallState) {
                printer.Print("Возникла ошибка с залом, проверьте правильность ввода");
                return;
            }
            if (!hall.HallState) { 
                printer.Print("Зал занят другим сеансом");
                return;
            }
            try {
                seance.SeanceDatetime = SeanceDateTime;
                seance.MovieId = MovieId;
                seance.HallId = HallID;
                seance.SeancePlace = hall.HallCapacity;
                db.Seances.Update(seance);
                db.SaveChanges();
                RaiseNotify("Сеанс с ID = " + SeanceId + " успешно изменен");
            }
            catch (Exception ex) {
                printer.Print("Возникла ошибка во время изменения сеанса, перепроверьте данные");
                printer.Print("Ошибка: " + ex.Message);
            }
        }
        public void ShowAll() {
            foreach (Seance i in db.Seances) {
                printer.Print($" ID сеанса = {i.SeanceId} ");
                printer.Print($" ID зала = {i.HallId}");
                printer.Print($" ID фильма = {i.MovieId}");
                printer.Print($" Дата и время сеанса = {i.SeanceDatetime}");
                printer.Print($" Кол-во мест для сеанса = {i.SeancePlace}\n");
            }
        }
        public void Show() {
            int id = printer.Read<int>("Введите id сеанса, который хотите вывести на экран");
            Seance? seance = db.Seances.FirstOrDefault(p => p.SeanceId == id);
            if (seance == null) {
                printer.Print("Вы ввели неверный id");
                return;
            }
            printer.Print($" ID сеанса = {seance.SeanceId} ");
            printer.Print($" ID зала = {seance.HallId}");
            printer.Print($" ID фильма = {seance.MovieId}");
            printer.Print($" Дата и время сеанса = {seance.SeanceDatetime}");
            printer.Print($" Кол-во мест для сеанса = {seance.SeancePlace}\n");
        }
    }
}
