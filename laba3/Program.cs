using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using static laba_1.Program;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
﻿
namespace laba_1 {
    internal class Program {
        public static string BASE_DIR = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\");

        #region Models
        /* ----- Классы для описание объектов таблиц ----- */

        public interface IEntity {
            public uint Id { get; }          // Идентификатор
            public string Name { get; set; } // Название

            public void Print();
        }

        /// <summary>
        /// Завод
        /// </summary>
        public class Factory : IEntity {
            public uint Id { get; }                 // Идентификатор завода (Primary key)
            public string Name { get; set; }        // Название завода
            public string Description { get; set; } // Описание завода

            // Конструктор с параметрами
            public Factory(uint id, string name, string description) {
                Id = id;
                Name = name;
                Description = description;
            }

            /// <summary>
            /// Вывод объекта класса в консоль
            /// </summary>
            public void Print() {
                Console.WriteLine($"└ Завод #{Id}");
                Console.WriteLine($"  ├ Название: {Name}");
                Console.WriteLine($"  └ Описание: {Description}\n");
            }
        }

        /// <summary>
        /// Установка
        /// </summary>
        public class Unit : IEntity {
            public uint Id { get; }                     // Идентификатор установки (Primary key)
            public string Name { get; set; }        // Название установки
            public string Description { get; set; } // Описание установки
            public uint FactoryId { get; set; }              // Идентификатор завода (Foreign key)

            // Конструктор с параметрами
            public Unit(uint id, string name, string description, uint factoryId) {
                Id = id;
                Name = name;
                Description = description;
                FactoryId = factoryId;
            }

            /// <summary>
            /// Вывод объекта класса в консоль
            /// </summary>
            public void Print() {
                Console.WriteLine($"└ Установка #{Id}");
                Console.WriteLine($"  ├ Название: {Name}");
                Console.WriteLine($"  ├ Описание: {Description}");
                Console.WriteLine($"  └ Завод #{FactoryId}\n");
            }
        }

        /// <summary>
        /// Резервуар
        /// </summary>
        public class Tank : IEntity {
            public uint Id { get; }            // Идентификатор резервуара (Primary key)
            public string Name { get; set; }        // Название резервуара
            public string Description { get; set; } // Описание резервуара
            public double Volume { get; set; }      // Объём резервуара
            public double MaxVolume { get; set; }   // Максимальный объём резервуара
            public uint UnitId { get; set; }        // Идентификатор установки (Foreign key)

            // Конструктор с параметрами
            public Tank(uint id, string name, string description, double volume, double maxVolume, uint unitId) {
                Id = id;
                Name = name;
                Description = description;
                Volume = volume;
                MaxVolume = maxVolume;
                UnitId = unitId;
            }

            /// <summary>
            /// Вывод объекта класса в консоль
            /// </summary>
            public void Print() {
                Console.WriteLine($"└ Резервуар #{Id}");
                Console.WriteLine($"  ├ Название: {Name}");
                Console.WriteLine($"  ├ Описание: {Description}");
                Console.WriteLine($"  ├ Объём: {Volume}");
                Console.WriteLine($"  ├ Максимальный объём: {MaxVolume}");
                Console.WriteLine($"  └ Установка #{UnitId}\n");
            }
        }

        public struct TankInfo {
            public uint Id { get; set; }             // Идентификатор резервуара (Primary key)
            public string Name { get; set; }         // Название резервуара
            public string Description { get; set; }  // Описание резервуара
            public double Volume { get; set; }       // Объём резервуара
            public double MaxVolume { get; set; }    // Максимальный объём резервуара
            public string FactoryName { get; set; }  // Название завода
            public string WorkshopName { get; set; } // Описание завода

            public TankInfo(uint id, string name, string description, double volume, double maxVolume, string factoryName, string workshopName) {
                Id = id;
                Name = name;
                Description = description;
                Volume = volume;
                MaxVolume = maxVolume;
                FactoryName = factoryName;
                WorkshopName = workshopName;
            }

