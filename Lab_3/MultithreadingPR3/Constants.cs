using System;
using System.Text;

namespace MultithreadingPR3
{
    public static class Constants
    {
        public const string File1Path = "file1.xml";
        public const string File2Path = "file2.xml";
        public const string MergedFilePath = "merged.txt";

        public const string PlaneFormat = "Plane: Model={0}, PlaneCode={1}, EngineType={2}";
        public const string ManufacturerFormat = "Manufacturer: Name={0}, Address={1}";

        public const string Task1Completed = "Task 1 completed: Files serialized in parallel";
        public const string Task2Completed = "Task 2 completed: Files merged";
        public const string Task3SingleThread = "\nTask 3.1: Single thread reading";
        public const string Task3TwoThreads = "\nTask 3.2: Two threads reading";
        public const string Task3TenThreads = "\nTask 3.3: Ten threads with semaphore (max 5 concurrent)";
        public const string TimeTakenFormat = "Time taken: {0} ms";
    }
}