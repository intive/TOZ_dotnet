using System;
using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Toz.Dotnet.Resources.Configuration;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Net.Http;
using Toz.Dotnet.Authorization;

namespace Toz.Dotnet.Controllers
{
    public class NewsController : TozControllerBase<NewsController>
    {
        private readonly IFilesManagementService _filesManagementService;
        private readonly INewsManagementService _newsManagementService;
        private readonly IOptions<AppSettings> _appSettings;

        public NewsController(IFilesManagementService filesManagementService, INewsManagementService newsManagementService,
            IStringLocalizer<NewsController> localizer, 
            IOptions<AppSettings> appSettings, IBackendErrorsService backendErrorsService, IAuthService authService) 
            : base(backendErrorsService, localizer ,appSettings, authService)
        {
            _filesManagementService = filesManagementService;
            _newsManagementService = newsManagementService;
            _appSettings = appSettings;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            List<News> news = await _newsManagementService.GetAllNews(CurrentCookiesToken, cancellationToken);
            foreach (var n in news)
            {
                if (!string.IsNullOrEmpty(n.ImageUrl))
                {
                    try
                    {
                        var downloadedImg = _filesManagementService.DownloadImage(_appSettings.Value.BaseUrl + n.ImageUrl);

                        if (downloadedImg != null)
                        {
                            var thumbnail = _filesManagementService.GetThumbnail(downloadedImg);
                            n.Photo = _filesManagementService.ImageToByteArray(thumbnail);
                        }
                    }
                    catch (HttpRequestException)
                    {
                        n.Photo = null;
                    }
                    catch (AggregateException)
                    {
                        n.Photo = null;
                    }
                }
            }
            return View(news.OrderByDescending(x => x.Published ?? DateTime.MaxValue).ThenByDescending(x => x.Title).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            [Bind("Title, Contents")] 
            News news, string status, CancellationToken cancellationToken)
        {
            Enum.TryParse(status, out NewsStatus newsStatus);
            news.Type = newsStatus;
            
            if (ModelState.IsValid)
            {
                if (await _newsManagementService.CreateNews(news, CurrentCookiesToken, cancellationToken))
                {
                    return Json(new { success = true });
                }

               CheckUnexpectedErrors(); 
            }

            return PartialView(news);
        } 

        public IActionResult Add()
        {
            return PartialView(new News());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [Bind("Id, Title, Contents, Status, PublishingTime, AddingTime, LastEditTime")] 
            News news, string status, CancellationToken cancellationToken)
        {
            Enum.TryParse(status, out NewsStatus newsStatus);
            news.Type = newsStatus;

            if (ModelState.IsValid)
            {
                if (await _newsManagementService.UpdateNews(news, CurrentCookiesToken, cancellationToken))
                {
                    return Json(new { success = true });
                }

                CheckUnexpectedErrors();
            }

            return PartialView(news); 
        } 

        public async Task<ActionResult> Edit(string id, CancellationToken cancellationToken) 
        {
            return PartialView("Edit", await _newsManagementService.GetNews(id, CurrentCookiesToken, cancellationToken));
        }

        public async Task<ActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var pet = await _newsManagementService.GetNews(id, CurrentCookiesToken, cancellationToken);
            if(pet != null)
            {
                await _newsManagementService.DeleteNews(pet, CurrentCookiesToken, cancellationToken);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Avatar(string id, CancellationToken cancellationToken)
        {
            var files = Request.Form.Files;
            if (await _filesManagementService.UploadNewsAvatar(id, CurrentCookiesToken, files, cancellationToken))
            {
                return Json(new { success = true });
            }

            CheckUnexpectedErrors();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Images(string id, CancellationToken cancellationToken)
        {
            return PartialView("Images", await _newsManagementService.GetNews(id, CurrentCookiesToken, cancellationToken));
        }
    }
}