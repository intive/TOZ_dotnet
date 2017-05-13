using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models.Errors;

namespace Toz.Dotnet.Core.Services
{
    public class BackendErrorsService : IBackendErrorsService
    {
        List<Error> _errorsList = new List<Error>();

        public string UpdateModelState(ModelStateDictionary modelState)
        {
            foreach(Error error in _errorsList)
            {
                if (!string.IsNullOrEmpty(error.Field))
                {
                    modelState.AddModelError(error.Field, Resources.ModelsDataValidation.InvalidField);
                    continue;
                }
                return error.Message ?? Resources.ModelsDataValidation.UnknownError;
            }
            _errorsList.Clear();
            return string.Empty;
        }

        public void AddErrors(ErrorsList list)
        {
            _errorsList.AddRange(list.Errors);
        }

        public void AddError(Error error)
        {
            _errorsList.Add(error);
        }
    }
}
