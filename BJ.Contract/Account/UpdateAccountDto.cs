namespace BJ.Contract.Account
{
    public class UpdateAccountDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmployeeName { get; set; }
        public string HasedPassword { get; set; }
        public bool Active { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int Role { get; set; }
        public string UserChanged { get;set; }
        public AuthorizeRole AuthorizeRole { get; set; }

    }
}
