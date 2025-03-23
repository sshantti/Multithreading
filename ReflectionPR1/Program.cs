using ReflectionPR1;
using System;
using System.Reflection;

namespace ReflectionPR1 
{
    class Program
    {
        static void Main()
        {
            const string libraryFileName = "ReflectionLibrary.dll";

            Assembly assembly;
            try
            {
                assembly = BuilderLoader.LoadLibrary(libraryFileName);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Ошибка загрузки библиотеки: {ex.Message}");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
                return;
            }

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nВыберите задание:");
                Console.WriteLine("1. Задание 1. Инвокатор методов");
                Console.WriteLine("2. Задание 2. Вывести список классов и их элементов");
                Console.WriteLine("3. Задание 3. Вывести информациию об объекте");
                Console.WriteLine("4. Выход");
                Console.Write("Ваш выбор: ");
                string? choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        MethodInvocation.InvokeMethodFromUserInput(assembly);
                        break;
                    case "2":
                        InformationOutput.ShowClassesAndProperties(assembly);
                        break;
                    case "3":
                        ObjectCreator.CreateAndPrintObject(assembly);
                        break;
                    case "4":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Вы вправе распоряжаться только цифрами от 1 до 4. Попробуйте снова, lol.");
                        break;
                }
            }
        }
    }
}