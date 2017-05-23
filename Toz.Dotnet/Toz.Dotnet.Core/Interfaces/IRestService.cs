using System.Threading;
using System.Threading.Tasks;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IRestService
    {
        Task<bool> ExecuteDeleteAction(string address, CancellationToken cancelationToken = default(CancellationToken));
        Task<T> ExecuteGetAction<T>(string address, CancellationToken cancelationToken = default(CancellationToken)) where T : class;
        Task<bool> ExecutePostAction<T>(string address, T obj, CancellationToken cancelationToken = default(CancellationToken)) where T : class;
        Task<bool> ExecutePutAction<T>(string address, T obj, CancellationToken cancelationToken = default(CancellationToken)) where T : class;
    }
}