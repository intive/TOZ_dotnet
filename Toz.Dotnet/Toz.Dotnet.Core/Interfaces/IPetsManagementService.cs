using System.Collections.Generic;
using Toz.Dotnet.Models;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IPetsManagementService
    {
		Pet GetPet(int id);
        List<Pet> GetAllPets();
        bool UpdatePet(Pet pet);
        bool DeletePet(Pet pet);
        bool CreatePet(Pet pet);      
    }
}