using System.Collections.Generic;
using Toz.Dotnet.Models;

namespace Toz.Dotnet.Core.Interfaces
{
    public interface IPetsManagementService
    {
         List<Pet> GetPetsList();
         List<Pet> GetSamplePetsList();
         void AddPet(Pet pet);
    }
}