using kursach;
using Kursach.classes_from_interfaces;
using Kursach.Managements;
using Microsoft.EntityFrameworkCore;

// 0 - false ( занят, не вип)
// 1 - true  ( свободен, вип)
namespace Kursach
{
    internal class Program {
        static void Main(string[] args) {
            ConsolePrinter printer = new ConsolePrinter();
            Notification notify=new Notification();
            CinemaContext db = new CinemaContext();

            printer.Print("Изменить рабочую базу данных? Да - 1");
            int choice = printer.Read<int>();
            if(choice == 1) 
                db.ConnectSqlServer();

            db.Register(notify.Notificate);
            Authorization authorization = new Authorization();
            // если регистрация не пройдет успешно
            if (!authorization.Authentification()) return;
            while (true) {
                printer.Print("Выберите с чем будете взаимодействовать");
                printer.Print("Билеты - 1");
                printer.Print("Сеансы - 2");
                printer.Print("Залы - 3");
                printer.Print("Места - 4");
                printer.Print("Фильмы - 5");
                printer.Print("Пользователи - 6");
                printer.Print("Выход - 0");
                int tableChoice = printer.Read<int>(); // выбор в главном меню
                choice =0; // выбор в побочном меню
                bool backToMenu = true; // переменная для выхода в главное меню
                //очистка консоли
                printer.Clear();
                switch (tableChoice) {
                    case 0: printer.Print("Вы вышли из системы"); return;
                    case 1:
                        while (backToMenu) {
                            TicketManagement TM = new TicketManagement();
                            printer.Print("Выберите действие по работе с билетами");
                            printer.Print("Добавить новый билет - 1");
                            printer.Print("Изменить билет - 2");
                            printer.Print("Вывести все билеты - 3");
                            printer.Print("Вывести конкретный билет - 4");
                            printer.Print("Вывести число свободных билетов на сеанс - 5");
                            printer.Print("В главное меню - 0");
                            choice = printer.Read<int>();
                            printer.Clear();
                            switch (choice) {
                                case 0: backToMenu=false; break;
                                // вызов метода; ожидание нажатия; выход из case
                                case 1: TM.Add(); printer.Wait(); break;
                                case 2: TM.Change(); printer.Wait(); break;
                                case 3: TM.ShowAll(); printer.Wait(); break;
                                case 4: TM.Show(); printer.Wait(); break;
                                case 5: TM.CntFreeTicket(); printer.Wait(); break;
                                default: printer.Print("Неверный ввод"); printer.Wait(); printer.Clear(); 
                                    break;
                            }
                            printer.Clear(); // очистка консоли
                        }
                        break;
                    case 2:
                        while (backToMenu) {
                            SeanceManagement SM = new SeanceManagement();
                            printer.Print("Выберите действие по работе с сеансами");
                            printer.Print("Добавить новый сеанс - 1");
                            printer.Print("Изменить сеанс - 2");
                            printer.Print("Вывести все сеансы - 3");
                            printer.Print("Вывести конкретный сеанс - 4");
                            printer.Print("Удалить сеанс - 5");
                            printer.Print("В главное меню - 0");
                            choice = printer.Read<int>();
                            printer.Clear();
                            switch (choice) {
                                case 0: backToMenu = false; break;
                                case 1: SM.Add(); printer.Wait(); break;
                                case 2: SM.Change(); printer.Wait(); break;
                                case 3: SM.ShowAll(); printer.Wait(); break;
                                case 4: SM.Show(); printer.Wait(); break;
                                case 5: SM.Remove(); printer.Wait(); break;
                                default: printer.Print("Неверный ввод");printer.Wait();printer.Clear(); 
                                    break;
                            }
                            printer.Clear();
                        }
                        break;
                    case 3:
                        while (backToMenu) {
                            HallsManagement HM = new HallsManagement();
                            printer.Print("Выберите действие по работе с залами");
                            printer.Print("Добавить новый зал - 1");
                            printer.Print("Изменить название зала - 2");
                            printer.Print("Вывести все залы - 3");
                            printer.Print("Вывести конкретный зал - 4");
                            printer.Print("В главное меню - 0");
                            choice = printer.Read<int>();
                            printer.Clear();
                            switch (choice) {
                                case 0: backToMenu = false; break;
                                case 1: HM.Add(); printer.Wait(); break;
                                case 2: HM.Change(); printer.Wait(); break;
                                case 3: HM.ShowAll(); printer.Wait(); break;
                                case 4: HM.Show(); printer.Wait(); break;
                                default: printer.Print("Неверный ввод"); printer.Wait(); printer.Clear(); 
                                    break;
                            }
                            printer.Clear();
                        }
                        break;
                    case 4:
                        while (backToMenu) {
                            PlacesManagement PM = new PlacesManagement();
                            printer.Print("Выберите действие по работе с местами");
                            printer.Print("Добавить новое место - 1");
                            printer.Print("Изменить категорию места - 2");
                            printer.Print("Вывести все места - 3");
                            printer.Print("Вывести конкретное место - 4");
                            printer.Print("В главное меню - 0");
                            choice = printer.Read<int>();
                            printer.Clear();
                            switch (choice) {
                                case 0: backToMenu = false; break;
                                case 1: PM.Add(); printer.Wait(); break;
                                case 2: PM.Change(); printer.Wait(); break;
                                case 3: PM.ShowAll(); printer.Wait(); break;
                                case 4: PM.Show(); printer.Wait(); break;
                                default: printer.Print("Неверный ввод");printer.Wait();printer.Clear(); 
                                    break;
                            }
                            printer.Clear();
                        }
                        break;
                    case 5:
                        while (backToMenu) {
                            Moviemanagement MM = new Moviemanagement();
                            printer.Print("Выберите действие по работе с фильмами");
                            printer.Print("Добавить новый фильм - 1");
                            printer.Print("Изменить фильм - 2");
                            printer.Print("Вывести все фильмы - 3");
                            printer.Print("Вывести конкретный фильм - 4");
                            printer.Print("Удалить фильм - 5");
                            printer.Print("В главное меню - 0");
                            choice = printer.Read<int>();
                            printer.Clear();
                            switch (choice) {
                                case 0: backToMenu = false; break;
                                case 1: MM.Add(); printer.Wait(); break;
                                case 2: MM.Change(); printer.Wait(); break;
                                case 3: MM.ShowAll(); printer.Wait(); break;
                                case 4: MM.Show(); printer.Wait(); break;
                                case 5: MM.Remove(); printer.Wait(); break;
                                default: printer.Print("Неверный ввод"); printer.Wait(); printer.Clear(); 
                                    break;
                            }
                            printer.Clear();
                        }
                        break;
                    case 6:
                        while (backToMenu) {
                            printer.Print("Выберите действие по работе с пользователями");
                            printer.Print("Добавить нового пользоваетля - 1");
                            printer.Print("В главное меню - 0");
                            choice = printer.Read<int>();
                            printer.Clear();
                            switch (choice) {
                                case 0: backToMenu = false; printer.Wait(); break;
                                case 1: authorization.AddUser(); printer.Wait(); break;
                                default: printer.Print("Неверный ввод");printer.Wait();printer.Clear();
                                    break;
                            }
                            printer.Clear();
                        }
                        break;
                    default: printer.Print("Неверный ввод");printer.Wait();printer.Clear();
                        break;
                }
            }
        }
    }
}
