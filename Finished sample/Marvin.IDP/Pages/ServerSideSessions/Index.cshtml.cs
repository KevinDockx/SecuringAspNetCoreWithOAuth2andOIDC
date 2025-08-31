// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Marvin.IDP.Pages.ServerSideSessions
{
    public class IndexModel(ISessionManagementService? sessionManagementService = null) : PageModel
    {
        public QueryResult<UserSession>? UserSessions { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? DisplayNameFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SessionIdFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SubjectIdFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Token { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Prev { get; set; }

        public async Task OnGet()
        {
            if (sessionManagementService != null)
            {
                UserSessions = await sessionManagementService.QuerySessionsAsync(new SessionQuery
                {
                    ResultsToken = Token,
                    RequestPriorResults = Prev == "true",
                    DisplayName = DisplayNameFilter,
                    SessionId = SessionIdFilter,
                    SubjectId = SubjectIdFilter
                });
            }
        }

        [BindProperty]
        public string? SessionId { get; set; }

        public async Task<IActionResult> OnPost()
        {
            ArgumentNullException.ThrowIfNull(sessionManagementService);

            await sessionManagementService.RemoveSessionsAsync(new RemoveSessionsContext
            {
                SessionId = SessionId,
            });
            return RedirectToPage("/ServerSideSessions/Index", new { Token, DisplayNameFilter, SessionIdFilter, SubjectIdFilter, Prev });
        }
    }
}

