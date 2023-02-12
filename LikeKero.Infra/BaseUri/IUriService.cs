using System;

namespace LikeKero.Infra.BaseUri
{
    public interface IUriService
    {
        Uri GetBaseUri();
        string GetDefaultProfilePic();
        
    }
}
