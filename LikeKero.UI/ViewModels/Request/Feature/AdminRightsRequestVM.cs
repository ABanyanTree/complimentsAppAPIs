using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LikeKero.UI.ViewModels.Request.Course
{
    public class AdminRightsRequestVM : BaseRequestVM
    {
        public string RoleFeatureId { get; set; }
        public string RoleId { get; set; }
        public string FeatureId { get; set; }
        public bool Status { get; set; }
        public string RequesterUserId { get; set; }
    }
}
