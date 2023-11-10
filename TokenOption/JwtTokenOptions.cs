using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WLI.JwtAuthentication.TokenOption
{
    public class JwtTokenOptions
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public bool ValidateActor { get; set; } = true;
        public bool ValidateIssuer { get; set; } = true;
        public bool ValidateAudience { get; set; } = true;
        public bool ValidateLifetime { get; set; } = true;
        public bool ValidateIssuerSigningKey { get; set; } = true;
        public bool RequireExpirationTime { get; set; } = true;
        public JwtBearerEvents Events { get; set; }
    }
}