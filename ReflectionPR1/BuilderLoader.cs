using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReflectionPR1
{
    public static class BuilderLoader
    {
        public static Assembly LoadLibrary(string libraryFileName)
        {
            string libraryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, libraryFileName);
            if (!File.Exists(libraryPath))
            {
                throw new FileNotFoundException($"File not found: {libraryPath}");
            }
            return Assembly.LoadFrom(libraryPath);
        }
    }
}