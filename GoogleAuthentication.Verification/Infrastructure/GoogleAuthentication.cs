using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth;
using GoogleAuthentication.Verification.Infrastructure.DTOs;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Refit;
using UserManagement.Acl.GoogleAuthentication;

namespace GoogleAuthentication.Verification.Infrastructure
{
    public interface IGoogleAuthentication
    {
        Task<GoogleUserModel> GetGoogleUser(string code);
        Task<GoogleAccessTokenModel> GetGoogleAccessToken(string code);
        Task<GoogleUserModel> VerifyGoogleUser(string idToken);

    }

    public class GoogleAuthentication : IGoogleAuthentication
    {
        private readonly GoogleAuthenticationConfig _googleAuthenticationConfig;

        public GoogleAuthentication(GoogleAuthenticationConfig googleAuthenticationConfig)
        {
            _googleAuthenticationConfig = googleAuthenticationConfig ?? throw new ArgumentNullException(nameof(googleAuthenticationConfig));
        }

        public async Task<GoogleUserModel> GetGoogleUser(string code)
        {
            var googleAccessToken = await GetGoogleAccessToken(code);
            if (googleAccessToken == null) return null;

            var googleAuthenticationApi = RestService.For<IGoogleAuthenticationApi>(_googleAuthenticationConfig.UrlProfile);
            var googleUserModel = await googleAuthenticationApi.GetUserProfile(googleAccessToken.AccessToken);

            return googleUserModel;
        }

        public async Task<GoogleAccessTokenModel> GetGoogleAccessToken(string code)
        {
            var getGoogleAccessTokenQuery = new GetGoogleAccessTokenQuery
            {
                ClientId = _googleAuthenticationConfig.ClientId,
                ClientSecret = _googleAuthenticationConfig.ClientSecret,
                Code = code,
                GrantType = _googleAuthenticationConfig.GrantType,
                RedirectUri = _googleAuthenticationConfig.RedirectUrl,
                TokenUrl = _googleAuthenticationConfig.TokenUrl
            };

            var googleAuthenticationApi = RestService.For<IGoogleAuthenticationApi>(getGoogleAccessTokenQuery.TokenUrl);
            var googleAccessToken = await googleAuthenticationApi.GetAccessToken(getGoogleAccessTokenQuery);

            return googleAccessToken;
        }

        public async Task<GoogleUserModel> VerifyGoogleUser(string idToken)
        {
            GoogleJsonWebSignature.ValidationSettings settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new List<string>() { "223160961578-9pn2lc00p2qvs53o6ikbcf5bpuj047qt.apps.googleusercontent.com" }
            };

            try
            {
                var validated = GoogleJsonWebSignature.ValidateAsync(idToken,
                    new GoogleJsonWebSignature.ValidationSettings()
                    {
                        //HostedDomain = "https://stage.bitraman.com",
                        Audience = new List<string>() { "223160961578-9pn2lc00p2qvs53o6ikbcf5bpuj047qt.apps.googleusercontent.com" }
                    }).Result;

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

                //var googleUserModel = ValidateToken(idToken);
                //if (validPayload == null)
                //    return null;
                return null;
                //return new GoogleUserModel()
                //{
                //    Email = validPayload.Email,
                //    Id = validPayload.JwtId,
                //    Name = validPayload.Name,
                //    GivenName = validPayload.GivenName
                //};
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //private async Task<GoogleUserModel> GoogleJsonWebSignatureValidate(string idToken)
        //{
        //    var jwtToken = new JwtSecurityToken(idToken);
        //    var googleUserModel = new GoogleUserModel()
        //    {
        //        Email = jwtToken.Payload.FirstOrDefault(s => s.Key == "email").Value.ToString(),
        //        Id = jwtToken.Payload.Claims.Where(t=>t.)

        //    };



        //    var t1 = jwtToken.Payload.FirstOrDefault(s => s.Key == "email").Value;

        //    return;
        //}

        public ClaimsPrincipal ValidateToken(string jwtToken)
        {
            //IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = "223160961578-9pn2lc00p2qvs53o6ikbcf5bpuj047qt.apps.googleusercontent.com".ToLower();
            validationParameters.ValidIssuer = "accounts.google.com".ToLower();
            //validationParameters.IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes("bCqXgPFypEmtsOUw3W4AiAXg"));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);


            return principal;
        }
    }
}
