using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Marvin.IDP.Data;
using Marvin.IDP.Areas.Identity.Data;

namespace Marvin.IDP;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetService<MarvinIDPContext>();
            context.Database.Migrate();

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var emma = userMgr.FindByNameAsync("Emma").Result;
            if (emma == null)
            {
                emma = new ApplicationUser
                {
                    UserName = "Emma",
                    Email = "emma@someprovider.com",
                    EmailConfirmed = true,
                };
                var result = userMgr.CreateAsync(emma, "P@ssword1").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(emma, new Claim[]{
                        new Claim("role", "PayingUser"),
                        new Claim(JwtClaimTypes.GivenName, "Emma"),
                        new Claim(JwtClaimTypes.FamilyName, "Flagg"),
                        new Claim(JwtClaimTypes.Email, "emma@someprovider.com"),
                        new Claim("country", "be"),
                        }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("Emma created");
            }
            else
            {
                Log.Debug("Emma already exists");
            }

            var david = userMgr.FindByNameAsync("David").Result;
            if (david == null)
            {
                david = new ApplicationUser
                {
                    UserName = "David",
                    Email = "david@someprovider.com",
                    EmailConfirmed = true
                };
                var result = userMgr.CreateAsync(david, "P@ssword1").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(david, new Claim[]{
                            new Claim("role", "FreeUser"),
                            new Claim(JwtClaimTypes.GivenName, "David"),
                            new Claim(JwtClaimTypes.FamilyName, "Flagg"),
                            new Claim(JwtClaimTypes.Email, "david@someprovider.com"),
                            new Claim("country", "nl")
                        }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
                Log.Debug("David created");
            }
            else
            {
                Log.Debug("David already exists");
            }
        }
    }
}
