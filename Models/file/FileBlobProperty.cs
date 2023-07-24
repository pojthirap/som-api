using Microsoft.AspNetCore.Http;

namespace MyFirstAzureWebApp.Models
{
    public class FileBlobProperty
    {
        public byte[] FileByte { get; set; }
        public string ContentType { get; set; }
        public string Extension { get; set; }
        public string Name { get; set; }
    }


}
