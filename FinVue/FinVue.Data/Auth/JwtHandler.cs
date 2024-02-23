using System.Security.Cryptography;
using FinVue.Data.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FinVue.Data.Auth; 

public class JwtHandler : IDisposable {
    private readonly string _issuer;
    private readonly string _audience;
    
    private SecurityKey _issuerSigningKey;
    private RSA _publicRsa;
    
    public TokenValidationParameters Parameters { get; private set; }
    
    public JwtHandler(IConfiguration config) {
        _issuer = config.GetJwtIssuer();
        _audience = config.GetJwtAudience();
        
        ConfigureRsa(config.GetJwtPublicKeyPath());
        ConfigureJwtParameters();
    }

    private void ConfigureRsa(string publicKeyPath) {
        _publicRsa = RSA.Create();
        var publicKeyXml = File.ReadAllText(publicKeyPath);
        _publicRsa.FromXmlString(publicKeyXml);
        _issuerSigningKey = new RsaSecurityKey(_publicRsa);
    }

    private void ConfigureJwtParameters() {
        Parameters = new TokenValidationParameters {
            ValidIssuer = _issuer,
            ValidAudience = _audience,
            IssuerSigningKey = _issuerSigningKey,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    }

    public void Dispose() {
        _publicRsa?.Dispose();
    }
}