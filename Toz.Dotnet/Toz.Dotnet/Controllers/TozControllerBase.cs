using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Authorization;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Resources.Configuration;

namespace Toz.Dotnet.Controllers
{
    public abstract class TozControllerBase<T> : Controller where T : Controller
    {
        protected IBackendErrorsService BackendErrorsService { get; }
        protected IStringLocalizer<T> StringLocalizer { get;} 
        protected AppSettings AppSettings { get; }
        protected IAuthService AuthService { get; }
        protected IMemoryCache MemoryCache { get; }

        protected TozControllerBase(IBackendErrorsService backendErrorsService, IStringLocalizer<T> localizer, 
            IOptions<AppSettings> settings, IAuthService authService,
            IMemoryCache memoryCache = null)
        {
            BackendErrorsService = backendErrorsService;
            StringLocalizer = localizer;
            AppSettings = settings.Value;
            AuthService = authService;
            MemoryCache = memoryCache;
        }

        protected void CheckUnexpectedErrors()
        {
            var overallError = BackendErrorsService.UpdateModelState(ModelState);
            if (!string.IsNullOrEmpty(overallError))
            {
                ViewData["UnhandledError"] = overallError;
            }
        }

        protected string CurrentCookiesToken => AuthService.ReadCookie(HttpContext, AppSettings.CookieTokenName, true);

        protected void SetCache<T1>(string cacheKey, T1 value, CacheItemPriority priority = CacheItemPriority.Normal, TimeSpan? expiration = null)
        {
            if (MemoryCache == null)
            {
                return;
            }
            MemoryCache.CreateEntry(cacheKey);
            MemoryCache.Set(cacheKey, value, new MemoryCacheEntryOptions()
            {
                Priority = priority,
                SlidingExpiration = expiration
            });

        }

        protected T1 GetFromCache<T1>(string cacheKey)
        {
            if (MemoryCache == null)
            {
                return default(T1);
            }

            T1 obj;
            MemoryCache.TryGetValue(cacheKey, out obj);
            return obj;
        }
    }
}