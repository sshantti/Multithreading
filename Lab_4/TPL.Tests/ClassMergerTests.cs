using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;
using Library;
using TPL;

namespace TPL.Tests
{
    [TestClass]
    public class ClassMergerTests
    {
        private const string TestFile1 = "test_merge1.xml";
        private const string TestFile2 = "test_merge2.xml";
        private const string MergedFile = "merged_test.txt";

        [TestInitialize]
        public void Initialize()
        {
            File.WriteAllText(TestFile1, @"<?xml version=""1.0""?><ArrayOfObject><Plane><Model>Test</Model></Plane></ArrayOfObject>");
            File.WriteAllText(TestFile2, @"<?xml version=""1.0""?><ArrayOfObject><Manufacturer><Name>Test</Name></Manufacturer></ArrayOfObject>");
        }

        [TestCleanup]
        public void Cleanup()
        {
            File.Delete(TestFile1);
            File.Delete(TestFile2);
            if (File.Exists(MergedFile)) File.Delete(MergedFile);
        }

        [TestMethod]
        public async Task MergeFilesAsync_ShouldCreateMergedFile()
        {
            var merger = new ClassMerger(TestFile1, TestFile2, MergedFile);
            await merger.MergeFilesAsync();
            Assert.IsTrue(File.Exists(MergedFile));
        }

        [TestMethod]
        public async Task MergeFilesAsync_ShouldCreateValidFile()
        {
            var merger = new ClassMerger(TestFile1, TestFile2, MergedFile);
            await merger.MergeFilesAsync();

            var content = File.ReadAllText(MergedFile);
            StringAssert.Contains(content, "Plane: Model=Test");
            StringAssert.Contains(content, "Manufacturer: Name=Test");
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public async Task ReadFileAsync_ShouldThrowOnMissingFile()
        {
            var merger = new ClassMerger("missing.xml", "missing.xml", MergedFile);
            await merger.MergeFilesAsync();
        }
    }
}