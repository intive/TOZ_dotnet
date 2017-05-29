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
using Toz.Dotnet.Models.ViewModels;
using Toz.Dotnet.Models.EnumTypes;

namespace Toz.Dotnet.Controllers
{
    public class CommentsController : TozControllerBase<CommentsController>
    {
        private readonly ICommentsManagementService _commentsManagementService;
        private readonly IUsersManagementService _usersManagementService;

        public CommentsController(ICommentsManagementService commentsManagementService, IUsersManagementService usersManagementService,
            IBackendErrorsService backendErrorsService, IStringLocalizer<CommentsController> localizer, IOptions<AppSettings> settings, IAuthService authService) : base(backendErrorsService, localizer, settings, authService)
        {
            _commentsManagementService = commentsManagementService;
            _usersManagementService = usersManagementService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            List<Comment> comments = await _commentsManagementService.GetAllComments(AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true), cancellationToken);
            List<CommentViewModel> viewModels = new List<CommentViewModel>();

            foreach(Comment comment in comments)
            {
                if (comment.State == CommentState.Deleted)
                {
                    continue;
                }

                User user = await _usersManagementService.GetUser(comment.UserUuid, AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true));
                viewModels.Add(new CommentViewModel() { TheUser = user, TheComment = comment });
            }

            return View(viewModels.OrderByDescending(x => x.Created).ToList());
        }

        public async Task<IActionResult> DeletedList(CancellationToken cancellationToken)
        {
            List<Comment> comments = await _commentsManagementService.GetAllComments(AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true), cancellationToken);
            List<CommentViewModel> viewModels = new List<CommentViewModel>();

            foreach (Comment comment in comments)
            {
                if (comment.State == CommentState.Active)
                {
                    continue;
                }

                User user = await _usersManagementService.GetUser(comment.UserUuid, AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true));
                viewModels.Add(new CommentViewModel() { TheUser = user, TheComment = comment });
            }

            return View(viewModels.OrderByDescending(x => x.Created).ToList());
        }

        public IActionResult DeleteModal(CancellationToken cancellationToken)
        {
            return PartialView("DeleteModal");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteModal(string id, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(id) && await _commentsManagementService.DeleteComment(id, AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true), cancellationToken))
            {
                return Json(new { success = true });
            }

            CheckUnexpectedErrors();
            return PartialView("DeleteModal");
        }
    }
}
