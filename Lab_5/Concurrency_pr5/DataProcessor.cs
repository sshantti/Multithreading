using ClassLibrary;
using System.Collections.Concurrent;

namespace Concurrency_pr5
{
    public class DataProcessor
    {
        // Сортировка данных внутри словаря по ключам
        public static void SortData(ConcurrentDictionary<string, ConcurrentBag<object>> data)
        {
            try
            {
                foreach (var key in data.Keys.ToArray())
                {
                    var sorted = data[key] 
                        .OrderBy(o => o switch
                        {
                            Plane p => p.Model,
                            Manufacturer m => m.Name,
                            _ => string.Empty
                        })
                        .ToList();

                    data[key] = new ConcurrentBag<object>(sorted);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sorting data: {ex.Message}");
                throw;
            }
        }
    }
}