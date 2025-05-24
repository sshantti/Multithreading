using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TPL;

namespace TPL.Tests
{
    [TestClass]
    public class ReaderTests
    {
        private const string TestFile = "test_reader.txt";

        [TestInitialize]
        public void Initialize()
            => File.WriteAllText(TestFile, "Test content", Encoding.UTF8);

        [TestCleanup]
        public void Cleanup()
            => File.Delete(TestFile);

        [TestMethod]
        // Проверяет однопоточное чтение файла.
        public async Task ReadFileSingleThreadAsync_ShouldReadFile()
        {
            var reader = new Reader(TestFile);
            await reader.ReadFileSingleThreadAsync();
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        // Проверяет, что конструктор выбрасывает исключение при отсутствии файла.
        public void Constructor_ShouldThrowOnMissingFile()
            => new Reader("missing.txt");
    }
}