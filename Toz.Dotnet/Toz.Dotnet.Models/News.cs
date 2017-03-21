using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Toz.Dotnet.Models.EnumTypes;
using Toz.Dotnet.Models.JsonConventers;

namespace Toz.Dotnet.Models
{
    public class News
    {
        public string Id {get; set;}

        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]    
        public string Title {get; set;}

        public DateTime PublishingTime  {get; set;}

        public DateTime AddingTime  {get; set;}

        public DateTime LastEditTime {get; set;}

        [Required(ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "EmptyField")]
        [StringLength(1000, ErrorMessageResourceType = typeof(Resources.ModelsDataValidation), ErrorMessageResourceName = "MaxLength")]   
        public string Body {get; set;}

        public byte [] Photo {get; set;}

        public NewsStatus Status {get; set;}

        public News(string id, string title, DateTime publishingTime, DateTime addingTime, DateTime lastEditTime,
                    string body, byte[] photo, NewsStatus status)
                {
                    Id = id;
                    Title = title;
                    PublishingTime = publishingTime;
                    AddingTime = addingTime;
                    LastEditTime = lastEditTime;
                    Body = body;
                    Photo = photo;
                 }
        }
}