using BaseTemplate.Repository.Elastic.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Serilog.Core;
using System.Security.Claims;
using System.Text;

namespace BaseTemplate.Presentation.Configurations
{
    public static class JwtConfiguration
    {
        public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
        {

            opt.TokenValidationParameters = new()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = configuration["Token:Audience"],
                ValidIssuer = configuration["Token:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:SecurityKey"])),
                LifetimeValidator = (notBefore, expires, securityToken, validationParamters) => expires != null ? expires > DateTime.UtcNow : false,
                NameClaimType = ClaimTypes.Name
            };
            opt.Events = new JwtBearerEvents()
            {
                OnChallenge = context =>
                {
                    context.Response.StatusCode = 401;
                    context.ErrorDescription = "Authentication Failed.";
                    context.HandleResponse();
                    var payload = new JObject
                    {
                        ["errorCode"] = context.Response.StatusCode,
                        ["errorDescription"] = context.ErrorDescription,
                    };

                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = 401;

                    return context.Response.WriteAsync(payload.ToString());
                }
            };
        });
        }
    }

}
