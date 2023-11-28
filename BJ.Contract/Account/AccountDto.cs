namespace BJ.Contract.Account
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string EmployeeName { get; set; }
        public string HasedPassword { get; set; }
        public bool Active { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public AuthorizeRole AuthorizeRole { get; set; }

    }
}
