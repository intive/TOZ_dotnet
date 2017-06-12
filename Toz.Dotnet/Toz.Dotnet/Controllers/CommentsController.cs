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
using Microsoft.Extensions.Caching.Memory;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.ViewModels;
using Toz.Dotnet.Models.EnumTypes;

namespace Toz.Dotnet.Controllers
{
    public class CommentsController : TozControllerBase<CommentsController>
    {
        private readonly ICommentsManagementService _commentsManagementService;
        private readonly IPetsManagementService _petsManagementService;

        public CommentsController(ICommentsManagementService commentsManagementService,
            IPetsManagementService petsManagementService, 
            IBackendErrorsService backendErrorsService, 
            IStringLocalizer<CommentsController> localizer, 
            IOptions<AppSettings> settings, IAuthService authService,
            IMemoryCache memoryCache) : base(backendErrorsService, localizer, settings, authService, memoryCache)
        {
            _commentsManagementService = commentsManagementService;
            _petsManagementService = petsManagementService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken, CommentState commentState = CommentState.Active)
        {
            var comments = await _commentsManagementService.GetAllComments(CurrentCookiesToken, cancellationToken);
            var viewModels = new List<CommentViewModel>();
            
            foreach(var comment in comments.Where(c=> c.State == commentState))
            {
                var petName = GetFromCache<string>(comment.PetUuid);
                if (petName == null)
                {
                    petName = (await _petsManagementService.GetPet(comment.PetUuid, CurrentCookiesToken, cancellationToken))?.Name;
                    TimeSpan? timeSpan = TimeSpan.FromMinutes(AppSettings.CacheExpirationTimeInMinutes);
                    SetCache(comment.PetUuid, petName, CacheItemPriority.High, timeSpan);
                }
                
                viewModels.Add(new CommentViewModel() { Comment = comment, PetName = petName });
            }

            return View(viewModels.OrderByDescending(x => x.Comment.Created).ToList());
        }

        public IActionResult DeleteModal(CancellationToken cancellationToken)
        {
            return PartialView("DeleteModal");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteModal(string id, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(id) && await _commentsManagementService.DeleteComment(id, CurrentCookiesToken, cancellationToken))
            {
                return Json(new { success = true });
            }

            CheckUnexpectedErrors();
            return PartialView("DeleteModal");
        }
    }
}
