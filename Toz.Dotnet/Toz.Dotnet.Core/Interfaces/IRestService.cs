using System.Threading;
using System.Threading.Tasks;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IRestService
    {
        Task<bool> ExecuteDeleteAction<T>(string address, T obj, CancellationToken cancelationToken = default(CancellationToken)) where T : class;
        Task<T> ExecuteGetAction<T>(string address, CancellationToken cancelationToken = default(CancellationToken)) where T : class;
        Task<bool> ExecutePostAction<T>(string address, T obj, CancellationToken cancelationToken = default(CancellationToken)) where T : class;
        Task<string> ExecutePostActionAndReturnId<T>(string address, T obj, CancellationToken cancelationToken = default(CancellationToken)) where T : class;
        Task<bool> ExecutePutAction<T>(string address, T obj, CancellationToken cancelationToken = default(CancellationToken)) where T : class;
    }
}