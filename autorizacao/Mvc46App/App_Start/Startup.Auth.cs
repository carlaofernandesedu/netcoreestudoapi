using Microsoft.Owin.Security.Cookies;
//using Microsoft.Owin.Security.OAuth;
using Microsoft.Owin.Security;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;

namespace Mvc46App
{
    public partial class Startup
    {
        //public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        //public static string PublicClientId { get; private set; }
        public void ConfigureAuth(IAppBuilder app)
        {
        }

        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            //// Enable the application to use a cookie to store information for the signed in user  
            //// and to use a cookie to temporarily store information about a user logging in with a third party login provider  
            //// Configure the sign in cookie  
            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    LoginPath = new PathString("/Account/Login"),
            //    LogoutPath = new PathString("/Account/LogOff"),
            //    ExpireTimeSpan = TimeSpan.FromMinutes(5.0),
            //});

            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //// Configure the application for OAuth based flow  
            //PublicClientId = "self";
            //OAuthOptions = new OAuthAuthorizationServerOptions
            //{
            //    TokenEndpointPath = new PathString("/Token"),
            //    Provider = new AppOAuthProvider(PublicClientId),
            //    AuthorizeEndpointPath = new PathString("/Account/ExternalLogin"),
            //    AccessTokenExpireTimeSpan = TimeSpan.FromHours(4),
            //    AllowInsecureHttp = true //Don't do this in production ONLY FOR DEVELOPING: ALLOW INSECURE HTTP!  
            //};

            //// Enable the application to use bearer tokens to authenticate users  
            //app.UseOAuthBearerTokens(OAuthOptions);

        }

        private void ConfigureOAuthTokenConsumption(IAppBuilder app)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap = 
            //new Dictionary<string, string>();
            // before v5.0 was: JwtSecurityTokenHandler.InboundClaimTypeMap

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                ClientId = "mvcnetfull",
                Authority = "http://localhost:5000",
                //RedirectUri = < base address of your app as registered in IdP >,
                //ResponseType = "id_token",
                //Scope = "openid email",
                //UseTokenLifetime = false,
                SaveTokens = true,
                SignInAsAuthenticationType = "Cookies"
            });

            //var issuer = "http://localhost:59822";
            //string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            //byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

            //// Api controllers with an [Authorize] attribute will be validated with JWT
            //app.UseJwtBearerAuthentication(
            //    new JwtBearerAuthenticationOptions
            //    {
            //        AuthenticationMode = AuthenticationMode.Active,
            //        AllowedAudiences = new[] { audienceId },
            //        IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
            //        {
            //            new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
            //        }
            //    });
        }
    }
}