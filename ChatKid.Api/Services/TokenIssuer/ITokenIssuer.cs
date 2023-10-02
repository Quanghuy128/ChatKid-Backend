using ChatKid.Api.Models;
using ChatKid.Api.Services.TokenIssuer;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ChatKid.ApiFramework.AuthTokenIssuer
{
    public interface ITokenIssuer
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        Task<IEnumerable<Claim>> GenerateJwtClaims(ClaimModel user);
    }
}
