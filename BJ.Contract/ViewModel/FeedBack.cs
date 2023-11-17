using System.ComponentModel.DataAnnotations;

namespace BJ.Contract.ViewModel
{
    public class FeedBack
    {
        public string Reason { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }
        public string VibeMember { get; set; }

        public string StoreName { get; set; }

        public string Message { get; set; }
    }
}
