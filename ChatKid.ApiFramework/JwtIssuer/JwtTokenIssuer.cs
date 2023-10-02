using ChatKid.ApiFramework.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatKid.ApiFramework.AuthJwtIssuer
{
    public class JwtTokenIssuer : IJwtTokenIssuer
    {
        private readonly AuthenticationSettings authenticationSettings;

        public JwtTokenIssuer(AuthenticationSettings authenticationSettings)
        {
            this.authenticationSettings = authenticationSettings;
        }

        public string GenerateJwtToken(IEnumerable<Claim> claims, DateTime expiryTime, string issuer, string audience, ref Guid tokenId)
        {
            tokenId = tokenId == Guid.Empty ? Guid.NewGuid() : tokenId;

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims.Concat(new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, $"{tokenId}")
                }),
                DateTime.UtcNow,
                expiryTime,
                new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.authenticationSettings.Key)), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
