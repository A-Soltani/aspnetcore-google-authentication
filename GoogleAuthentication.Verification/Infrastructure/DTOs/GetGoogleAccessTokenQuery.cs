namespace GoogleAuthentication.Verification.Infrastructure.DTOs
{
    public class GetGoogleAccessTokenQuery /*: IQuery<GoogleAccessTokenModel>*/
    {
        public string Code { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string GrantType { get; set; }
        public string RedirectUri { get; set; }
        public string TokenUrl { get; set; }
    }
}