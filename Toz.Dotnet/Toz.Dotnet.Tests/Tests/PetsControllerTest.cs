using Xunit;
using Toz.Dotnet.Controllers;
using Toz.Dotnet.Core.Services;
using Toz.Dotnet.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Toz.Dotnet.Tests.Tests {
    public class PetsControllerTest
    {
        [Fact]
        public void Index_ReturnsAViewResult_WithAListOfPets()
        {
            var controller = new PetsController(new PetsManagementService(new FilesManagementService()));

            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
             var model = Assert.IsAssignableFrom<List<Pet>>(
                viewResult.ViewData.Model);
            Assert.Equal(3, model.Count);
        }
    }
}