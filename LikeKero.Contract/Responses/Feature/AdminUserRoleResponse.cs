namespace LikeKero.Contract.Responses.Feature
{
    public class AdminUserRoleResponse : BaseResponse
    {
        public string RoleId { get; set; }
        public string DisplayRole { get; set; }
        public string RoleName { get; set; }
        public int SequenceNo { get; set; }
    }
}
