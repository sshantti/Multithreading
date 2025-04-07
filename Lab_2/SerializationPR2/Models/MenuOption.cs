using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationPR2.Models;

public enum MenuOption
{
    CreateAndDisplayObjects = 1,
    SerializeToXml,
    DisplayXmlFile,
    ReadAndDisplayObjects,
    FindModelsWithXDocument,
    FindModelsWithXmlDocument,
    ModifyAttributeWithXDocument,
    ModifyAttributeWithXmlDocument,
    Exit
}