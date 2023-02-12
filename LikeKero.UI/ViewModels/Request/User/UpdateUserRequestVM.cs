namespace LikeKero.UI.ViewModels
{
    public class GetUserRequestVM : BaseRequestVM
    {
        public string UserId { get; set; }
        public string RequesterUserId { get; set; }
        public string FirstName { get; set; }        
        public string LastName { get; set; }        
    }
}
