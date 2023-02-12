namespace LikeKero.Contract.Responses.User
{
    public class UserForgotPasswordResponse : BaseResponse
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }
}
