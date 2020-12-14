using System.Threading.Tasks;
using GoogleAuthentication.Verification.Infrastructure.DTOs;
using Refit;

namespace UserManagement.Acl.GoogleAuthentication
{
    public interface IGoogleAuthenticationApi
    {
        [Post("/token")]
        Task<GoogleAccessTokenModel> GetAccessToken([Query] GetGoogleAccessTokenQuery getGoogleAccessTokenQuery);

        [Get("")]
        Task<GoogleUserModel> GetUserProfile([AliasAs("access_token")] string accessToken);
    }
}