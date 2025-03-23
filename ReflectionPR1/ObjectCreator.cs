using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionPR1
{
    public static class ObjectCreator
    {
        public static void CreateAndPrintObject(Assembly assembly)
        {
            Console.WriteLine("Введите имя класса:");
            string? className = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(className))
            {
                Console.WriteLine("Зачем козе баян, необходимо ввести имя класса");
                return;
            }

            Type? selectedType = assembly.GetType($"ReflectionLibrary.{className}");
            if (selectedType == null)
            {
                Console.WriteLine("Класс не найден");
                return;
            }

            MethodInfo? createMethod = selectedType.GetMethod("Create");
            if (createMethod == null)
            {
                Console.WriteLine("Метод Create не найден");
                return;
            }

            object[] args = GetMethodArguments(createMethod);
            object? instance = createMethod.Invoke(null, args);

            if (instance == null)
            {
                Console.WriteLine("Не получилось создать объект");
                return;
            }

            MethodInfo? printMethod = selectedType.GetMethod("PrintObject");
            if (printMethod == null)
            {
                Console.WriteLine("Метод PrintObject не найден");
                return;
            }

            try
            {
                printMethod.Invoke(instance, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка вызова метода PrintObject: {ex.Message}");
            }
        }

        private static object[] GetMethodArguments(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            object[] args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                Console.WriteLine($"Введите значение для {parameters[i].Name}:");
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    throw new ArgumentException("Зачем козе баян, необходимо ввести аргумент");
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
                    throw new ArgumentException($"Ошибка преобразования аргумента: {ex.Message}");
                }
            }

            return args;
        }
    }
}