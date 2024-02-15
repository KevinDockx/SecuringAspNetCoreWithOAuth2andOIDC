using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdentityModel.Client;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace ImageGallery.BFF.Controllers
{
    [Authorize]
    public class UserSessionController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserSessionController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ??
                throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public IActionResult GivenName()
        {
            var objectToReturn = new 
            {
                given_name = User.Claims
                    .FirstOrDefault(c => c.Type == "given_name")?.Value
            };

            return Json(objectToReturn);             
        }

        public async Task Logout()
        {
            var client = _httpClientFactory.CreateClient("IDPClient");

            var discoveryDocumentResponse = await client
                .GetDiscoveryDocumentAsync();
            if (discoveryDocumentResponse.IsError)
            {
                throw new Exception(discoveryDocumentResponse.Error);
            }

            var accessTokenRevocationResponse = await client
                .RevokeTokenAsync(new()
                {
                    Address = discoveryDocumentResponse.RevocationEndpoint,
                    ClientId = "imagegallerybff",
                    ClientSecret = "anothersecret",
                    Token = await HttpContext.GetTokenAsync(
                        OpenIdConnectParameterNames.AccessToken)
                });

            if (accessTokenRevocationResponse.IsError)
            {
                throw new Exception(accessTokenRevocationResponse.Error);
            }

            var refreshTokenRevocationResponse = await client
                .RevokeTokenAsync(new()
                {
                    Address = discoveryDocumentResponse.RevocationEndpoint,
                    ClientId = "imagegallerybff",
                    ClientSecret = "anothersecret",
                    Token = await HttpContext.GetTokenAsync(
                        OpenIdConnectParameterNames.RefreshToken)
                });

            if (refreshTokenRevocationResponse.IsError)
            {
                throw new Exception(accessTokenRevocationResponse.Error);
            }

            // Clears the  local cookie
            await HttpContext.SignOutAsync("BFFCookieScheme");

            // Redirects to the IDP so it can clear its own session/cookie
            await HttpContext.SignOutAsync("BFFChallengeScheme");
        }
    }
}
