using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionPR1
{
    public class InformationOutput
    {
        public static void ShowClassesAndProperties(Assembly assembly)
        {
            Type[] types = assembly.GetTypes();
            var userTypes = types.Where(t => t.Namespace == "ReflectionLibrary" && !t.Name.EndsWith("Attribute"));

            Console.WriteLine("Class List:");
            foreach (Type classType in userTypes)
            {
                Console.WriteLine($"- {classType.Name}");
                PropertyInfo[] properties = classType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (PropertyInfo prop in properties)
                {
                    Console.WriteLine($"  {prop.Name}");
                }
            }
        }
    }
}
