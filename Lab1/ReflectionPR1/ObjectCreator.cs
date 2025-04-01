using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionPR1
{
    public class ObjectCreator
    {
        private const string CreateMethodName = "Create";
        private const string PrintObjectMethodName = "PrintObject";
        public static void CreateAndPrintObject(Assembly assembly)
        {
            Console.WriteLine("Enter the class name:");
            string? className = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(className))
            {
                Console.WriteLine("You need to enter the class name");
                return;
            }

            Type? selectedType = assembly.GetType($"ReflectionLibrary.{className}");
            if (selectedType == null)
            {
                Console.WriteLine("Class not found");
                return;
            }

            MethodInfo? createMethod = selectedType.GetMethod(CreateMethodName);
            if (createMethod == null)
            {
                Console.WriteLine($"Method {CreateMethodName} not found");
                return;
            }

            object[] args = Arguments.GetMethodArguments(createMethod);
            object? instance = createMethod.Invoke(null, args);

            if (instance == null)
            {
                Console.WriteLine("Failed to create object");
                return;
            }

            MethodInfo? printMethod = selectedType.GetMethod(PrintObjectMethodName);
            if (printMethod == null)
            {
                Console.WriteLine($"Method {PrintObjectMethodName} not found");
                return;
            }

            try
            {
                printMethod.Invoke(instance, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Method invocation error: {ex.Message}");
            }
        }
    }
}