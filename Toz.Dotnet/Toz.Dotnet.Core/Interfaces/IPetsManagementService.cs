using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Toz.Dotnet.Models;

namespace Toz.Dotnet.Core.Interfaces
{

    public interface IPetsManagementService
    {
		Task<Pet> GetPet(string id);
        Task<List<Pet>> GetAllPets();
        Task<bool> UpdatePet(Pet pet);
        bool DeletePet(Pet pet);
        Task<bool> CreatePet(Pet pet);
        byte[] ConvertPhotoToByteArray(Stream fileStream);
    }
}