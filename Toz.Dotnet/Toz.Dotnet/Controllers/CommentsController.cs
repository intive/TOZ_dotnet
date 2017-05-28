using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Authorization;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Resources.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Toz.Dotnet.Models;

namespace Toz.Dotnet.Controllers
{
    public class CommentsController : TozControllerBase<CommentsController>
    {
        private readonly ICommentsManagementService _commentsManagementService;

        public CommentsController(ICommentsManagementService commentsManagementService,
            IBackendErrorsService backendErrorsService, IStringLocalizer<CommentsController> localizer, IOptions<AppSettings> settings, IAuthService authService) : base(backendErrorsService, localizer, settings, authService)
        {
            _commentsManagementService = commentsManagementService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            List<Comment> comments = await _commentsManagementService.GetAllComments(AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true), cancellationToken);
            return View(comments.OrderByDescending(x => x.Created).ThenByDescending(x => x.LastModified).ToList());
        }
    }
}
