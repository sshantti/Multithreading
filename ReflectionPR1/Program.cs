using ReflectionPR1;
using System;
using System.Reflection;

namespace ReflectionPR1 
{
    class Program
    {
        static void Main()
        {
            string libraryPath = "ReflectionLibrary.dll";

            Assembly assembly = Assembly.LoadFrom(libraryPath);

            Type[] types = assembly.GetTypes();

            var userTypes = types.Where(t => t.Namespace == "ReflectionLibrary" && !t.Name.EndsWith("Attribute"));

            Console.WriteLine("Список классов в библиотеке:");
            foreach (Type classType in userTypes)
            {
                Console.WriteLine($"- {classType.Name}");
            }

            Console.WriteLine("Введите имя класса:");
            string? className = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(className))
            {
                Console.WriteLine("Имя класса не может быть пустым.");
                return;
            }

            Type? type = assembly.GetType($"ReflectionLibrary.{className}");
            if (type == null)
            {
                Console.WriteLine("Класс не найден.");
                return;
            }

            Console.WriteLine("Введите имя метода:");
            string? methodName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(methodName))
            {
                Console.WriteLine("Имя метода не может быть пустым.");
                return;
            }

            MethodInfo? method = type.GetMethod(methodName);
            if (method == null)
            {
                Console.WriteLine("Метод не найден.");
                return;
            }

            ParameterInfo[] parameters = method.GetParameters();
            object[] args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                Console.WriteLine($"Введите аргумент {parameters[i].Name} ({parameters[i].ParameterType}):");
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Аргумент не может быть пустым.");
                    return;
                }
                try
                {
                    if (parameters[i].ParameterType.IsEnum)
                    {
                        args[i] = Enum.Parse(parameters[i].ParameterType, input);
                    }
                    else
                    {
                        args[i] = Convert.ChangeType(input, parameters[i].ParameterType);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка преобразования аргумента: {ex.Message}");
                    return;
                }
            }

            object? instance = null;
            if (!method.IsStatic)
            {
                instance = Activator.CreateInstance(type);
                if (instance == null)
                {
                    Console.WriteLine("Не удалось создать экземпляр класса.");
                    return;
                }
            }

            object? result = method.Invoke(instance, args);
            if (result != null)
            {
                Console.WriteLine("Результат выполнения метода: " + result);
            }
            else
            {
                Console.WriteLine("Метод выполнен успешно, но не вернул результат.");
            }
        }
    }
}