            public void Print() {
                Console.WriteLine($"└ Резервуар #{Id}");
                Console.WriteLine($"  ├ Название: {Name}");
                Console.WriteLine($"  ├ Описание: {Description}");
                Console.WriteLine($"  ├ Объём: {Volume}");
                Console.WriteLine($"  ├ Максимальный объём: {MaxVolume}");
                Console.WriteLine($"  ├ Название завода: {FactoryName}");
                Console.WriteLine($"  └ Имя цеха: {WorkshopName}\n");
            }
        }

        /* ----------------------------------------------- */
        #endregion

        #region ImportModelsFromFiles
        /* ----- Функции для загрузки объектов таблиц из файла ----- */

        private static uint GetNextUnusedId(HashSet<uint> ids) {
            uint nextId = 1;
            while (ids.Contains(nextId))
                nextId++;

            return nextId;
        }

        /// <summary>
        /// Функция для получения списка заводов из файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="delimiter">Разделитель данных</param>
        /// <returns>Список заводов</returns>
        public static IList<Factory> GetFactoriesFromFile(string path, string delimiter = ";") {
            List<Factory> factories = new();
            HashSet<uint> Ids = new();
            uint lineCount = 1, id;


            if (!File.Exists(path)) {
                Console.WriteLine($"Файл по данному пути: {path} - не найден!");
                return factories;
            }

            foreach (var line in File.ReadAllLines(path)) {
                if (string.IsNullOrEmpty(line))
                    continue;

                var data = line.Split(delimiter);

                if (data.Length != 3) {
                    Console.WriteLine($"[!] Некорректный формат данных! Строка #{lineCount}");
                    continue;
                }

                if (!uint.TryParse(data[0], out id))
                    id = 0;

                if (id != 0 && !string.IsNullOrEmpty(data[1]) && !string.IsNullOrEmpty(data[2])) {
                    // Проверка на уникальность ID
                    if (factories.Any(factory => factory.Id == id))
                        id = GetNextUnusedId(Ids);

                    Ids.Add(id); // Добавить ID в список занятых идентификаторов
                    lineCount++;
                    var factory = new Factory(id, data[1], data[2]);
                    factories.Add(factory);
                }

                else {
                    Console.WriteLine($"[!] Некорректный формат данных! Строка #{lineCount}");
                }
            }

            return factories.OrderBy(factory => factory.Id).ToList();
        }

        /// <summary>
        /// Функция для получения списка резервуаров из файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="delimiter">Разделитель данных</param>
        /// <returns>Список установок</returns>
        public static IList<Unit> GetUnitsFromFile(string path, string delimiter = ";") {
            List<Unit> units = new List<Unit>() { };
            HashSet<uint> Ids = new HashSet<uint>() { };
            uint lineCount = 1, id, factoryId;

            if (!File.Exists(path)) {
                Console.WriteLine($"Файл по данному пути: {path} - не найден!");
                return units;
            }

            foreach (var line in File.ReadAllLines(path)) {
                if (string.IsNullOrEmpty(line))
                    continue;

                var data = line.Split(delimiter);

                if (data.Length != 4) {
                    Console.WriteLine($"[!] Некорректный формат данных! Строка #{lineCount}");
                    continue;
                }


                if (!uint.TryParse(data[0], out id) ||
                    !uint.TryParse(data[3], out factoryId)) {
                    Console.WriteLine($"[!] Некорректный формат данных! Строка #{lineCount}");
                    continue;
                }

                if (id == 0 || factoryId == 0 || string.IsNullOrEmpty(data[1]) || string.IsNullOrEmpty(data[2])) {
                    Console.WriteLine($"[!] Некорректный формат данных! Строка #{lineCount}");
                    continue;
                }

                // Проверка на уникальность ID
                if (units.Any(unit => unit.Id == id))
                    id = GetNextUnusedId(Ids);

                Ids.Add(id); // Добавить ID в список занятых идентификаторов
                lineCount++;
                var unit = new Unit(id, data[1], data[2], factoryId);
                units.Add(unit);
            }

            return units.OrderBy(factory => factory.Id).ToList();
        }

