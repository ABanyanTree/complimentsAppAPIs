namespace LikeKero.Domain
{
    public class UserRole 
    {
        public string RoleId { get; set; }
        public string DisplayRole { get; set; }
        public string RoleName { get; set; }
        public string SearchOnGroupId { get; set; }
        public bool IsActive { get; set; }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public bool Status { get; set; }
        public string UserRoleId { get; set; }
        public int SequenceNo { get; set; }
        public int TotalAdmin { get; set; }
    }
}
