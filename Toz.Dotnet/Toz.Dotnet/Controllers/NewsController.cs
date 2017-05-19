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

        private static byte[] _lastAcceptPhoto;
        private string _validationPhotoAlert;

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
            News news, [Bind("Photo")] IFormFile photo, string status, CancellationToken cancellationToken)
        {
            bool result = ValidatePhoto(news, photo);
            news.PhotoUrl = "storage/a5/0d/4d/a50d4d4c-ccd2-4747-8dec-d6d7f521336e.jpg";

            Enum.TryParse(status, out NewsStatus newsStatus);
            news.Type = newsStatus;
            
            if (result && ModelState.IsValid)
            {
                if (await _newsManagementService.CreateNews(news))
                {
                    _lastAcceptPhoto = null;
                    _validationPhotoAlert = null;
                    return Json(new { success = true });
                }

               CheckUnexpectedErrors();
               return PartialView(news);
            }
            else
            {
                if(!result)
                {
                    ViewData["ValidationPhotoAlert"] = _validationPhotoAlert;
                    if(_lastAcceptPhoto != null)
                    {
                        news.Photo = _lastAcceptPhoto;
                        ViewData["SelectedPhoto"] = "PhotoAlertWithLastPhoto";
                    }
                    else
                    {
                        ViewData["SelectedPhoto"] = "PhotoAlertWithoutPhoto";
                    }
                }
                return PartialView(news);
            }
        } 

        public IActionResult Add()
        {
            return PartialView(new News());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [Bind("Id, Title, Contents, Status, PublishingTime, AddingTime, LastEditTime")] 
            News news, [Bind("Photo")] IFormFile photo, string status, CancellationToken cancellationToken)
        {
            //todo add photo if will be available on backends
            _lastAcceptPhoto = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 }; //get photo from backend, if available

            bool result = ValidatePhoto(news, photo);
            news.PhotoUrl = "storage/a5/0d/4d/a50d4d4c-ccd2-4747-8dec-d6d7f521336e.jpg";

            Enum.TryParse(status, out NewsStatus newsStatus);
            news.Type = newsStatus;

            if (result && ModelState.IsValid)
            {
                if (await _newsManagementService.UpdateNews(news))
                {
                    _lastAcceptPhoto = null;
                    _validationPhotoAlert = null;
                    return Json(new { success = true });
                }

                CheckUnexpectedErrors();

                return PartialView(news);
            }

            if(!result)
            {
                ViewData["ValidationPhotoAlert"] = _validationPhotoAlert;
                if(_lastAcceptPhoto != null)
                {
                    news.Photo = _lastAcceptPhoto;
                    ViewData["SelectedPhoto"] = "PhotoAlertWithLastPhoto";
                }
                else
                {
                    ViewData["SelectedPhoto"] = "PhotoAlertWithoutPhoto";
                }
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

        private bool IsAcceptedPhotoType(string photoType, string[] acceptTypes)
        {
            return acceptTypes.Any(type => type == photoType);
        }

        private bool ValidatePhoto(News news, IFormFile photo)
        {
            if(photo != null)
            {
                if(IsAcceptedPhotoType(photo.ContentType, AppSettings.AcceptPhotoTypes))
                {
                    if(photo.Length > 0)
                    {
                        news.Photo = _newsManagementService.ConvertPhotoToByteArray(photo.OpenReadStream());
                        _lastAcceptPhoto = news.Photo;
                        return true;
                    }
                    _validationPhotoAlert = "EmptyFile";
                    return false;
                }

                _validationPhotoAlert = "WrongFileType";
                return false;                
            }

            if(_lastAcceptPhoto != null)
            {
                news.Photo = _lastAcceptPhoto;
            }
            return true;
        }
    }
}