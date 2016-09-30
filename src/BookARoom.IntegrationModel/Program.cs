using System;

namespace BookARoom.IntegrationModel
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("BookARoom Integration files generator.");

            Console.WriteLine($"Generates integration (json) files for a few hotels.{Environment.NewLine}");

            IntegrationFilesGenerator.GenerateJsonFileForNewYorkSofitel();
            IntegrationFilesGenerator.GenerateJsonFileForGrandBudapestHotel();
            IntegrationFilesGenerator.GenerateJsonFileForDanubiusHealthSpaResortHelia();
            IntegrationFilesGenerator.GenerateJsonFileForBudaFullAlwaysUnavailable();

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Type <enter> to exit:");
            Console.ReadLine();
        }
    }
}
