using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kursach
{
    class PlacesManagement: CinemaContext, IAddChanShow
    {
        CinemaContext db = new CinemaContext();
        ConsolePrinter printer = new ConsolePrinter();
        public void Add() {
            int PlaceId = printer.Read<int>("Введите ID места: ");
            int HallID = printer.Read<int>("Введите ID зала: ");
            int PlaceRow = printer.Read<int>("Введите ряд, в котором находится место: ");
            int PlaceNumber = printer.Read<int>("Введите номер места : ");
            bool PlaceCategory = printer.Read<bool>("Введите категория места (true - VIP ,false - стандарт): ");
            // проверка добавления сеанса с учетом проверки id
            try
            {
                Place place = new Place(PlaceId, HallID, PlaceCategory,PlaceNumber,PlaceRow);
                db.Places.Add(place);
                db.SaveChanges();
                RaiseNotify("Сеансов успешно добавлен");
            }
            catch (Exception ex)
            {
                printer.Print("Ошибка при создании сеанса: " + ex.Message);
            }
        }
        public void Change() {
            int PlaceId = printer.Read<int>("Введите ID места для изменения: ");
            Place? place = db.Places.FirstOrDefault(p=>p.PlaceId == PlaceId);
            if (place == null) {
                printer.Print("Неверный ID места");
                return;
            }
            place.PlaceCategory = !place.PlaceCategory;
            db.Places.Update(place);
            db.SaveChanges();
            RaiseNotify("Категория места была изменена");
        }
        public void ShowAll() {
            int id = printer.Read<int>("Введите id зала, места которого хотите увидеть");
            Hall? hall = db.Halls.FirstOrDefault(h=>h.HallId == id);
            if (hall == null) {
                printer.Print("Вы ввели неверный id");
                return;
            }
            foreach (Place i in db.Places) {
                if (i.HallId == id) {
                    printer.Print($" ID места = {i.PlaceId} ");
                    printer.Print($" Категория места (vip) = {i.PlaceCategory}");
                    printer.Print($" Номер ряда = {i.PlaceRow}");
                    printer.Print($" Номер места = {i.PlaceNumber}");
                    printer.Print($" ID зала = {i.HallId} \n");
                }
            }
        }
        public void Show() {
            int id = printer.Read<int>("Введите id места, которое хотите вывести на экран");
            Place? place = db.Places.FirstOrDefault(p => p.PlaceId == id);
            if (place == null) {
                printer.Print("Вы ввели неверный id");
                return;
            }
            printer.Print($" ID места = {place.PlaceId} ");
            printer.Print($" Категория места (vip) = {place.PlaceCategory}");
            printer.Print($" Номер ряда = {place.PlaceRow}");
            printer.Print($" Номер места = {place.PlaceNumber}");
            printer.Print($" ID зала = {place.HallId}\n");
        }
    }
}
