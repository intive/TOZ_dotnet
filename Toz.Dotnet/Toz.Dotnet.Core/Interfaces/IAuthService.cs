using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Toz.Dotnet.Models;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IAuthService
    {
        bool IsAuth { get; }
        Task SignIn(Login login);
        void SighOut();
        void AddTokenToHttpClient(HttpClient httpClient);
        string ActiveUser { get; }
        string RequestUri { get; set; }
    }
}