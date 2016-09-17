using System;
using System.Collections.Generic;

namespace BookARoom.Infra.Adapters
{
    public class PlaceCatalogFileAdaptor : ICatalogPlaces
    {
        public PlaceCatalogFileAdaptor(string integrationFilesDirectoryPath)
        {
            this.IntegrationFilesDirectoryPath = integrationFilesDirectoryPath;
        }

        public string IntegrationFilesDirectoryPath { get; }

        public IEnumerable<Place> SearchFromLocation(string location)
        {
            throw new NotImplementedException();
        }
    }
}