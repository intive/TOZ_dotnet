using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Toz.Dotnet.Models.Schedule.ViewModels
{
    public class ReservationToken
    {
        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        public DateTime Date { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        public EnumTypes.Period TimeOfDay { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        public string VolunteerId { get; set; }

        public SelectList Volunteers { get; set; }
    }
}
