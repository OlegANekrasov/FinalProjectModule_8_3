using System;
using System.IO;

namespace FinalProjectModule_8_3
{
    class Program
    {
        // Доработайте программу из задания 1, используя ваш метод из задания 2.
        // При запуске программа должна:
        // 1.	Показать, сколько весит папка до очистки.Использовать метод из задания 2. 
        // 2.	Выполнить очистку.
        // 3.	Показать сколько файлов удалено и сколько места освобождено.
        // 4.	Показать, сколько папка весит после очистки.
        static void Main(string[] args)
        {
            // На вход программа принимает путь до папки.
            if (args.Length == 0)
            {
                Console.WriteLine("Не задан путь до папки!");
                Console.ReadLine();
                return;
            }

            var path = args[0];
            var directory = new DirectoryInfo(path);
            if (directory.Exists) // предусмотрена проверка на наличие папки по заданному пути
            {
                Console.WriteLine($"Папка: {path}\n");
                try // предусмотрена обработка исключений при доступе к папке
                {
                    var size = GetSize(directory);
                    Console.WriteLine($"Исходный размер папки: {size} байт");
                    
                    Cleaning(directory);
                    
                    var sizeNew = GetSize(directory);
                    Console.WriteLine($"Освобождено: {size - sizeNew} байт");
                    Console.WriteLine($"Текущий размер папки: {sizeNew} байт");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка чистки папки: " + ex); // логирует исключение в консоль
                }
            }
            else
            {
                Console.WriteLine("Заданой папки не существует!");
            }

            Console.ReadLine();
        }

        static void Cleaning(DirectoryInfo directory)
        {
            FileInfo[] files = directory.GetFiles();
            foreach (FileInfo file in files)
            {
                TimeSpan interval = DateTime.Now - file.LastWriteTime;
                if (interval.Minutes > 30)
                {
                    file.Delete();
                }
            }

            DirectoryInfo[] dirs = directory.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                Cleaning(dir); // код должен удалять папки рекурсивно

                TimeSpan interval = DateTime.Now - dir.LastWriteTime;
                // Удаляем папку если в ней нет папок и файлов и она не изменялась больше 30 минут
                if (interval.Minutes > 30 && dir.GetDirectories().Length == 0 && dir.GetFiles().Length == 0)
                {
                    dir.Delete(true);
                }
            }
        }

        static long GetSize(DirectoryInfo directory)
        {
            long size = 0;

            FileInfo[] files = directory.GetFiles();
            foreach (FileInfo file in files)
            {
                size += file.Length;
            }

            DirectoryInfo[] dirs = directory.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                size += GetSize(dir);
            }

            return size;
        }
    }
}
