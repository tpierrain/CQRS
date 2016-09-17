using System;
using System.Collections.Generic;
using System.IO;
using BookARoom.Domain;

namespace BookARoom.Infra.Adapters
{
    public class PlaceCatalogFileAdapter : ICatalogPlaces
    {
        private Dictionary<Place, Dictionary<DateTime, List<RoomStatus>>> placesWithPerDateRoomStatus;

        public PlaceCatalogFileAdapter(string integrationFilesDirectoryPath)
        {
            this.placesWithPerDateRoomStatus = new Dictionary<Place, Dictionary<DateTime, List<RoomStatus>>>();
            this.IntegrationFilesDirectoryPath = integrationFilesDirectoryPath;
        }

        public IEnumerable<Place> Places => this.placesWithPerDateRoomStatus.Keys;

        public string IntegrationFilesDirectoryPath { get; }

        public IEnumerable<Place> SearchFromLocation(string location)
        {
            throw new NotImplementedException();
        }

        public void LoadPlaceFile(string placeFilePath)
        {
            using (var streamReader = File.OpenText(placeFilePath))
            {
                var jsonContent = streamReader.ReadToEnd();
                throw new NotImplementedException();
            }
        }
    }
}