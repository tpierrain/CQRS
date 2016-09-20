using System;

namespace BookARoom.Integration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("BookARoom Integration files generator.");

            Console.WriteLine("Generates integration (json) files for a few hotels.");

            IntegrationFilesGenerator.GenerateJsonFileForNewYorkSofitel();
            IntegrationFilesGenerator.GenerateJsonFileForGrandBudapestHotel();
            IntegrationFilesGenerator.GenerateJsonFileForDanubiusHealthSpaResortHelia();
            IntegrationFilesGenerator.GenerateJsonFileForBudaFullAlwaysUnavailable();
        }
    }
}
