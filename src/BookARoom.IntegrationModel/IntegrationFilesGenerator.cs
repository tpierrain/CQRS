using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace BookARoom.IntegrationModel
{
    public static class IntegrationFilesGenerator
    {
        private const string myFavorite2017Saturday = "2017-09-16";

        public static string WhereToGenerateDirectoryFullPath => Path.GetFullPath(AppContext.BaseDirectory + @"..\..\..\..\..\..\IntegrationFiles\");

        public static void GenerateJsonFileForNewYorkSofitel()
        {
            var hotelName = "New York Sofitel";
            var location = "New York";
            var roomsAvailability =  new RoomsAvailability(hotelName, location);
            roomsAvailability.AvailabilitiesAt.Add(DateTime.Parse(myFavorite2017Saturday), new RoomStatusAndPrices[] { new RoomStatusAndPrices("101", new Price("EUR", 109), new Price("EUR", 140)), new RoomStatusAndPrices("102", new Price("EUR", 109), new Price("EUR", 140)), new RoomStatusAndPrices("201", new Price("EUR", 209), new Price("EUR", 240)) });

            var generatedFilePath = SerializeToJsonFile(roomsAvailability);
            Console.WriteLine($"Integration file generated: {generatedFilePath}");
        }

        public static void GenerateJsonFileForGrandBudapestHotel()
        {
            var hotelName = "THE GRAND BUDAPEST HOTEL";
            var location = "Budapest";
            var roomsAvailability =  new RoomsAvailability(hotelName, location);
            roomsAvailability.AvailabilitiesAt.Add(DateTime.Parse(myFavorite2017Saturday), new RoomStatusAndPrices[] { new RoomStatusAndPrices("101", new Price("EUR", 109), new Price("EUR", 140)), new RoomStatusAndPrices("102", new Price("EUR", 109), new Price("EUR", 140)), new RoomStatusAndPrices("201", new Price("EUR", 209), new Price("EUR", 240)) });

            var generatedFilePath = SerializeToJsonFile(roomsAvailability);
            Console.WriteLine($"Integration file generated: {generatedFilePath}");
        }

        public static void GenerateJsonFileForDanubiusHealthSpaResortHelia()
        {
            var hotelName = "Danubius Health Spa Resort Helia";
            var location = "Budapest";
            var roomsAvailability =  new RoomsAvailability(hotelName, location);
            roomsAvailability.AvailabilitiesAt.Add(DateTime.Parse(myFavorite2017Saturday), new RoomStatusAndPrices[] { new RoomStatusAndPrices("101", new Price("EUR", 109), new Price("EUR", 140)) });

            var generatedFilePath = SerializeToJsonFile(roomsAvailability);
            Console.WriteLine($"Integration file generated: {generatedFilePath}");
        }

        public static void GenerateJsonFileForBudaFullAlwaysUnavailable()
        {
            var hotelName = "BudaFull-the-always-unavailable-hotel";
            var location = "Budapest";
            var roomsAvailability =  new RoomsAvailability(hotelName, location);
            roomsAvailability.AvailabilitiesAt.Add(DateTime.Parse(myFavorite2017Saturday), new RoomStatusAndPrices[] { });

            var generatedFilePath = SerializeToJsonFile(roomsAvailability);
            Console.WriteLine($"Integration file generated: {generatedFilePath}");
        }

        private static string SerializeToJsonFile(RoomsAvailability roomsAvailability)
        {
            var jsonContent = JsonConvert.SerializeObject(roomsAvailability, Formatting.Indented);
            var fileName = roomsAvailability.PlaceName + "-availabilities.json";
            
            // ensures the full paht directory exist
            Directory.CreateDirectory(WhereToGenerateDirectoryFullPath);

            // Generate JSON file
            var fileFullPath = Path.Combine(WhereToGenerateDirectoryFullPath, fileName);
            File.WriteAllText(fileFullPath, jsonContent, Encoding.UTF8);

            return fileFullPath;
        }
    }
}