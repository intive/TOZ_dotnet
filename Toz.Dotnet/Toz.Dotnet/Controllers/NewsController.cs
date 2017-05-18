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
    public class NewsController : Controller
    {
        private IFilesManagementService _filesManagementService;
        private INewsManagementService _newsManagementService;
        private IBackendErrorsService _backendErrorsService;
        private readonly IStringLocalizer<NewsController> _localizer;
        private readonly AppSettings _appSettings;
        private static byte[] _lastAcceptPhoto;
        private string _validationPhotoAlert;

        public NewsController(IFilesManagementService filesManagementService, INewsManagementService newsManagementService,
            IStringLocalizer<NewsController> localizer, IOptions<AppSettings> appSettings, IBackendErrorsService backendErrorsService)
        {
            _filesManagementService = filesManagementService;
            _newsManagementService = newsManagementService;
			_localizer = localizer;
            _appSettings = appSettings.Value;
            _backendErrorsService = backendErrorsService;
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
            
            if (news != null && result && ModelState.IsValid)
            {
                if (await _newsManagementService.CreateNews(news))
                {
                    _lastAcceptPhoto = null;
                    _validationPhotoAlert = null;
                    return Json(new { success = true });
                }

                var overallError = _backendErrorsService.UpdateModelState(ModelState);
                if (!string.IsNullOrEmpty(overallError))
                {
                    this.ViewData["UnhandledError"] = overallError;
                }
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

            if (news != null && result && ModelState.IsValid)
            {
                if (await _newsManagementService.UpdateNews(news))
                {
                    _lastAcceptPhoto = null;
                    _validationPhotoAlert = null;
                    return Json(new { success = true });
                }

                var overallError = _backendErrorsService.UpdateModelState(ModelState);
                if (!string.IsNullOrEmpty(overallError))
                {
                    this.ViewData["UnhandledError"] = overallError;
                }

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
            return PartialView("Edit", await _newsManagementService.GetNews(id));
        }

        public async Task<ActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var pet = await _newsManagementService.GetNews(id);
            if(pet != null)
            {
                await _newsManagementService.DeleteNews(pet);
            }

            return RedirectToAction("Index");
        }

        private bool IsAcceptedPhotoType(string photoType, string[] acceptTypes)
        {
            foreach(var type in acceptTypes)
            {
                if(type == photoType)
                    return true;
            }
            return false;
        }

        private bool ValidatePhoto(News news, IFormFile photo)
        {
            if(photo != null)
            {
                if(IsAcceptedPhotoType(photo.ContentType, _appSettings.AcceptPhotoTypes))
                {
                    if(photo.Length > 0)
                    {
                        news.Photo = _newsManagementService.ConvertPhotoToByteArray(photo.OpenReadStream());
                        _lastAcceptPhoto = news.Photo;
                        return true;
                    }
                    else
                    {
                        _validationPhotoAlert = "EmptyFile";
                        return false;
                    }
                }
                else
                {
                    _validationPhotoAlert = "WrongFileType";
                    return false; 
                }
            }
            else
            {
                if(_lastAcceptPhoto != null)
                {
                    news.Photo = _lastAcceptPhoto;
                }
                return true;
            }
        }
    }
}