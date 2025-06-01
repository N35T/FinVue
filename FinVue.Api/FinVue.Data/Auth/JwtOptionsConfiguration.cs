using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;

namespace FinVue.Data.Auth; 

public class JwtOptionsConfiguration : IConfigureNamedOptions<JwtBearerOptions> {
    private readonly JwtHandler _jwtHandler;
    
    public JwtOptionsConfiguration(JwtHandler jwtHandler) {
        _jwtHandler = jwtHandler;
    }
    
    public void Configure(JwtBearerOptions options) {
        Configure(string.Empty, options);
    }

    public void Configure(string? name, JwtBearerOptions options) {
        if (name == JwtBearerDefaults.AuthenticationScheme) {
            options.TokenValidationParameters = _jwtHandler.Parameters;
            options.Events = new JwtBearerEvents() {
                OnMessageReceived = ctx => {
                    ctx.Token = ctx.Request.Cookies["identity-token"];
                    return Task.CompletedTask;
                }
            };
        }
    }
}