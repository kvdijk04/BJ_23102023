using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Contract
{
    public class ImportResponse
    {
        public IFormFile File { get; set; }
        public string Msg { get; set; }
    }
}
