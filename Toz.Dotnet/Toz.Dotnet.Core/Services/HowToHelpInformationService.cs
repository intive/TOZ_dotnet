using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using Toz.Dotnet.Resources.Configuration;

namespace Toz.Dotnet.Core.Services
{
    public class HowToHelpInformationService : IHowToHelpInformationService
    {
        private readonly IRestService _restService;

        public string BecomeVolunteerUrl { get; set; }
        public string DonateInfoUrl { get; set; }

        public HowToHelpInformationService(IRestService restService, IOptions<AppSettings> appSettings)
        {
            _restService = restService;
            BecomeVolunteerUrl = appSettings.Value.BackendBecomeVolunteerUrl;
            DonateInfoUrl = appSettings.Value.BackendDonateInfoUrl;
        }

        public async Task<bool> UpdateOrCreateHelpInfo(HowToHelpInfo helpInfo, HowToHelpInfoType type, CancellationToken cancelationToken = new CancellationToken())
        {
            string currentUrl;
            if (!GetCurrentUrl(type, out currentUrl))
            {
                return false;
            }

            Func<string, HowToHelpInfo, CancellationToken, Task<bool>> methodToExecute = _restService.ExecutePutAction;

            if (await GetHelpInfo(type, cancelationToken) == null)
            {
                methodToExecute = _restService.ExecutePostAction;
            }

            return await methodToExecute(currentUrl, helpInfo, cancelationToken);
        }

        public async Task<HowToHelpInfo> GetHelpInfo(HowToHelpInfoType type, CancellationToken cancellationToken = new CancellationToken())
        {
            string currentUrl;
            if (!GetCurrentUrl(type, out currentUrl))
            {
                return null;
            }

            return await _restService.ExecuteGetAction<HowToHelpInfo>(currentUrl, cancellationToken);
        }

        private bool GetCurrentUrl(HowToHelpInfoType type, out string resultUrl)
        {
            switch (type)
            {
                case HowToHelpInfoType.BecomeVolunteer:
                    resultUrl = BecomeVolunteerUrl;
                    break;
                case HowToHelpInfoType.Donate:
                    resultUrl = DonateInfoUrl;
                    break;
                default:
                    resultUrl = string.Empty;
                    return false;
            }
            return true;
        }
    }
}
