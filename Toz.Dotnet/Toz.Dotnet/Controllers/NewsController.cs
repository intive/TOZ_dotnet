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

namespace Toz.Dotnet.Controllers
{
    public class NewsController : TozControllerBase<NewsController>
    {
        private readonly IFilesManagementService _filesManagementService;
        private readonly INewsManagementService _newsManagementService;

        public NewsController(IFilesManagementService filesManagementService, INewsManagementService newsManagementService,
            IStringLocalizer<NewsController> localizer, IOptions<AppSettings> appSettings, IBackendErrorsService backendErrorsService) : base(backendErrorsService, localizer ,appSettings)
        {
            _filesManagementService = filesManagementService;
            _newsManagementService = newsManagementService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            List<News> news = await _newsManagementService.GetAllNews();
            //todo add photo if will be avaialbe on backends
            var img = _filesManagementService.DownloadImage(@"http://img.cda.pl/obr/thumbs/6adb80c33f5b55df46a481b57a61c64c.png_oooooooooo_273x.png");
            var thumbnail = _filesManagementService.GetThumbnail(img);
            news.ForEach(n => n.Photo = _filesManagementService.ImageToByteArray(thumbnail)); // temporary
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
                if (await _newsManagementService.CreateNews(news))
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
                if (await _newsManagementService.UpdateNews(news))
                {
                    return Json(new { success = true });
                }

                CheckUnexpectedErrors();
            }

            return PartialView(news); 
        } 

        public async Task<ActionResult> Edit(string id, CancellationToken cancellationToken) 
        {
            return PartialView("Edit", await _newsManagementService.GetNews(id, cancellationToken));
        }

        public async Task<ActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var pet = await _newsManagementService.GetNews(id, cancellationToken);
            if(pet != null)
            {
                await _newsManagementService.DeleteNews(pet, cancellationToken);
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Images(string id, CancellationToken cancellationToken)
        {
            return PartialView("Images", await _newsManagementService.GetNews(id));
        }
    }
}