using Xunit;
using Toz.Dotnet.Controllers;
using Toz.Dotnet.Core.Services;
using Toz.Dotnet.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Toz.Dotnet.Core.Interfaces;
using Toz.Dotnet.Tests.Helpers;

namespace Toz.Dotnet.Tests.Tests {
    public class PetsControllerTest
    {
        private IPetsManagementService _petsManagementService;
        public PetsControllerTest()
        {
            _petsManagementService = ServiceProvider.Instance.Resolve<IPetsManagementService>();
        }

        [Fact]
        public void IndexReturnsAViewResult()
        {
            var controller = new PetsController(_petsManagementService);

            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void IndexReturnsAListWithPets()
        {
            var controller = new PetsController(_petsManagementService);

            var result = controller.Index();

            var viewResult = (ViewResult)result;
            var model = Assert.IsAssignableFrom<List<Pet>>(viewResult.ViewData.Model);
        }

        [Fact]
        public void IndexReturnsAListWithExpectedAmountOfModels()
        {
            var controller = new PetsController(_petsManagementService);

            var result = controller.Index();

            var models = (List<Pet>)((ViewResult)result).ViewData.Model;
            Assert.Equal(3, models.Count);
        }
    }
}