using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Library;
using TPL;

namespace TPL.Tests
{
    [TestClass]
    public class SerializerTests
    {
        private const string TestFile1 = "test1.xml";
        private const string TestFile2 = "test2.xml";

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(TestFile1)) File.Delete(TestFile1);
            if (File.Exists(TestFile2)) File.Delete(TestFile2);
        }

        [TestMethod]
        public async Task SerializeInParallelAsync_ShouldCreateFiles()
        {
            var objects = new List<object> { Plane.Create("SN1", "Model1", "PC1", EngineType.Electrical) };
            var serializer = new Serializer(TestFile1, TestFile2, objects, objects);

            await serializer.SerializeInParallelAsync();

            Assert.IsTrue(File.Exists(TestFile1));
            Assert.IsTrue(File.Exists(TestFile2));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowOnNullParameters()
        {
            var serializer = new Serializer(null!, "valid.xml", new List<object>(), new List<object>());
        }
    }
}