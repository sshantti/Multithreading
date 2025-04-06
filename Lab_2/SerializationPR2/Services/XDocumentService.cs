using SerializationPR2.Servises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SerializationPR2.Services
{
    public sealed class XDocumentService : IXmlService
    {
        public void DisplayXmlFile(string filePath)
        {
            var doc = XDocument.Load(filePath);
            Console.WriteLine(doc);
        }
        public IEnumerable<string> FindModels(string filePath, string elementName)
        {
            var doc = XDocument.Load(filePath);
            return doc.Descendants(elementName).Select(x => x.Value);
        }
        public void ModifyAttribute(
            string filePath,
            string attributeName,
            int elementIndex,
            string newValue)
        {
            var doc = XDocument.Load(filePath);
            var elements = doc.Descendants().ToList();

            if (elementIndex < 0 || elementIndex >= elements.Count)
                throw new IndexOutOfRangeException("Invalid element index.");

            var element = elements[elementIndex];
            var attribute = element.Attribute(attributeName);

            if (attribute != null)
            {
                attribute.Value = newValue;
            }
            else
            {
                var property = element.Element(attributeName)
                    ?? throw new KeyNotFoundException($"Attribute or element '{attributeName}' not found.");
                property.Value = newValue;
            }

            doc.Save(filePath);
        }
    }
}