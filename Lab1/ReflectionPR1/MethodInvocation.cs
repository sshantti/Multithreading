using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionPR1
{
    public class MethodInvocation
    {
        private const string PrintObjectMethodName = "PrintObject";
        private const string CreateMethodName = "Create";

        public static void InvokeMethodFromUserInput(Assembly assembly)
        {
            Console.WriteLine("Enter the class name:");
            string? className = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(className))
            {
                Console.WriteLine("You need to enter the class name");
                return;
            }

            Type? type = assembly.GetType($"ReflectionLibrary.{className}");
            if (type == null)
            {
                Console.WriteLine("Class not found");
                return;
            }

            Console.WriteLine("Enter the method name:");
            string? methodName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(methodName))
            {
                Console.WriteLine("You need to enter the method name");
                return;
            }

            MethodInfo? method;
            if (methodName == PrintObjectMethodName)
                method = type.GetMethod(PrintObjectMethodName);
            else if (methodName == CreateMethodName)
                method = type.GetMethod(CreateMethodName);
            else
                method = type.GetMethod(methodName);

            if (method == null)
            {
                Console.WriteLine("Method not found");
                return;
            }

            object[] args = Arguments.GetMethodArguments(method);
            object? instance = null;

            if (!method.IsStatic)
            {
                instance = Activator.CreateInstance(type);
                if (instance == null)
                {
                    Console.WriteLine("Cannot create an instance of the class");
                    return;
                }
            }

            try
            {
                object? result = method.Invoke(instance, args);
                if (result != null)
                {
                    Console.WriteLine("Result of the method execution: " + result);
                }
                else
                {
                    Console.WriteLine("Method executed, but did not return a result");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when calling a method: {ex.Message}");
            }
        }
    }
}