        /// <summary>
        /// Функция для получения списка резервуаров из файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="delimiter">Разделитель данных</param>
        /// <returns>Список объектов класса Tank</returns>
        public static IList<Tank> GetTanksFromFile(string path, string delimiter = ";") {
            List<Tank> tanks = new List<Tank>();
            HashSet<uint> Ids = new HashSet<uint>() { };
            uint id, volume, maxVolume, unitId, lineCount = 1;

            if (!File.Exists(path)) {
                Console.WriteLine($"Файл по данному пути: {path} - не найден!");
                return tanks;
            }



            foreach (var line in File.ReadAllLines(path)) {
                if (string.IsNullOrEmpty(line))
                    continue;

                var data = line.Split(delimiter);

                if (data.Length != 6) {
                    Console.WriteLine($"[!] Некорректный формат данных! Строка #{lineCount}");
                    continue;
                }

                if (!uint.TryParse(data[0], out id) ||
                    !uint.TryParse(data[3], out volume) ||
                    !uint.TryParse(data[4], out maxVolume) ||
                    !uint.TryParse(data[5], out unitId)
                ) {
                    Console.WriteLine($"[!] Некорректный формат данных! Строка #{lineCount}");
                    continue;
                }

                if (id == 0 || unitId == 0 || string.IsNullOrEmpty(data[1]) || string.IsNullOrEmpty(data[2])) {
                    Console.WriteLine($"[!] Некорректный формат данных! Строка #{lineCount}");
                    continue;
                }

                if ((volume >= 0 || maxVolume >= 0) && volume <= maxVolume) {
                    // Проверка на уникальность ID
                    if (tanks.Any(tank => tank.Id == id))
                        id = GetNextUnusedId(Ids);

                    Ids.Add(id); // Добавить ID в список занятых идентификаторов
                    lineCount++;
                    var tank = new Tank(id, data[1], data[2], volume, maxVolume, unitId);
                    tanks.Add(tank);
                }
                else {
                    Console.WriteLine($"[!] Объём резервуара не может быть отрицательным и/или больше максимального объёма! Строка #{lineCount}");
                }
            }

            return tanks.OrderBy(factory => factory.Id).ToList();
        }

        /* --------------------------------------------------------- */
        #endregion

        #region ImportModelsFromJson
        /* ----- Функции для загрузки объектов таблиц из JSON-файла ----- */

