using System;

namespace LikeKero.Infra.BaseUri
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        private readonly string _profilePic;

        public UriService()
        {
        }
        public UriService(string BaseUri, string ProfilePicName)
        {
            _baseUri = BaseUri;
            _profilePic = ProfilePicName;
        }

        public Uri GetBaseUri()
        {
            return new Uri(_baseUri);
        }

        public string GetDefaultProfilePic()
        {
            return _profilePic;
        }
    }
}
