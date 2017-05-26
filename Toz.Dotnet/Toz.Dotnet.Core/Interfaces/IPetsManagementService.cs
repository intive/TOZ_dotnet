using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Toz.Dotnet.Models;
using System.Threading;

namespace Toz.Dotnet.Core.Interfaces
{

    public interface IPetsManagementService
    {
		Task<Pet> GetPet(string id, string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<List<Pet>> GetAllPets(string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> UpdatePet(Pet pet, string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> DeletePet(Pet pet, string token, CancellationToken cancelationToken = default(CancellationToken));
        Task<bool> CreatePet(Pet pet, string token, CancellationToken cancelationToken = default(CancellationToken));
        byte[] ConvertPhotoToByteArray(Stream fileStream);
        string RequestUri { get; set; }
    }
}