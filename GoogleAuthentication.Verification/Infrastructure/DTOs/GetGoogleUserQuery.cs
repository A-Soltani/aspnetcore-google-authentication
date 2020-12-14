namespace GoogleAuthentication.Verification.Infrastructure.DTOs
{
    public class GetGoogleUserQuery /*: IQuery<GoogleUserModel>*/
    {
        public string AccessToken { get; set; }
    }
}