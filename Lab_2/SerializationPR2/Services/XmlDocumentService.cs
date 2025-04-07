using SerializationPR2.Servises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SerializationPR2.Services
{
    public sealed class XmlDocumentService : IXmlService
    {
        public void DisplayXmlFile(string filePath)
        {
            var doc = new XmlDocument();
            doc.Load(filePath);
            Console.WriteLine(doc.OuterXml);
        }
        public IEnumerable<string> FindModels(string filePath, string elementName)
        {
            var doc = new XmlDocument();
            doc.Load(filePath);

            var modelNodes = doc.SelectNodes($"//{elementName}")
                ?? throw new InvalidOperationException($"No {elementName} nodes found.");

            foreach (XmlNode node in modelNodes)
            {
                yield return node.InnerText;
            }
        }

        public void ModifyAttribute(string filePath, string attributeName,
                                  int elementIndex, string newValue)
        {
            var doc = new XmlDocument();
            doc.Load(filePath);

            var nodes = doc.SelectNodes("//*")
                ?? throw new InvalidOperationException("No nodes found in XML document.");

            if (elementIndex < 0 || elementIndex >= nodes.Count)
                throw new IndexOutOfRangeException("Invalid element index.");
            var node = nodes[elementIndex]!;
            var attribute = node.Attributes?[attributeName];

            if (attribute != null)
            {
                attribute.Value = newValue;
            }
            else
            {
                var childNode = node.SelectSingleNode(attributeName)
                    ?? throw new KeyNotFoundException($"Attribute or element '{attributeName}' not found.");
                childNode.InnerText = newValue;
            }

            doc.Save(filePath);
        }
    }
}