using Marvin.IDP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Marvin.IDP.Pages.User.Activation
{
    [SecurityHeaders]
    [AllowAnonymous]

    public class IndexModel(ILocalUserService localUserService) : PageModel
    {
        private readonly ILocalUserService _localUserService = localUserService ??
                throw new ArgumentNullException(nameof(localUserService));

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel() { Message = string.Empty };

        public async Task<IActionResult> OnGet(string securityCode)
        {
            Input = new InputModel() { Message = string.Empty };

            if (await _localUserService.ActivateUserAsync(securityCode))
            {
                Input.Message = "Your account was successfully activated.  " +
                    "Navigate to your client application to log in.";
            }
            else
            {
                Input.Message = "Your account couldn't be activated, " +
                    "please contact your administrator.";
            }

            await _localUserService.SaveChangesAsync();

            return Page();


        }
    }
}
