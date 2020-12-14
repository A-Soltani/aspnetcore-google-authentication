using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleAuthentication.Verification.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoogleAuthentication.Verification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGoogleLoginCallBackCommandHandler _googleLoginCallBackCommandHandler;

        public UserController(IGoogleLoginCallBackCommandHandler googleLoginCallBackCommandHandler)
        {
            _googleLoginCallBackCommandHandler = googleLoginCallBackCommandHandler ?? throw new ArgumentNullException(nameof(googleLoginCallBackCommandHandler));
        }

        [AllowAnonymous]
        [HttpGet("GoogleCallBack")]
        public async Task<IActionResult> GoogleCallBack(string tokenId)
        {
            var googleLoginCallBackCommand = new GoogleLoginCallBackCommand()
            {
                TokenId = tokenId
            };
            var jsonWebToken = await _googleLoginCallBackCommandHandler.HandleAsync(googleLoginCallBackCommand);

            return Ok(jsonWebToken);
        }
    }
}