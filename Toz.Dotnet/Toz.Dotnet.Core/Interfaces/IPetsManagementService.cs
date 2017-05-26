using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Toz.Dotnet.Models;
using System.Threading;

namespace Toz.Dotnet.Core.Interfaces
{

    public interface IPetsManagementService
    {
		Task<Pet> GetPet(string id, CancellationToken cancelationToken = default(CancellationToken));
        Task<List<Pet>> GetAllPets(CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> UpdatePet(Pet pet, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> DeletePet(Pet pet, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> CreatePet(Pet pet, CancellationToken cancelationToken = default(CancellationToken));
        byte[] ConvertPhotoToByteArray(Stream fileStream);
        string RequestUri { get; set; }
    }
}