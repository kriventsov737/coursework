using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kursach.Tables;

namespace Kursach
{
    internal class Authorization : CinemaContext {
        CinemaContext db = new CinemaContext();
        ConsolePrinter printer = new ConsolePrinter();
        public Authorization() { }
        //возвращает true, false в зависимости от успешности входа в систему
        public bool Authentification() {
            bool result = false;
            string login = printer.Read<string>("Введите имя пользователя");
            string password = printer.Read<string>("Введите пароль");
            try {
                AuthorizationData? user = db.AuthorizationData.FirstOrDefault(p => p.UserName == login);
                if (user == null)
                    printer.Print("Вы ввели неверное имя пользователя");
                else if (user.Pass == password) {
                    result = true;
                    printer.Print("Вы вошли в систему");
                }
                else
                    printer.Print("Вы ввели неверный пароль");
                return result;
            } catch (Exception ex) {
                printer.Print("Не удалось войти в систему. Ошибка: " + ex.Message);
                return result;
            }
        }
        public void AddUser() {
            int ID = printer.Read<int>("Введите ID для нового пользователя");
            string login = printer.Read<string>("Введите имя пользователя");
            string password = printer.Read<string>("Введите пароль пользователя");
            try {
                AuthorizationData user = new AuthorizationData(ID,login, password);
                db.AuthorizationData.Add(user);
                db.SaveChanges();
                RaiseNotify("Новый пользователь добавлен в базу данных");
            } catch (Exception ex) {
                printer.Print("Не удалось добавить нового пользователя. Ошибка: "+ ex.Message);
            }
        }
    }
}
