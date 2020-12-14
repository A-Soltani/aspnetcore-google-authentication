namespace UserManagement.Acl.GoogleAuthentication
{
    public interface IGoogleAuthenticationConfig
    {
        string ClientId { get; set; }
        string RedirectUrl { get; set; }
        string ClientSecret { get; set; }
        string TokenUrl { get; set; }
        string GrantType { get; set; }
        string OAuthUrl { get; set; }
        string ResponseType { get; set; }
        string Scope { get; set; }
        string UiCallBack { get; set; }
        string UrlProfile { get; set; }
    }

    public class GoogleAuthenticationConfig : IGoogleAuthenticationConfig
    {
        public string ClientId { get; set; }
        public string RedirectUrl { get; set; }
        public string ClientSecret { get; set; }
        public string TokenUrl { get; set; }
        public string GrantType { get; set; }
        public string OAuthUrl { get; set; }
        public string ResponseType { get; set; }
        public string Scope { get; set; }
        public string UiCallBack { get; set; }
        public string UrlProfile { get; set; }
    }
}