namespace GoogleAuthentication.Verification.Infrastructure.DTOs
{
    public class GoogleAccessTokenModel
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
        public string IdToken { get; set; }
        public string RefreshToken { get; set; }
    }
}