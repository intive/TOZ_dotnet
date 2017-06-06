using Microsoft.AspNetCore.Mvc.Rendering;

namespace Toz.Dotnet.Models.ViewModels

{
    public class PetViewModel
    {
        public Pet ThePet { get; set; }
        public PetsStatus ThePetStatus { get; set; }
        public SelectList TheStatusList { get; set; }
        public Helper TheHelper { get; set; }
        public SelectList TheHelpersList { get; set; }
    }
}
