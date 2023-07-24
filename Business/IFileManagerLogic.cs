using MyFirstAzureWebApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Business
{
    public interface IFileManagerLogic
    {
        Task Upload(FileModel model);
        Task Upload(FileModel model, string fileName, string container, string fileExt, string fileNameOriginal, string fullPath);
        Task<FileBlobProperty> Get(string imageName, string container);
        Task<byte[]> Get(string imageName);
        Task Delete(string imageName);
        Task<byte[]> GetByTag(string imageName);
        Task<List<byte[]>> GetByTagMulti(string imageName);
    }
    
}
