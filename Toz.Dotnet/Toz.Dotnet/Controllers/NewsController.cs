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

namespace Toz.Dotnet.Controllers
{
    public class NewsController : Controller
    {
        private INewsManagementService _newsManagementService;
	    private readonly IStringLocalizer<NewsController> _localizer;
        private readonly AppSettings _appSettings;

        public NewsController(INewsManagementService newsManagementService, IStringLocalizer<NewsController> localizer, IOptions<AppSettings> appSettings)
        {
            _newsManagementService = newsManagementService;
			_localizer = localizer;
            _appSettings = appSettings.Value;
        }

        public ActionResult Index() {
            List<News> news = new List<News>(){
                new News("","ABC",DateTime.MinValue,DateTime.Now, DateTime.Now, "body", new byte[10], NewsStatus.Draft),
                new News("","ABCD",DateTime.Now,DateTime.Now, DateTime.Now, "bodyy", new byte[10], NewsStatus.Published)
            };

            return View(news);
        }

        public ActionResult Add() {
            return View();
        }

        public ActionResult Edit() {
            return View();
        }

        public ActionResult Delete() {
            return View();
        }
    }
}