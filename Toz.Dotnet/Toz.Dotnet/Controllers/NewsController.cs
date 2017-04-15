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
        private INewsManagementService _newsManagementService;
	    private readonly IStringLocalizer<NewsController> _localizer;
        private readonly AppSettings _appSettings;
        private static byte[] _lastAcceptPhoto;
        private string _validationPhotoAlert;

        public NewsController(INewsManagementService newsManagementService, IStringLocalizer<NewsController> localizer, IOptions<AppSettings> appSettings)
        {
            _newsManagementService = newsManagementService;
			_localizer = localizer;
            _appSettings = appSettings.Value;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            List<News> news = await _newsManagementService.GetAllNews();
            //todo add photo if will be avaialbe on backends
            news.ForEach(n=> n.Photo = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 }); // temporary
            return View(news.OrderByDescending(x => x.PublishingTime ?? DateTime.MaxValue).ThenByDescending(x => x.Title).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(
            [Bind("Title, Body")] 
            News news, [Bind("Photo")] IFormFile photo, string status, CancellationToken cancellationToken)
        {
            bool result = ValidatePhoto(news, photo);
            news.PhotoUrl = "storage/a5/0d/4d/a50d4d4c-ccd2-4747-8dec-d6d7f521336e.jpg";

            Enum.TryParse(status, out NewsStatus newsStatus);
            news.Status = newsStatus;
            
            if (news != null && result && ModelState.IsValid)
            {
                    if (await _newsManagementService.CreateNews(news))
                    {
                        _lastAcceptPhoto = null;
                        _validationPhotoAlert = null;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return BadRequest();
                    }
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
                return View(news);
            }
        } 

        public IActionResult Add()
        {
            return View(new News());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            [Bind("Id, Title, Body, Status, PublishingTime, AddingTime, LastEditTime")] 
            News news, [Bind("Photo")] IFormFile photo, string status, CancellationToken cancellationToken)
        {
            //todo add photo if will be available on backends
            _lastAcceptPhoto = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 }; //get photo from backend, if available

            bool result = ValidatePhoto(news, photo);
            news.PhotoUrl = "storage/a5/0d/4d/a50d4d4c-ccd2-4747-8dec-d6d7f521336e.jpg";

            Enum.TryParse(status, out NewsStatus newsStatus);
            news.Status = newsStatus;

            if (news != null && result && ModelState.IsValid)
            {
                    if (await _newsManagementService.UpdateNews(news))
                    {
                        _lastAcceptPhoto = null;
                        _validationPhotoAlert = null;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return BadRequest();
                    }
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
                return View(news);
            }
            
        } 

        public async Task<ActionResult> Edit(string id, CancellationToken cancellationToken) 
        {
            return View(await _newsManagementService.GetNews(id));
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

        public async Task<ActionResult> Details(string id, CancellationToken cancellationToken) {
            return View(await _newsManagementService.GetNews(id));
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