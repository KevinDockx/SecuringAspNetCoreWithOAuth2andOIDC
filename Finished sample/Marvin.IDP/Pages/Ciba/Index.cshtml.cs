// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Marvin.IDP.Pages.Ciba;

[AllowAnonymous]
[SecurityHeaders]
public class IndexModel(IBackchannelAuthenticationInteractionService backchannelAuthenticationInteractionService, ILogger<IndexModel> logger) : PageModel
{
    public BackchannelUserLoginRequest LoginRequest { get; set; } = default!;

    public async Task<IActionResult> OnGet(string id)
    {
        var result = await backchannelAuthenticationInteractionService.GetLoginRequestByInternalIdAsync(id);
        if (result == null)
        {
            logger.InvalidBackchannelLoginId(id);
            return RedirectToPage("/Home/Error/Index");
        }
        else
        {
            LoginRequest = result;
        }

        return Page();
    }
}