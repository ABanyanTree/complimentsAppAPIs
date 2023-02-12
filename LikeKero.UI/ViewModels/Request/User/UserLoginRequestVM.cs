using System.ComponentModel.DataAnnotations;

namespace LikeKero.UI.ViewModels
{
    public class UserLoginRequestVM : BaseRequestVM
    {
        [Required(ErrorMessage = "Please enter LoginID")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Please enter Password")]
        public string Password { get; set; }       
        public string Salt { get; set; }
    }
}