        public static IList<Factory>? GetFactoriesFromJson(string path) {
            try {
                return !File.Exists(path) ? null : JsonSerializer.Deserialize<List<Factory>>(File.ReadAllText(path));
            }

            catch (Exception e) {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static IList<Unit>? GetUnitsFromJson(string path) {
            try {
                return !File.Exists(path) ? null : JsonSerializer.Deserialize<List<Unit>>(File.ReadAllText(path));
            }

            catch (Exception e) {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static IList<Tank>? GetTanksFromJson(string path) {
            try {
                return !File.Exists(path) ? null : JsonSerializer.Deserialize<List<Tank>>(File.ReadAllText(path));
            }

            catch (Exception e) {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        /* -------------------------------------------------------------- */
        #endregion

        #region SearchModel
        /* ----- Функции для поиска объекта из списка по имени ----- */

        /// <summary>
        /// Функция для поиска завода по имени
        /// </summary>
        /// <param name="factories">Список заводов</param>
        /// <param name="name">Имя завода для поиска</param>
        /// <returns>Объект завода или null</returns>
        public static Factory? SearchFactoryByName(IEnumerable<Factory> factories, string name) {
            //return (from factory in factories
            //        where factory.Name == name
            //        select factory).FirstOrDefault();

            return factories.FirstOrDefault(factory => factory.Name == name);
        }

        /// <summary>
        /// Функция для поиска установки по имени
        /// </summary>
        /// <param name="units">Список установок</param>
        /// <param name="name">Имя установки для поиска</param>
        /// <returns>Объект установки или null</returns>
        public static Unit? SearchUnitByName(IEnumerable<Unit> units, string name) {
            //return (from unit in units
            //        where unit.Name == name
            //        select unit).FirstOrDefault();

            return units.FirstOrDefault(unit => unit.Name == name);
        }

        /// <summary>
        /// Функция для поиска резервуара по имени
        /// </summary>
        /// <param name="tanks">Список резервуаров</param>
        /// <param name="name">Имя резервуара для поиска</param>
        /// <returns>Объект резервуара или null</returns>
        public static Tank? SearchTankByName(IEnumerable<Tank> tanks, string name) {
            //return (from tank in tanks
            //        where tank.Name == name
            //        select tank).FirstOrDefault();

            return tanks.FirstOrDefault(tank => tank.Name == name);
        }

        /* --------------------------------------------------------- */
        #endregion

        #region OtherFunctions
        /// <summary>
        /// Функция для поиска установки, которой принадлежит резервуар
        /// </summary>
        /// <param name="units">Список установок</param>
        /// <param name="tanks">Список резервуаров</param>
        /// <param name="tankName">Название резервуара для поиска</param>
        /// <returns>Объект установки или null</returns>
        public static Unit? FindUnit(IEnumerable<Unit> units, IEnumerable<Tank> tanks, string tankName) {
            //return (from unit in units
            //        join tank in tanks on unit.Id equals tank.UnitId
            //        where tank.Name == tankName
            //        select unit).FirstOrDefault();

            var tank = tanks.FirstOrDefault(tank => tank.Name == tankName);
            if (tank == null)
                throw new InvalidOperationException($"Не найден резервуар с именем {tankName}!");

            return tank == null ? null : units.FirstOrDefault(unit => unit.Id == tank.UnitId);
        }

        /// <summary>
        /// Функция для поиска заводу, которому принадлежит установка
        /// </summary>
        /// <param name="factories">Список заводов</param>
        /// <param name="unit">Объект установок</param>
        /// <returns>Объект завода или null</returns>
        public static Factory? FindFactory(IEnumerable<Factory> factories, Unit unit) {
            //return (from factory in factories
            //        where factory.Id== unit.Id
            //        select factory).FirstOrDefault();

            return factories.FirstOrDefault(factory => factory.Id == unit.Id);
        }

        /// <summary>
        /// Функция для вычисления суммарного объёма все резервуаров
        /// </summary>
        /// <param name="tanks">Список резервуаров</param>
        /// <returns>Суммарный объём всех резервуаров</returns>
        public static double GetTotalVolume(IEnumerable<Tank> tanks) {
            //return (from tank in tanks
            //        select tank.Volume).Sum();

            return tanks.Sum(item => item.Volume);
        }

        /// <summary>
        /// Функция для вывода информации об резервуарах и их расположении
        /// </summary>
        /// <param name="factories"></param>
        /// <param name="units"></param>
        /// <param name="tanks"></param>
        public static void PrintAllTanks(IEnumerable<Factory> factories, IEnumerable<Unit> units, IEnumerable<Tank> tanks) {
            bool flag = true;
            string? choice;

            foreach (var tank in tanks) {
                var unit = units.FirstOrDefault(unit => unit.Id == tank.UnitId);
                if (unit == null)
                    continue;

                var factory = factories.FirstOrDefault(f => f.Id == unit.FactoryId);
                if (factory == null)
                    continue;

                Console.WriteLine($"└ Резервуар #{tank.Id}");
                Console.WriteLine($"  ├ Название: {tank.Name}");
                Console.WriteLine($"  ├ Описание: {tank.Description}");
                Console.WriteLine($"  ├ Объём: {tank.Volume}");
                Console.WriteLine($"  ├ Максимальный объём: {tank.MaxVolume}");
                Console.WriteLine($"  ├ Название завода: {factory.Description}");
                Console.WriteLine($"  └ Имя цеха: {factory.Name}\n");
            }

            while (flag) {
                Console.Write("\n[*] Сохранить результат в JSON-файл? [Д(Y) / н(n)] > ");
                choice = Console.ReadLine();

                if (choice != null)
                    if ("ДдYy".Contains(choice)) {
                        if (SaveTanksInfoFromJson(factories, units, tanks)) {
                            Console.WriteLine($"\n[*] Результат сохранен в JSON-файл по пути: {Path.Combine(BASE_DIR, "Output\\TanksInfoOutput.json")}");
                            flag = false;
                        }

                    }

                    else {
                        Console.WriteLine("\n[!] Не удалось сохранить информацию в JSON-файл!");
                        flag = false;
                    }
            }
        }

        public static bool SaveTanksInfoFromJson(IEnumerable<Factory> factories, IEnumerable<Unit> units, IEnumerable<Tank> tanks) {
            List<TankInfo> TanksInfo = new List<TankInfo>() { };
            TankInfo tankInfo;

            foreach (var tank in tanks) {
                var unit = units.FirstOrDefault(unit => unit.Id == tank.UnitId);
                if (unit == null)
                    continue;

                var factory = factories.FirstOrDefault(f => f.Id == unit.FactoryId);
                if (factory == null)
                    continue;

                tankInfo = new TankInfo(tank.Id, tank.Name, tank.Description, tank.Volume, tank.MaxVolume, factory.Description, factory.Name);
                TanksInfo.Add(tankInfo);
                tankInfo.Print();
            }

            if (tanks.Count() > 0) {
                try {
                    if (!Directory.Exists(Path.Combine(BASE_DIR, "Output")))
                        Directory.CreateDirectory(Path.Combine(BASE_DIR, "Output"));

                    var options = new JsonSerializerOptions {
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                        WriteIndented = true
                    };

                    File.WriteAllText(Path.Combine(BASE_DIR, "Output\\TanksInfoOutput.json"), JsonSerializer.Serialize(TanksInfo, options));

                    return true;
                }

                catch { return false; }
            }

            else
                return false;
        }

        void PrintCollection(List<IEntity> collection, string collectionName) {
            if (collection != null && collection.Count != 0) {
                foreach (var item in collection)
                    item.Print();
            }

            else {
                Console.WriteLine($"\n[!] Невозможно выполнить действие так, как отсутствуют данные об {collectionName}");
            }

            Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
            Console.ReadKey();
            Console.Clear();
        }
        #endregion

        static void Main(string[] args) {
            bool flag = true, flag_1, flag_2;
            IList<Factory>? factories = new List<Factory>() { };
            IList<Unit>? units = new List<Unit>() { };
            IList<Tank>? tanks = new List<Tank>() { };
            int choice;

            while (flag) {
                Console.WriteLine("[*] Выберете способ импорта данных:");
                Console.WriteLine("  > [1] Text File");
                Console.WriteLine("  > [2] JSON File");

                Console.Write("\n[*] Ваш выбор > ");
                if (int.TryParse(Console.ReadLine(), out choice)) {
                    switch (choice) {
                        case 1:
                            factories = GetFactoriesFromFile(Path.Combine(BASE_DIR, "Files\\FactoriesData.txt"));
                            units = GetUnitsFromFile(Path.Combine(BASE_DIR, "Files\\UnitsData.txt"));
                            tanks = GetTanksFromFile(Path.Combine(BASE_DIR, "Files\\TanksData.txt"));
                            flag = false;
                            break;

                        case 2:
                            factories = GetFactoriesFromJson(Path.Combine(BASE_DIR, "Json\\Factories.json"));
                            units = GetUnitsFromJson(Path.Combine(BASE_DIR, "Json\\Units.json"));
                            tanks = GetTanksFromJson(Path.Combine(BASE_DIR, "Json\\Tanks.json"));
                            flag = false;
                            break;

                        default:
                            Console.WriteLine("\n[!] Неверный выбор! Повторите попытку\n");
                            break;
                    }
                }
            }

            Console.Clear();
            flag = true;

            while (flag) {
                Console.WriteLine("[*] Меню программы:");
                Console.WriteLine("  > [1] Вывод все объекты таблицы");
                Console.WriteLine("  > [2] Вывод всех резервуаров с именами цеха и завода, где они числятся");
                Console.WriteLine("  > [3] Вывод общей сумму загрузки всех резервуаров");
                Console.WriteLine("  > [4] Найти объект в таблице по имени");
                Console.WriteLine("  > [5] Найти установку, которой принадлежит резервуар (по имени резервуара)");
                Console.WriteLine("  > [0] Выход");

                while (true) {
                    Console.Write("\n[*] Ваш выбор > ");
                    if (int.TryParse(Console.ReadLine(), out choice)) {
                        switch (choice) {
                            case 0:
                                flag = false;
                                break;

                            case 1:
                                flag_1 = true;
                                Console.WriteLine("\n[*] Выберите таблицу:");
                                Console.WriteLine("  > [1] Заводы");
                                Console.WriteLine("  > [2] Установки");
                                Console.WriteLine("  > [3] Резервуары");
                                Console.WriteLine("  > [0] Выход");

                                while (flag_1) {
                                    Console.Write("\n[*] Ваш выбор > ");
                                    if (int.TryParse(Console.ReadLine(), out choice)) {
                                        switch (choice) {
                                            case 0:
                                                flag_1 = false;
                                                break;

                                            case 1:
                                                if (factories != null) {
                                                    if (factories.Count != 0) {
                                                        foreach (var factory in factories)
                                                            factory.Print();

                                                        flag_1 = false;
                                                    }
                                                }

                                                else
                                                    Console.WriteLine("\n[!] Невозможно выполнить действие так, как отсутствуют данные об резервуарах");

                                                Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                                Console.ReadKey();
                                                Console.Clear();

                                                break;

                                            case 2:
                                                if (units != null) {
                                                    if (units.Count != 0) {
                                                        foreach (var unit in units)
                                                            unit.Print();

                                                        flag_1 = false;
                                                    }
                                                }

                                                else
                                                    Console.WriteLine("\n[!] Невозможно выполнить действие так, как отсутствуют данные об резервуарах");

                                                Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                                Console.ReadKey();
                                                Console.Clear();

                                                break;

                                            case 3:
                                                if (tanks != null) {
                                                    if (tanks.Count != 0) {
                                                        foreach (var tank in tanks)
                                                            tank.Print();

                                                        flag_1 = false;
                                                    }
                                                }

                                                else
                                                    Console.WriteLine("\n[!] Невозможно выполнить действие так, как отсутствуют данные об резервуарах");

                                                Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                                Console.ReadKey();
                                                Console.Clear();

                                                break;

                                            default:
                                                Console.WriteLine("\n[!] Неверный выбор! Повторите попытку");
                                                break;
                                        }
                                    }

                                    else {
                                        Console.WriteLine("\n[!] Неверный выбор! Повторите попытку");
                                    }


                                }

                                break;

                            case 2:
                                if (factories != null && units != null && tanks != null) {
                                    if (factories.Count != 0 || units.Count != 0 || tanks.Count != 0) {
                                        Console.WriteLine("\n");
                                        PrintAllTanks(factories, units, tanks);
                                    }
                                }

                                else
                                    Console.WriteLine("\n[!] Невозможно выполнить действие так, как отсутствуют данные о заводах и/или установках и/или резервуарах");

                                Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                Console.ReadKey();
                                Console.Clear();
                                break;

                            case 3:
                                if (tanks != null) {
                                    if (tanks.Count != 0)
                                        Console.WriteLine($"\n\nОбщая сумма загрузки всех резервуаров: {GetTotalVolume(tanks)}");

                                    else
                                        Console.WriteLine("Список резервуаров пуст!");

                                    Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                    Console.ReadKey();
                                    Console.Clear();
                                }

                                else
                                    Console.WriteLine("\n[!] Невозможно выполнить действие так, как отсутствуют данные об резервуарах");


                                break;

                            case 4:
                                flag_2 = true;
                                while (flag_2) {
                                    Console.WriteLine("\n[*] Выберите таблицу:");
                                    Console.WriteLine("  > [1] Заводы");
                                    Console.WriteLine("  > [2] Установки");
                                    Console.WriteLine("  > [3] Резервуары");
                                    Console.WriteLine("  > [0] Выход");

                                    Console.Write("\n[*] Ваш выбор > ");
                                    if (int.TryParse(Console.ReadLine(), out choice)) {
                                        switch (choice) {
                                            case 0:
                                                flag_2 = false;
                                                break;

                                            case 1:
                                                if (factories != null) {
                                                    if (factories.Count != 0) {
                                                        Console.Write("\n[*] Введите имя завода для поиска >");

                                                        string? name = Console.ReadLine();
                                                        if (!string.IsNullOrEmpty(name)) {
                                                            var result = SearchFactoryByName(factories, name);
                                                            if (result != null)
                                                                result.Print();

                                                            else
                                                                Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");
                                                        }

                                                        else
                                                            Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");

                                                        flag_2 = false;
                                                    }

                                                    else
                                                        Console.WriteLine("\nСписок заводов пуст!");
                                                }

                                                else
                                                    Console.WriteLine("\n[!] Невозможно выполнить действие так, как отсутствуют данные о заводах");

                                                Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                                Console.ReadKey();
                                                Console.Clear();

                                                break;

                                            case 2:
                                                if (units != null) {
                                                    if (units.Count != 0) {
                                                        Console.Write("\n[*] Введите имя установки для поиска > ");

                                                        string? name = Console.ReadLine();
                                                        if (!string.IsNullOrEmpty(name)) {
                                                            var result = SearchUnitByName(units, name);
                                                            if (result != null) {
                                                                result.Print();
                                                            }
                                                            else {
                                                                Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");
                                                            }
                                                        }

                                                        else
                                                            Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");

                                                        flag_2 = false;
                                                    }

                                                    else
                                                        Console.WriteLine("\nСписок установок пуст!");
                                                }

                                                else
                                                    Console.WriteLine("\n[!] Невозможно выполнить действие так, как отсутствуют данные об установках");

                                                Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                                Console.ReadKey();
                                                Console.Clear();

                                                break;

                                            case 3:
                                                if (tanks != null) {
                                                    if (tanks.Count != 0) {
                                                        Console.Write("\n[*] Введите имя резервуара для поиска >");

                                                        string? name = Console.ReadLine();
                                                        if (!string.IsNullOrEmpty(name)) {
                                                            var result = SearchTankByName(tanks, name);
                                                            if (result != null)
                                                                result.Print();

                                                            else
                                                                Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");
                                                        }

                                                        else
                                                            Console.WriteLine($"\nПо данному имени: {name} - ничего не найдено!");

                                                        flag_2 = false;
                                                    }


                                                    else
                                                        Console.WriteLine("\nСписок резервуаров пуст!");
                                                }

                                                else
                                                    Console.WriteLine("\n[!] Невозможно выполнить действие так, как отсутствуют данные об резервуарах");

                                                Console.WriteLine("\nНажмите любую клавишу, для возвращения в меню...");
                                                Console.ReadKey();
                                                Console.Clear();

                                                break;

                                            default:
                                                Console.WriteLine("\n[!] Неверный выбор! Повторите попытку");
                                                break;
                                        }
                                    }

                                    else {
                                        Console.WriteLine("\n[!] Неверный выбор! Повторите попытку");
                                    }
                                }

                                break;

                            default:
                                Console.WriteLine("\n[!] Неверный выбор! Повторите попытку\n");
                                break;
                        }

                        break;
                    }

                    else {
                        Console.WriteLine("\n[!] Неверный выбор! Повторите попытку");
                    }
                }
            }
        }
    }
}