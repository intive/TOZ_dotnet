using System.Threading.Tasks;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IRestService
    {
        Task<bool> ExecuteDeleteAction<T>(string address, T obj);
        Task<T> ExecuteGetAction<T>(string address);
        Task<bool> ExecutePostAction<T>(string address, T obj);
        Task<bool> ExecutePutAction<T>(string address, T obj);
    }
}