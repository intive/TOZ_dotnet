using Toz.Dotnet.Models.Errors;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IBackendErrorsService
    {
        void AddErrors(ErrorsList list);
        string UpdateModelState(ModelStateDictionary modelState);
    }
}
