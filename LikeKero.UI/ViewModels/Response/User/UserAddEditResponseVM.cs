namespace LikeKero.UI.ViewModels
{
    public class UserAddEditResponseVM : BaseResponseVM
    {
        public string UserId { get; set; }
        public string Message { get; set; }
        public string OIGMessage { get; set; }
        public bool IsOIGChecked { get; set; }
        public string OIGCheckURL { get; set; }
        public int NoOfHits { get; set; }
    }
}
