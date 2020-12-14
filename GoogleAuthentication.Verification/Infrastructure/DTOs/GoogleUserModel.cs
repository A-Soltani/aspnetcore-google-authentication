﻿namespace GoogleAuthentication.Verification.Infrastructure.DTOs
{
    public class GoogleUserModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string GivenName { get; set; }
        public string Email { get; set; }
        public string Picture { get; set; }
    }
}