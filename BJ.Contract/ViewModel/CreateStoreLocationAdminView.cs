using BJ.Contract.StoreLocation;
using BJ.Contract.Translation.Store;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Contract.ViewModel
{
    public class CreateStoreLocationAdminView
    {
        public CreateStoreLocationDto CreateStoreLocationDto { get; set; } = new CreateStoreLocationDto();
        public CreateStoreLocationTranslationDto CreateStoreLocationTranslation { get; set; } = new CreateStoreLocationTranslationDto();
        public IFormFile ImageStore { get; set; }

    }
}
