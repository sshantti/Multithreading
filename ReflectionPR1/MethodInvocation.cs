using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionPR1
{
    public static class MethodInvocation
    {
        public static void InvokeMethodFromUserInput(Assembly assembly)
        {
            Console.WriteLine("Введите имя класса:");
            string? className = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(className))
            {
                Console.WriteLine("Зачем козе баян, необходимо ввести имя класса");
                return;
            }

            Type? type = assembly.GetType($"ReflectionLibrary.{className}");
            if (type == null)
            {
                Console.WriteLine("Класс не найден");
                return;
            }

            Console.WriteLine("Введите имя метода:");
            string? methodName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(methodName))
            {
                Console.WriteLine("Зачем козе баян, необходимо ввести имя метода");
                return;
            }

            MethodInfo? method = type.GetMethod(methodName);
            if (method == null)
            {
                Console.WriteLine("Метод не найден");
                return;
            }

            object[] args = GetMethodArguments(method);
            object? instance = null;

            if (!method.IsStatic)
            {
                instance = Activator.CreateInstance(type);
                if (instance == null)
                {
                    Console.WriteLine("Не получается создать экземпляр класса");
                    return;
                }
            }

            try
            {
                object? result = method.Invoke(instance, args);
                if (result != null)
                {
                    Console.WriteLine("Результат выполнения метода: " + result);
                }
                else
                {
                    Console.WriteLine("Метод выполнен, но не вернул результат");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при вызове метода: {ex.Message}");
            }
        }

        private static object[] GetMethodArguments(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            object[] args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                Console.WriteLine($"Введите аргумент {parameters[i].Name}:");
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
