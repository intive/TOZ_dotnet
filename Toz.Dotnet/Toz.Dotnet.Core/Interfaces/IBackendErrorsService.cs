using Toz.Dotnet.Models.Errors;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IBackendErrorsService
    {
        void AddErrors(ErrorsList list);
        void UpdateModelState(ModelStateDictionary modelState);
    }
}
