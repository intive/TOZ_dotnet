using System;
using Toz.Dotnet.Models.EnumTypes;

namespace Toz.Dotnet.Models.ViewModels
{
    public class ReservationToken
    {       
        public DateTime Date {get;set;}
        public Period TimeOfDay {get;set;}
        public string FirstName {get;set;}
        public string LastName {get;set;}
    }
}