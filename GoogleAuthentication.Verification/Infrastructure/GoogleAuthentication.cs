using System;
using System.Threading.Tasks;
using GoogleAuthentication.Verification.Infrastructure.DTOs;
using Refit;
using UserManagement.Acl.GoogleAuthentication;

namespace GoogleAuthentication.Verification.Infrastructure
{
    public interface IGoogleAuthentication
    {
        Task<GoogleUserModel> GetGoogleUser(string code);
        Task<GoogleAccessTokenModel> GetGoogleAccessToken(string code);
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
    }
}
