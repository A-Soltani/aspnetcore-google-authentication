using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Apis.Auth;
using GoogleAuthentication.Verification.Infrastructure.DTOs;
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
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>()
                {
                    "223160961578-9pn2lc00p2qvs53o6ikbcf5bpuj047qt.apps.googleusercontent.com"
                }
            };
            try
            {

                var validPayload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
                if (validPayload == null)
                    return null;

                return new GoogleUserModel()
                {
                    Email = validPayload.Email,
                    Id = validPayload.JwtId,
                    Name = validPayload.Name,
                    GivenName = validPayload.GivenName
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
