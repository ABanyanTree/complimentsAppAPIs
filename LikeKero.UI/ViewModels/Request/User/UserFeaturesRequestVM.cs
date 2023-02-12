using System.ComponentModel.DataAnnotations;

namespace LikeKero.UI.ViewModels
{
    public class UserFeaturesRequestVM : BaseRequestVM
    {
        public string UserId { get; set; }
        public string URoleId { get; set; }
    }
}
