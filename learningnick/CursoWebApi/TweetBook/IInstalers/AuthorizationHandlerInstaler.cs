using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TweetBook.Authorization;
using TweetBook.Options;

namespace TweetBook.IInstalers
{
    public class AuthorizationHandlerInstaler : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
             var authOptions = new AuthorizationModelOptions();
             configuration.GetSection(nameof(AuthorizationModelOptions)).Bind(authOptions);
            

            if (authOptions.Model == "claims")
            {
              services.AddAuthorization(options=>
              {
                options.AddPolicy("PolicyClaimViewer",builder =>{
                    builder.RequireClaim("policiesclaim.view","true");
                });
              });

            }
            else if (authOptions.Model == "custom")
            {
              services.AddAuthorization(options=>
              {
                options.AddPolicy("PolicyCustomViewer",builder =>{
                    builder.AddRequirements(new WorksForCompanyRequirement("xpto.com"));
                });
              });

              services.AddSingleton<IAuthorizationHandler,WorksForCompanyHandler>();
            }
            else if (authOptions.Model == "roles")
            {
              services.AddAuthorization(options =>
                {
                    options.AddPolicy("PolicyRolesViewer",
                      policy => policy.RequireRole("User"));
                });
            }
        }
    }
}