using System.Collections.Generic;
using Toz.Dotnet.Models;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IPetsManagementService
    {
		Pet GetPet(int id);
        List<Pet> GetAllPets();
        List<Pet> GetSamplePets();
        bool UpdatePet(Pet pet);
        bool DeletePet(Pet pet);
        bool AddPet(Pet pet);      
    }
}