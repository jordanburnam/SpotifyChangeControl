/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using AspNet.Security.OAuth.GitHub;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;


namespace Mvc.Client {
    public class Startup {
        public void ConfigureServices(IServiceCollection services) {
            services.AddAuthentication(options => {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app) {
            app.UseStaticFiles();

            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                LoginPath = new PathString("/signin"),
                LogoutPath = new PathString("/signout")
            });

            
            var Options = new AspNet.Security.OAuth.Spotify.SpotifyAuthenticationOptions()
            {
                ClientId = "d2036a1624b343e5a0dbe93824758bc0",
                ClientSecret = "14ff6ba40acb4f2e849f2adb9a8b42a0"

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