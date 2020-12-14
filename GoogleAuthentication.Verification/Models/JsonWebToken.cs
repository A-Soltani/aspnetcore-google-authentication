using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace GoogleAuthentication.Verification.Models
{
    public class JsonWebToken
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset expires_in { get; set; }
        public DateTimeOffset RefreshTokenExpires { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
        public bool RequiresTwoFactor { get; set; }
        public bool RequiresVerification { get; set; }
    }
}