using System;
using System.Collections.Generic;
using System.Text;

namespace Toz.Dotnet.Models.Images
{
    public class Photo
    {
        public string Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string Path { get; set; }
        public string FileUrl { get; set; }
    }
}
