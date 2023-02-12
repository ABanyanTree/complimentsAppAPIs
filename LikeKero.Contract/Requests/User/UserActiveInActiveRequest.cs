namespace LikeKero.Contract.Requests.User
{
    public class UserActiveInActiveRequest : BaseRequest
    {
        public bool Status { get; set; }
        public string UserId { get; set; }
    }
}
