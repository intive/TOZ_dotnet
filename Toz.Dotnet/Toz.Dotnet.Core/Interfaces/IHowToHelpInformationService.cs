using System.Threading;
using System.Threading.Tasks;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IHowToHelpInformationService
    {
        Task<bool> UpdateOrCreateHelpInfo(HowToHelpInfo helpInfo, HowToHelpInfoType type, CancellationToken cancelationToken = default(CancellationToken));
        Task<HowToHelpInfo> GetHelpInfo(HowToHelpInfoType type, CancellationToken cancellationToken = default(CancellationToken));
        string BecomeVolunteerUrl { get; set; }
        string DonateInfoUrl { get; set; }
    }
}