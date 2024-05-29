using Kursach;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursach
{
    internal class Moviemanagement: CinemaContext, IAddChanShow
    {
        CinemaContext db = new CinemaContext();
        ConsolePrinter printer = new ConsolePrinter();
        public void Add() {
            int movieID = printer.Read<int>("Введите ID нового фильма:");
            string movieName = printer.Read<string>("Введите название фильма:");
            int movieYear = printer.Read<int>("Введите год выхода фильма:");
            string movieDirector = printer.Read<string>("Введите директора:");
            string movieGenre = printer.Read<string>("Введите жанр фильма:");
            string movieChrono = printer.Read<string>("Введите хронометраж фильма:");
            string movieProduction = printer.Read<string>("Введите продакшн фильма:");
            try {
                Movie movie = new Movie(movieID, movieName, movieYear, movieDirector, movieGenre, movieChrono, movieProduction);
                db.Movies.Add(movie);
                db.SaveChanges();
                RaiseNotify("Фильм успешно добавлен");
            }
            catch(Exception ex) { 
                printer.Print("Ошибка при создании объекта: " + ex.Message);
            }
        }
        public void Change() {
            int movieID = printer.Read<int>("Введите ID фильма для изменения:");
            Movie? movie = db.Movies.FirstOrDefault(p=>p.MovieId==movieID);
            if(movie == null) {
                printer.Print("Неверный ID фильма");
                return;
            }
            movie.MovieName = printer.Read<string>("Введите новое название фильма:");
            movie.MovieYear = printer.Read<int>("Введите новый год выхода фильма:");
            movie.MovieDirector = printer.Read<string>("Введите нового директора:");
            movie.MovieGenre = printer.Read<string>("Введите новый жанр фильма:");
            movie.MovieChrono = printer.Read<string>("Введите новый хронометраж фильма:");
            movie.MovieProduction = printer.Read<string>("Введите новый продакшн фильма:");
            db.Movies.Update(movie);
            db.SaveChanges();
            RaiseNotify("Фильм успешно изменен");
        }
        public void Remove() {
            int movieID = printer.Read<int>("Введите ID фильма для удаления");
            Movie? movie = db.Movies.FirstOrDefault(p => p.MovieId == movieID);
            if (movie == null) {
                printer.Print("Неверный ID фильма");
                return;
            }
            Seance? seance = db.Seances.FirstOrDefault(p => p.MovieId == movieID); 
            if (seance != null) {
                printer.Print("Данный фильм используется в сеансе с ID = " + seance.SeanceId + ".");
                bool answer = printer.Read<bool>("Отменить сеанс ? [да-true,нет-false]");
                if (answer) {
                    TicketOffice ticketOffice = new TicketOffice();
                    ticketOffice.MoneyRefund(seance.SeanceId);
                }
                else {
                    printer.Print("Фильм не был удален");
                    return;
                }
            }
            //если seance == null значит нет сеанса с этим фильмом и можно спокойно удалять
            db.Movies.Remove(movie);
            db.SaveChanges();
            RaiseNotify("Фильм успешно удален");
        }
        public void ShowAll() {
            foreach (Movie i in db.Movies) {
                printer.Print($" ID фильма = {i.MovieId} ");
                printer.Print($" Название фильма = {i.MovieName}");
                printer.Print($" Год издания = {i.MovieYear}");
                printer.Print($" Жанр = {i.MovieGenre}");
                printer.Print($" Хронометраж = {i.MovieChrono}");
                printer.Print($" Директор = {i.MovieDirector}");
                printer.Print($" Продакшн = {i.MovieProduction}\n");
            }
        }
        public void Show() {
            int id = printer.Read<int>("Введите id фильма, который хотите вывести на экран");
            Movie? movie = db.Movies.FirstOrDefault(p => p.MovieId == id);
            if (movie == null) {
                printer.Print("Вы ввели неверный id");
                return;
            }
            printer.Print($" ID фильма = {movie.MovieId} ");
            printer.Print($" Название фильма = {movie.MovieName}");
            printer.Print($" Год издания = {movie.MovieYear}");
            printer.Print($" Жанр = {movie.MovieGenre}");
            printer.Print($" Хронометраж = {movie.MovieChrono}");
            printer.Print($" Директор = {movie.MovieDirector}");
            printer.Print($" Продакшн = {movie.MovieProduction}\n");
        }
    }
}
