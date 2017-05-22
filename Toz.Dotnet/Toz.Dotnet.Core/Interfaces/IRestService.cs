using System.Threading;
using System.Threading.Tasks;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IRestService
    {
        Task<bool> ExecuteDeleteAction(string address, string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<T> ExecuteGetAction<T>(string address, string token, CancellationToken cancelationToken = default(CancellationToken)) where T : class;
        Task<bool> ExecutePostAction<T>(string address, T obj, string token, CancellationToken cancelationToken = default(CancellationToken)) where T : class;
        Task<T1> ExecutePostAction<T1, T2>(string address, T2 obj, CancellationToken cancelationToken = default(CancellationToken)) where T1 : class where T2 : class;
        Task<bool> ExecutePutAction<T>(string address, T obj, string token, CancellationToken cancelationToken = default(CancellationToken)) where T : class;
    }
}