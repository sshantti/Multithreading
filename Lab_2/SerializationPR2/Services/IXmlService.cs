using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationPR2.Servises
{
    public interface IXmlService
    {
        void DisplayXmlFile(string filePath);
        IEnumerable<string> FindModels(string filePath, string elementName);
        void ModifyAttribute(string filePath, string attributeName, int elementIndex, string newValue);
    }
}