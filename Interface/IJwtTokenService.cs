using WLI.JwtAuthentication.TokenOption;

namespace WLI.JwtAuthentication.Interface
{
    public interface IJwtTokenService
    {
        public Tokens JwtToken(GenerateJwtTokenOptions options);
        public Tokens RefreshAccessToken(GenerateJwtTokenOptions options);
    }
}