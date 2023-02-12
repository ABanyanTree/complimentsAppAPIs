using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LikeKero.Api.options
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public TimeSpan TokenLifeTime { get; set; }

    }
}
