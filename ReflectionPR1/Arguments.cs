using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionPR1
{
    public static class Arguments
    {
        public static object[] GetMethodArguments(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            object[] args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                Console.WriteLine($"Enter an argument {parameters[i].Name}:");
                string? input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    throw new ArgumentException("You need to enter an argument");
                }
                args[i] = ConvertInput(input, parameters[i].ParameterType);
            }

            return args;
        }

        public static object ConvertInput(string input, Type targetType)
        {
            try
            {
                if (targetType.IsEnum)
                {
                    return Enum.Parse(targetType, input);
                }
                return Convert.ChangeType(input, targetType);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Argument conversion error: {ex.Message}");
            }
        }
    }
}
