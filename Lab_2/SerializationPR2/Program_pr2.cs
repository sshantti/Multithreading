using SerializationPR2.Models;
using SerializationPR2.Services;
using SerializationPR2.Servises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationPR2
{
    internal static class Program_pr2
    {
        public static void Main()
        {
            var objectService = new ObjectService();
            var xDocumentService = new XDocumentService();
            var xmlDocumentService = new XmlDocumentService();

            while (true)
            {
                DisplayMenu();
                var input = Console.ReadLine();

                if (!Enum.TryParse<MenuOption>(input, out var choice))
                {
                    Console.WriteLine("Invalid input. Please try again.");
                    continue;
                }

                try
                {
                    ProcessChoice(choice, objectService, xDocumentService, xmlDocumentService);
                    if (choice == MenuOption.Exit) break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private static void DisplayMenu()
        {
            Console.WriteLine("\nMenu:");
            Console.WriteLine("// Task 1:");
            Console.WriteLine("1. CreateAndDisplayObjects (A)");
            Console.WriteLine("2. SerializeToXml (B)");
            Console.WriteLine("3. DisplayXmlFile (C)");

            Console.WriteLine("\n// Task 2:");
            Console.WriteLine("4. ReadAndDisplayObjects (A)");
            Console.WriteLine("5. FindModelsWithXDocument (B)");
            Console.WriteLine("6. FindModelsWithXmlDocument (C)");

            Console.WriteLine("\n// Task 3:");
            Console.WriteLine("7. ModifyAttributeWithXDocument (XDocument)");
            Console.WriteLine("8. ModifyAttributeWithXmlDocument (XmlDocument)");

            Console.WriteLine("\n9. Exit");
            Console.Write("Select an option: ");
        }

        private static void ProcessChoice(
            MenuOption choice,
            ObjectService objectService,
            IXmlService xDocumentService,
            IXmlService xmlDocumentService)
        {
            switch (choice)
            {
                case MenuOption.CreateAndDisplayObjects:
                    objectService.CreateAndDisplayObjects();
                    break;

                case MenuOption.SerializeToXml:
                    objectService.SerializeToXml();
                    Console.WriteLine("Objects serialized to XML.");
                    break;

                case MenuOption.DisplayXmlFile:
                    xDocumentService.DisplayXmlFile(ObjectService.DefaultXmlFileName);
                    break;

                case MenuOption.ReadAndDisplayObjects:
                    ObjectService.ReadAndDisplayObjects();
                    break;

                case MenuOption.FindModelsWithXDocument:
                    Console.WriteLine("Models found with XDocument:");
                    Console.Write("Enter element name to search: ");
                    var elementNameXDoc = Console.ReadLine() ?? "Model";
                    Console.WriteLine($"Models found with XDocument ({elementNameXDoc}):");
                    foreach (var value in xDocumentService.FindModels(ObjectService.DefaultXmlFileName, elementNameXDoc))
                    {
                        Console.WriteLine(value);
                    }
                    break;

                case MenuOption.FindModelsWithXmlDocument:
                    Console.Write("Enter element name to search: ");
                    var elementNameXmlDoc = Console.ReadLine() ?? "Model";
                    Console.WriteLine($"Models found with XmlDocument ({elementNameXmlDoc}):");
                    foreach (var value in xmlDocumentService.FindModels(ObjectService.DefaultXmlFileName, elementNameXmlDoc))
                    {
                        Console.WriteLine(value);
                    }
                    break;

                case MenuOption.ModifyAttributeWithXDocument:
                    ModifyAttribute(xDocumentService, "XDocument");
                    break;

                case MenuOption.ModifyAttributeWithXmlDocument:
                    ModifyAttribute(xmlDocumentService, "XmlDocument");
                    break;

                case MenuOption.Exit:
                    Console.WriteLine("Exiting...");
                    break;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }

        private static void ModifyAttribute(IXmlService service, string serviceName)
        {
            Console.Write("Enter attribute name: ");
            var attributeName = Console.ReadLine() ?? string.Empty;

            Console.Write("Enter element index: ");
            if (!int.TryParse(Console.ReadLine(), out var index))
            {
                throw new FormatException("Invalid index.");
            }

            Console.Write("Enter new value: ");
            var newValue = Console.ReadLine() ?? string.Empty;

            service.ModifyAttribute(ObjectService.DefaultXmlFileName, attributeName, index, newValue);
            Console.WriteLine($"Attribute modified using {serviceName}.");
        }
    }
}