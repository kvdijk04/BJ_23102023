using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BJ.Contract.Account
{
    public class ChangePassword
    {
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string EmployeeName { get; set; }
        public string HasedNewPassword { get; set; }
        public DateTime DateUpdate { get; set; }
    }
}
