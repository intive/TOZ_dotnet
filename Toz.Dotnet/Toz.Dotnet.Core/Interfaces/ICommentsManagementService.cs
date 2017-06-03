using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Toz.Dotnet.Models;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface ICommentsManagementService
    {
        Task<List<Comment>> GetAllComments(string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<Comment> GetComment(string id, string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> DeleteComment(string id, string token, CancellationToken cancelationToken = default(CancellationToken));
        string RequestUri { get; set; }
    }
}
