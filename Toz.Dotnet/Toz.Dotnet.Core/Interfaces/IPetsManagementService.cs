using System.Collections.Generic;
using Toz.Dotnet.Models;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IPetsManagementService
    {
         List<Pet> GetPetsList();
         bool UpdatePet(Pet pet);
         bool DeletePet(Pet pet);
         bool AddPet(Pet pet);
         Pet GetPet(int id);
    }
}