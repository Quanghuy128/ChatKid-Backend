using ChatKid.Api.Models;
using ChatKid.Api.Services.TokenIssuer;
using ChatKid.ApiFramework.AuthTokenIssuer;
using ChatKid.Common.Providers;
using ChatKid.GoogleServices.GoogleAuthentication;
using ChatKid.GoogleServices.GoogleGmail;
using Google.Apis.Oauth2.v2.Data;
using KMS.Healthcare.TalentInventorySystem.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using ChatKid.GoogleServices.GoogleSettings;
using ChatKid.Api.Models.RequestModels;
using ChatKid.Api.Services.Validators;
using ChatKid.Common.CommandResult;

namespace ChatKid.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly ITokenIssuer _tokenIssuer;
        private readonly GoogleResponse _googleResponse;

        private readonly IGoogleAuthenticationService googleAuthenticationService;
        private readonly IGoogleGmailService googleGmailService;
        public AuthenticationController(IGoogleAuthenticationService googleAuthenticationService,
            IGoogleGmailService googleGmailService,
            ITokenIssuer tokenIssuer,
            GoogleResponse googleResponse)
        {
            this.googleAuthenticationService = googleAuthenticationService;
            this.googleGmailService = googleGmailService;
            _tokenIssuer = tokenIssuer;
            _googleResponse = googleResponse;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginTokenResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Login([FromBody] UserLogin model)
        {
            var validator = new UserLoginValidator().Validate(model);
            if (!validator.IsValid)
            {
                var errors = validator.Errors.Select(error => error.ErrorMessage);
                return BadRequest(errors);
            }

            ClaimModel claimModel = new()
            {
                Id = "",
                Email = "",
                FullName = "",
                LastName = model.UserName,
                FirstName = model.Password,
            };

            var claims = await _tokenIssuer.GenerateJwtClaims(claimModel);
            var token = _tokenIssuer.GenerateAccessToken(claims);

            LoginTokenResponse authResponse = new LoginTokenResponse()
            {
                Token = token,
                RefreshToken = LocalIPAddressProvider.EncryptIPAddress()
            };
            return Ok(authResponse);
        }

        [HttpPost("google-auth")]
        [ProducesResponseType(typeof(LoginTokenResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GoogleAuthentication([FromBody] GoogleAccessRequest request)
        {
            var validator = new GoogleAccessValidator().Validate(request);
            if (!validator.IsValid)
            {
                var errors = validator.Errors.Select(error => error.ErrorMessage);
                return BadRequest(errors);
            }

            var response = await this.googleAuthenticationService.GoogleLogin(request.AccessToken);
            if (response.Succeeded)
            {
                Userinfo userinfo = (Userinfo)response.Data;
                ClaimModel model = new()
                {
                    Id = userinfo.Id,
                    Email = userinfo.Email,
                    LastName = userinfo.FamilyName,
                    FirstName = userinfo.GivenName,
                    FullName = userinfo.Name
                };
                if (model == null) return Unauthorized();

                var claims = await _tokenIssuer.GenerateJwtClaims(model);
                var token = _tokenIssuer.GenerateAccessToken(claims);

                LoginTokenResponse authResponse = new LoginTokenResponse()
                {
                    Token = token,
                    RefreshToken = LocalIPAddressProvider.EncryptIPAddress()
                };
                return Ok(authResponse);
            }
            return StatusCode(response.GetStatusCode(), response.GetData());
        }

        [HttpPost("userinfo")]
        [ProducesResponseType(typeof(LoginTokenResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetUserInfo([FromBody] GoogleAccessRequest request)
        {
            var validator = new GoogleAccessValidator().Validate(request);
            if (!validator.IsValid)
            {
                var errors = validator.Errors.Select(error => error.ErrorMessage);
                return BadRequest(errors);
            }
            var response = await this.googleAuthenticationService.GoogleLogin(request.AccessToken);
            if(!response.Succeeded) return StatusCode(response.GetStatusCode(), response.GetData());
            return Ok(response.GetData());
        }
    }
}
