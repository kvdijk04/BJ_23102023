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
    public class UpdateStoreLocationAdminView
    {
        public UpdateStoreLocationDto UpdateStoreLocationDto { get; set; }

        public UpdateStoreLocationTranslationDto UpdateStoreLocationTranslationDto { get; set; }

        public IFormFile ImageStore { get; set; }
    }
}
