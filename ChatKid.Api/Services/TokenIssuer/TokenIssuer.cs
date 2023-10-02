using ChatKid.Api.Services.TokenIssuer;
using ChatKid.ApiFramework.Authentication;
using ChatKid.ApiFramework.AuthJwtIssuer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace ChatKid.ApiFramework.AuthTokenIssuer
{
    public class TokenIssuer : ITokenIssuer
    {
        private readonly IJwtTokenIssuer jwtTokenIssuer;
        private readonly AuthenticationSettings authenticationSettings;
        private readonly TrustedIssuerSettings trustedIssuerSettings;

        public TokenIssuer(
            IJwtTokenIssuer jwtTokenIssuer,
            AuthenticationSettings authenticationSettings,
            TrustedIssuerSettings trustedIssuerSettings)
        {
            this.jwtTokenIssuer = jwtTokenIssuer;
            this.authenticationSettings = authenticationSettings;
            this.trustedIssuerSettings = trustedIssuerSettings;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var tokenId = Guid.Empty;
            return GenerateJwtToken(claims, DateTime.Now.AddMinutes(this.authenticationSettings.ExpiryMinutes), ref tokenId);
        }

        public async Task<IEnumerable<Claim>> GenerateJwtClaims(ClaimModel user)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "kidtalkie"),
                new Claim(CustomJwtRegisteredClaimNames.DisplayName, user.FullName ?? ""),
                new Claim(CustomJwtRegisteredClaimNames.LastName, user.LastName ?? ""),
                new Claim(CustomJwtRegisteredClaimNames.FirstName, user.FirstName ?? ""),
                new Claim(CustomJwtRegisteredClaimNames.UserId, $"{ user.Id }"),
                new Claim(CustomJwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(CustomJwtRegisteredClaimNames.ChatKidClaimName, "BeztTeamEver"),
            };

            return claims;
        }

        private string GenerateJwtToken(IEnumerable<Claim> claims, DateTime expiryTime, ref Guid tokenId)
        {
            return this.jwtTokenIssuer.GenerateJwtToken(claims, expiryTime.ToUniversalTime(), trustedIssuerSettings.Issuer, trustedIssuerSettings.Url, ref tokenId);
        }
    }
}
