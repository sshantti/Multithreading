using System;
using System.Collections.Generic;
using ClassLibrary;

namespace MultithreadingPR3
{
    class Program_pr3
    {
        static void Main(string[] args)
        {
            var objects1 = new List<object>();
            var objects2 = new List<object>();

            for (int i = 0; i < 10; i++)
            {
                objects1.Add(Plane.Create($"SN{i}", $"Model{i}", $"PC{i}", EngineType.Electrical));
                objects2.Add(Manufacturer.Create($"Name{i}", $"Address{i}", i % 2 == 0));
            }

            var serializer = new Serializer(Constants.File1Path, Constants.File2Path, objects1, objects2);
            serializer.SerializeInParallel();
            Console.WriteLine(Constants.Task1Completed);

            var merger = new ClassMerger(Constants.File1Path, Constants.File2Path, Constants.MergedFilePath);
            merger.MergeClass();
            Console.WriteLine(Constants.Task2Completed);

            var reader = new Reader(Constants.MergedFilePath);

            Console.WriteLine(Constants.Task3SingleThread);
            reader.ReadFileSingleThread();

            Console.WriteLine(Constants.Task3TwoThreads);
            reader.ReadFileTwoThreads();

            Console.WriteLine(Constants.Task3TenThreads);
            reader.ReadFileWithTenThreads();
        }
    }
}