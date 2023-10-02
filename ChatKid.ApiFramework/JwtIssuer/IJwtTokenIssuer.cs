using System.Security.Claims;

namespace ChatKid.ApiFramework.AuthJwtIssuer
{
    public interface IJwtTokenIssuer
    {
        public string GenerateJwtToken(IEnumerable<Claim> claims, DateTime expiryTime, string issuer, string audience, ref Guid tokenId);
    }
}
