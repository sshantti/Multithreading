using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Library;

namespace TPL
{
    class Program_pr4
    {
        static async Task Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Task 1: Serialization objects");
                Console.WriteLine("2. Task 2: File merge");
                Console.WriteLine("3. Task 3: File reading");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        await ExecuteTask1();
                        break;
                    case "2":
                        await ExecuteTask2();
                        break;
                    case "3":
                        await ExecuteTask3();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Неверный выбор");
                        break;
                }
            }
        }

        static async Task ExecuteTask1()
        {
            try
            {
                var objects1 = new List<object>();
                var objects2 = new List<object>();
                for (int i = 0; i < 10; i++)
                {
                    objects1.Add(Plane.Create($"SN{i}", $"Model{i}", $"PC{i}", EngineType.Electrical));
                    objects2.Add(Manufacturer.Create($"Name{i}", $"Address{i}", i % 2 == 0));
                }

                var serializer = new Serializer(Constants.File1Path, Constants.File2Path, objects1, objects2);
                await serializer.SerializeInParallelAsync();
                Console.WriteLine(Constants.Task1Completed);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
        static async Task ExecuteTask2()
        {
            try
            {
                if (!File.Exists(Constants.File1Path)) throw new FileNotFoundException("File1 not found");
                if (!File.Exists(Constants.File2Path)) throw new FileNotFoundException("File2 not found");

                var merger = new ClassMerger(Constants.File1Path, Constants.File2Path, Constants.MergedFilePath);
                await merger.MergeFilesAsync();

                if (!File.Exists(Constants.MergedFilePath))
                    throw new InvalidOperationException("Merged file wasn't created");

                Console.WriteLine(Constants.Task2Completed);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        static async Task ExecuteTask3()
        {
            try
            {
                var reader = new Reader(Constants.MergedFilePath);
                Console.WriteLine(Constants.Task3SingleThread);
                await reader.ReadFileSingleThreadAsync();

                Console.WriteLine(Constants.Task3TwoThreads);
                await reader.ReadFileTwoThreadsAsync();

                Console.WriteLine(Constants.Task3TenThreads);
                await reader.ReadFileWithTenThreadsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}