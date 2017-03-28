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
        private List<News> _news = new List<News>(){
                new News("id_1","ABC",DateTime.MinValue,DateTime.Now, DateTime.Now, "body", new byte[10], NewsStatus.Draft),
                new News("id_2","ABCD",DateTime.Now,DateTime.Now, DateTime.Now, "bodyy", new byte[10], NewsStatus.Published)
            };

        public NewsController(INewsManagementService newsManagementService, IStringLocalizer<NewsController> localizer, IOptions<AppSettings> appSettings)
        {
            _newsManagementService = newsManagementService;
			_localizer = localizer;
            _appSettings = appSettings.Value;
        }

        public ActionResult Index() {
            return View(_news);
        }

        public ActionResult Add() {
            return View(new News());
        }

        public ActionResult Edit(string id) {
            return View(_news.Find(x => x.Id == id));
           
        }

        public ActionResult Delete() {
            return View();
        }

        public ActionResult Details(string id) {
            return View(_news.Find(x => x.Id == id));
        }
    }
}