using System;

namespace Toz.Dotnet.Models.Schedule.ViewModels
{
    public class ReservationToken
    {       
        public DateTime Date {get;set;}

        public EnumTypes.Period TimeOfDay {get;set;}

        //[Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        //[StringLength(30, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]
        //[RegularExpression(@"(([^\u0000-\u007F]|[a-zA-Z])+[\-\']?([^\u0000-\u007F]|[a-zA-Z]|(\s(?!$))+)?)*", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "FirstnameLetters")]
        //public string FirstName {get;set;}

        //[Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        //[StringLength(30, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]
        //[RegularExpression(@"(([^\u0000-\u007F]|[a-zA-Z])+[\-\']?([^\u0000-\u007F]|[a-zA-Z]|(\s(?!$))+)?)*", ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "LastnameLetters")]
        //public string LastName {get;set;}

        public UserBase Volunteer { get; set; }
    }
}
