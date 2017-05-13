using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Models.Errors;

namespace Toz.Dotnet.Core.Services
{
    public class BackendErrorsService : IBackendErrorsService
    {
        private readonly List<Error> _errorsList = new List<Error>();

        public void UpdateModelState(ModelStateDictionary modelState)
        {
            foreach(Error error in _errorsList)
            {
                //modelState.AddModelError(error.Field, error.Message);
                modelState.AddModelError(error.Field, Resources.ModelsDataValidation.InvalidValue);
            }

            _errorsList.Clear();
        }

        public void AddErrors(ErrorsList list)
        {
            _errorsList.AddRange(list.Errors);
        }
    }
}
