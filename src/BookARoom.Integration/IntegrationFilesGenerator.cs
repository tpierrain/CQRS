using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace BookARoom.Integration
{
    public static class IntegrationFilesGenerator
    {
        public static string WhereToGenerateDirectoryFullPath => Path.GetFullPath(AppContext.BaseDirectory + @"..\..\..\..\..\..\IntegrationFiles\");

        public static void GenerateJsonFileForNewYorkSofitel()
        {
            var hotelName = "New York Sofitel";
            var roomsAvailability =  new RoomsAvailability(hotelName);
            roomsAvailability.AvailabilitiesAt.Add(DateTime.Parse("2016-09-11"), new RoomStatus[] { new RoomStatus("101", new Price("EUR", 109), new Price("EUR", 140)), new RoomStatus("102", new Price("EUR", 109), new Price("EUR", 140)), new RoomStatus("201", new Price("EUR", 209), new Price("EUR", 240)) });

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