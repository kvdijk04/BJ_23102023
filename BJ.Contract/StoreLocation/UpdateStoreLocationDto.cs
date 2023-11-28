﻿using Microsoft.AspNetCore.Http;

namespace BJ.Contract.StoreLocation
{
    public class UpdateStoreLocationDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public string IconPath { get; set; }
        public string ImagePath { get; set; }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public bool Closed { get; set; }

        public bool Repaired { get; set; }
        public IFormFile ImageStore { get; set; }

    }
}
