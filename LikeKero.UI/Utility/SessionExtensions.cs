using LikeKero.UI.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.UI.Utility
{
    public static class SessionExtensions
    {
        public static string USERSESSIONKEY = "UserObject";
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);

        }

        public static AuthSuccessResponseVM  GetSessionUser(this ISession session)
        {
            var value = session.GetString(USERSESSIONKEY);

            return value == null ? null : JsonConvert.DeserializeObject<AuthSuccessResponseVM>(value);

        }

        public static void SetSessionUser(this ISession session, object value)
        {
            session.SetString(USERSESSIONKEY, JsonConvert.SerializeObject(value));
        }

        public static string GetBearerToken(this ISession session)
        {
            var value = session.GetString(USERSESSIONKEY);

            var obj = value == null ? new AuthSuccessResponseVM() : JsonConvert.DeserializeObject<AuthSuccessResponseVM>(value);

            if(obj != null && !string.IsNullOrEmpty(obj.Token))
            {
                return obj.Token;
            }


            return string.Empty;

        }

    }
}
