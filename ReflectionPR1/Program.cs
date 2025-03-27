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
                Console.WriteLine($"Library loading error: {ex.Message}");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected mistake: {ex.Message}");
                return;
            }

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nSelect a task:");
                Console.WriteLine("1. Task 1. Invoke Method");
                Console.WriteLine("2. Task 2. Print a list of classes and their elements");
                Console.WriteLine("3. Task 3. Output information");
                Console.WriteLine("4. Exit");
                Console.Write("Your selection: ");
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
                        Console.WriteLine("You are only allowed to dispose of the numbers 1 to 4. Try again, lol");
                        break;
                }
            }
        }
    }
}