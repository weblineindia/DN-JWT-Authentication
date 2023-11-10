using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace WLI.JwtAuthentication.TokenOption
{
    public class GenerateJwtTokenOptions : JwtTokenOptions
    {
        public string Email { get; set; }
        public string RefreshToken { get; set; }
        public string Role { get; set; }
        public int AccessTokenExpirationTime { get; set; }
        public int RefreshTokenExpirationTime { get; set; }
        public List<Claim> Claims { get; set; }
    }
}