using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace BookARoom.IntegrationModel
{
    /// <summary>
    /// Helper class to generate integration files for various hotels
    /// </summary>
    public static class IntegrationFilesGenerator
    {
        private const string myFavorite2017Saturday = "2017-09-16";

        public static string WhereToGenerateDirectoryFullPath => Path.GetFullPath(AppContext.BaseDirectory + @"..\..\..\..\..\..\integration-files\");

        public static void GenerateJsonFileForNewYorkSofitel()
        {
            var hotelId = 1;
            var hotelName = "New York Sofitel";
            var location = "New York";
            var numberOfRooms = 405;

            var roomsAvailability =  new HotelDetailsWithRoomsAvailabilities(hotelId, hotelName, location, numberOfRooms);
            roomsAvailability.AvailabilitiesAt.Add(DateTime.Parse(myFavorite2017Saturday), new RoomStatusAndPrices[] { new RoomStatusAndPrices("101", new Price("EUR", 109), new Price("EUR", 140)), new RoomStatusAndPrices("102", new Price("EUR", 109), new Price("EUR", 140)), new RoomStatusAndPrices("201", new Price("EUR", 209), new Price("EUR", 240)) });

            var generatedFilePath = SerializeToJsonFile(roomsAvailability);
            Console.WriteLine($"Integration file generated: {generatedFilePath}");
        }

        public static void GenerateJsonFileForGrandBudapestHotel()
        {
            var hotelId = 2;
            var hotelName = "THE GRAND BUDAPEST HOTEL";
            var location = "Budapest";
            var numberOfRooms = 240;

            var roomsAvailability = new HotelDetailsWithRoomsAvailabilities(hotelId, hotelName, location, numberOfRooms);
            roomsAvailability.AvailabilitiesAt.Add(DateTime.Parse(myFavorite2017Saturday), new RoomStatusAndPrices[] { new RoomStatusAndPrices("101", new Price("EUR", 109), new Price("EUR", 140)), new RoomStatusAndPrices("102", new Price("EUR", 109), new Price("EUR", 140)), new RoomStatusAndPrices("201", new Price("EUR", 209), new Price("EUR", 240)) });

            var generatedFilePath = SerializeToJsonFile(roomsAvailability);
            Console.WriteLine($"Integration file generated: {generatedFilePath}");
        }

        public static void GenerateJsonFileForDanubiusHealthSpaResortHelia()
        {
            var hotelId = 3;
            var hotelName = "Danubius Health Spa Resort Helia";
            var location = "Budapest";
            var numberOfRooms = 125;

            var roomsAvailability = new HotelDetailsWithRoomsAvailabilities(hotelId, hotelName, location, numberOfRooms);
            roomsAvailability.AvailabilitiesAt.Add(DateTime.Parse(myFavorite2017Saturday), new RoomStatusAndPrices[] { new RoomStatusAndPrices("101", new Price("EUR", 109), new Price("EUR", 140)) });

            var generatedFilePath = SerializeToJsonFile(roomsAvailability);
            Console.WriteLine($"Integration file generated: {generatedFilePath}");
        }

        public static void GenerateJsonFileForBudaFullAlwaysUnavailable()
        {
            var hotelId = 4;
            var hotelName = "BudaFull-the-always-unavailable-hotel";
            var location = "Budapest";
            var numberOfRooms = 5;

            var roomsAvailability = new HotelDetailsWithRoomsAvailabilities(hotelId, hotelName, location, numberOfRooms);
            roomsAvailability.AvailabilitiesAt.Add(DateTime.Parse(myFavorite2017Saturday), new RoomStatusAndPrices[] { });

            var generatedFilePath = SerializeToJsonFile(roomsAvailability);
            Console.WriteLine($"Integration file generated: {generatedFilePath}");
        }

        private static string SerializeToJsonFile(HotelDetailsWithRoomsAvailabilities hotelDetailsWithRoomsAvailabilities)
        {
            var jsonContent = JsonConvert.SerializeObject(hotelDetailsWithRoomsAvailabilities, Formatting.Indented);
            var fileName = hotelDetailsWithRoomsAvailabilities.HotelName + "-availabilities.json";
            
            // ensures the full paht directory exist
            Directory.CreateDirectory(WhereToGenerateDirectoryFullPath);

            // Generate JSON file
            var fileFullPath = Path.Combine(WhereToGenerateDirectoryFullPath, fileName);
            File.WriteAllText(fileFullPath, jsonContent, Encoding.UTF8);

            return fileFullPath;
        }
    }
}