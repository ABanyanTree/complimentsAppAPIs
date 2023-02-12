using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.ViewModels
{
    public class CreateTestCustomerRequestVM : BaseRequestVM
    {
        public string CustID { get; set; }
       
        [RegularExpression(@"^[a-zA-Z ']*$", ErrorMessage = "Please enter valid First Name.")]
        [Required(ErrorMessage = "Please enter First Name")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z ']*$", ErrorMessage = "Please enter valid Last Name.")]
        [Required(ErrorMessage = "Please enter Last Name")]

        public string LastName { get; set; }
        public string MobNo { get; set; }
        public string Address { get; set; }
        [Required(ErrorMessage = "Please select Gender")]
        public string Gender { get; set; }
        public string FileName { get; set; }
        public List<IFormFile> uploadedFile { get; set; }
    }
}
