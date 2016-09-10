/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.GitHub;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;

namespace Mvc.Client {
    public class Startup {
        public void ConfigureServices(IServiceCollection services) {
            services.AddAuthentication(options => {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });
           
            services.AddAuthorization();
            services.AddMvc( config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                    .RequireAuthenticatedUser()
                                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }        
            );
        }

        //public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        //{
        //    app.Map("/SpotifyChangeControl", map => Configure1(map, env, loggerFactory));
        //}

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {

            //Setup Routes

            //Setup Auth
            //BEGIN RJB 9/10/2016
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); //ADDED THIS TO HAVE EXCEPTION PAGES INT MY APP WHEN IT IS IN DEBUG MODE ONLY
            }
            else //WHEN NOT IN DEBUG THEN HAVE SOMETHING COOLER
            {
                app.UseExceptionHandler("/Errors");
               app.UseStatusCodePagesWithRedirects("~/Errors/GetCustomErrorPageToMakeTheStupidHumanFeelGoodAboutThereTecnicalAbilitiesAndReassureThemItsNotTheirFault?sHttpStatus={0}");
               //app.UseStatusCodePagesWithReExecute("/Errors/GetCustomErrorPageToMakeTheStupidHumanFeelGoodAboutThereTecnicalAbilitiesAndReassureThemItsNotTheirFault?sHttpStatus={0}");
            }
            //END RJB 9/10/2016
           
            app.UseStaticFiles();
            
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                AccessDeniedPath = new PathString("/Account/Forbidden/"),
                LoginPath = new PathString("/Account/Login/"),
                LogoutPath = new PathString("/Account/Logout/")
            });

            SpotifyChangeControlLib.SCCManager oSCCManager = new SpotifyChangeControlLib.SCCManager();
            var Options = new AspNet.Security.OAuth.Spotify.SpotifyAuthenticationOptions()
            {
                ClientId = oSCCManager.SCC_PUBLIC_ID,
                ClientSecret = oSCCManager.SCC_PRIVATE_ID,
                
            };
            
            Options.Scope.Add("playlist-read-private");
            Options.Scope.Add("user-read-private");
            Options.Scope.Add("user-read-email");
            Options.Scope.Add("user-library-read");
            Options.Scope.Add("user-follow-read");
            app.UseSpotifyAuthentication(Options);

        
            app.UseMvc();
        }
    }
}