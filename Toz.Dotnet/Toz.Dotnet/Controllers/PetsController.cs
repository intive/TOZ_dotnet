using System;

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Toz.Dotnet.Models;
using Toz.Dotnet.Models.EnumTypes;
using Toz.Dotnet.Core.Interfaces;

namespace Toz.Dotnet.Controllers {
    public class PetsController : Controller
    {
        private IPetsManagementService _petsManagementService;
        private List<Pet> _petsList = new List<Pet>();
        public PetsController(IPetsManagementService petsManagementService)
        {
            _petsManagementService = petsManagementService;
        }  

        public ActionResult Index() {
            _petsList.Add(new Pet(0,"Tofik",PetType.Dog,PetSex.Female,new byte[0],"Przyjacielski piesek","Znaleziono na Bohaterow Warszawy",DateTime.Parse("2017-03-06 13:26"),DateTime.Parse("2017-03-07 13:55")));
            _petsList.Add(new Pet(1,"Filemon",PetType.Cat,PetSex.Male,new byte[0],"Przyjacielski kotek","Znaleziono na Wydziale Informatyki ZUT",DateTime.Parse("2017-03-05 22:11"),DateTime.Parse("2017-03-07 12:55")));
            _petsList.Add(new Pet(2,"Bonifacy",PetType.Cat,PetSex.Male,new byte[0],"Bystry kotek","Znaleziony w Parku Kasprowicza",DateTime.Parse("2017-03-07 10:11"),DateTime.Parse("2017-03-07 12:34")));
            return View(_petsList);
        }
    }
}