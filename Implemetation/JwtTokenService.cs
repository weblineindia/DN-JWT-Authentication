using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WLI.JwtAuthentication.Interface;
using WLI.JwtAuthentication.TokenOption;

namespace WLI.JwtAuthentication
{
    public class JwtTokenService : IJwtTokenService
    {
        public Tokens JwtToken(GenerateJwtTokenOptions options)
        {   
            //Generate a access JWTToken 
            var tokenString = GenerateToken(options.Claims, options.AccessTokenExpirationTime, options.Key, options.Audience, options.Issuer);

            // Generate a refresh token with a longer expiration    
            var refreshTokenString = GenerateToken(options.Claims, options.RefreshTokenExpirationTime, options.Key, options.Audience, options.Issuer);
            var TokenModel = new Tokens
            {
                AccessToken = tokenString,
                RefreshToken = refreshTokenString
            };
            return TokenModel;
        }
       
        public Tokens RefreshAccessToken(GenerateJwtTokenOptions options)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key));
            var refreshTokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = options.ValidateIssuerSigningKey,
                IssuerSigningKey = securityKey,
                ValidIssuer = options.Issuer,
                ValidAudience = options.Audience,
                ValidateIssuer = options.ValidateIssuer,
                ValidateAudience = options.ValidateAudience,
                ValidateLifetime = options.ValidateLifetime
            };

            ClaimsPrincipal claimsPrincipal;
            try
            {
                claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(options.RefreshToken, refreshTokenValidationParameters, out _);
            }
            catch (Exception ex)
            {
                return null;
            }
            // Check if the refresh token is still within the valid timeframe
            var refreshTokenExp = claimsPrincipal.FindFirst("exp")?.Value;
            var refreshTokenExpTime = Convert.ToInt64(refreshTokenExp);
            var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var refreshTokenTimeRemaining = refreshTokenExpTime - currentTime;

            if (refreshTokenTimeRemaining <= 0)
            {
                // Refresh token has expired, cannot issue new access token
                return null;
            }

            // Generate a new set of access and refresh tokens
            var newTokens = GenerateJwtTokensFromClaims(claimsPrincipal.Claims, options.Key, options.Issuer, options.Audience, options.AccessTokenExpirationTime, options.RefreshTokenExpirationTime);

            return newTokens;
        }

        private Tokens GenerateJwtTokensFromClaims(IEnumerable<Claim> claims, string key, string issuer, string audience, int tokenExpirationTime, int refreshTokenExpirationTime)
        {
            //Generate a access JWTToken 
            var accessTokenString = GenerateToken(claims, tokenExpirationTime, key, audience, issuer);

            // Generate a refresh token with a longer expiration    
            var refreshTokenString = GenerateToken(claims, refreshTokenExpirationTime, key, audience, issuer);

            var TokenModel = new Tokens
            {
                AccessToken = accessTokenString,
                RefreshToken = refreshTokenString
            };
            return TokenModel;
        }

        private string GenerateToken(IEnumerable<Claim> claims, int expirationTime, string key, string audience, string issuer)
        {
            var tokenExpiration = DateTime.UtcNow.AddMinutes(expirationTime);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var claimDictionary = claims.ToDictionary(c => c.Type, c => (object)c.Value);
            var refreshSecurityToken = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Claims = claimDictionary,
                Expires = tokenExpiration,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = signingCred
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(refreshSecurityToken);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
    }
}