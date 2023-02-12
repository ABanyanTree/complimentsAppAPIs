namespace LikeKero.Contract.Requests.User
{
    public class UserForgotPasswordRequest : BaseRequest
    {
        public string EmailAddress { get; set; }
    }
}
