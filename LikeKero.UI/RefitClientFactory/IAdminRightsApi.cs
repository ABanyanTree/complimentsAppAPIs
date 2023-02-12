using LikeKero.UI.ViewModels;
using LikeKero.UI.ViewModels.Request.Course;
using LikeKero.UI.ViewModels.Response;
using LikeKero.UI.ViewModels.Response.User;
using Microsoft.AspNetCore.Http;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LikeKero.UI.RefitClientFactory
{
    [Headers("Authorization: Bearer")]
    public interface IAdminRightsApi
    {
        #region Vikas
        [Get(path: "/api/feature/getadminfeatures")]
        Task<ApiResponse<AdminRightsResponseVM>> GetAdminFeatures(string RequesterUserId);

        [Post(path: "/api/feature/saveadminrights")]
        Task<HttpResponseMessage> SaveAdminRights(AdminRightsRequestVM singleFeature);

        #endregion
    }
}
