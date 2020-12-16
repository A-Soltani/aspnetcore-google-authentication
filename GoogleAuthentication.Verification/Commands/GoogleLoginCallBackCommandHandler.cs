using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GoogleAuthentication.Verification.Infrastructure;
using GoogleAuthentication.Verification.Models;

namespace GoogleAuthentication.Verification.Commands
{
    public interface IGoogleLoginCallBackCommandHandler
    {
        Task<JsonWebToken> HandleAsync(GoogleLoginCallBackCommand googleLoginCallBackCommand);
    }

    public class GoogleLoginCallBackCommandHandler : IGoogleLoginCallBackCommandHandler
    {
        private readonly IGoogleAuthentication _googleAuthentication;

        public GoogleLoginCallBackCommandHandler(IGoogleAuthentication googleAuthentication)
        {
            _googleAuthentication = googleAuthentication ?? throw new ArgumentNullException(nameof(googleAuthentication));
        }

        public async Task<JsonWebToken> HandleAsync(GoogleLoginCallBackCommand googleLoginCallBackCommand)
        {
            var googleUser = await _googleAuthentication.VerifyGoogleUser(googleLoginCallBackCommand.TokenId);
            if (string.IsNullOrWhiteSpace(googleUser?.Email))
                return new JsonWebToken
                {
                    RequiresVerification = true
                };

            var jwtTokensModel = new JsonWebToken();
            
            return jwtTokensModel;

        }
    }
}
