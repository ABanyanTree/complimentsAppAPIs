using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LikeKero.UI.ViewModels
{
    public class ChangePasswordRequestVM : BaseRequestVM
    {
        [Required(ErrorMessage = "Please enter New Password")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Password should contain at least 8 characters. Password should have minimum 1 uppercase alphabet (A-Z), 1 lowercase alphabet (a-z), 1 special character(@,$,!,%,?,&,*) and 1 numeric character (0-9). Password should not contain first name, last name in it.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please enter Current Password")]
        [DisplayName("Current Password")]
        public string CurrentPassword { get; set; }

        public bool IsPasswordChanged { get; set; }
        [Required(ErrorMessage = "Please enter Confirm Password")]
        [Compare("Password")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
        public string LoginId { get; set; }
        public string UserId { get; set; }
        public bool IsAdminPasswordChangeRequest { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
