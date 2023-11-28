using System.ComponentModel.DataAnnotations;

namespace BJ.Domain.Entities
{
    public class Account
    {
        [Key]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string EmployeeName { get; set; }
        public string HasedPassword { get; set; }
        public bool Active { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public AuthorizeRoles AuthorizeRole { get; set; }


    }
}
