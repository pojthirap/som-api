using MyFirstAzureWebApp.common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Utils
{
    public static class FilesUtils
    {

        public static string getFileUrl(string containerName, string fileId)
        {
            string id = containerName + "_" + fileId;
            string Key = GenerateKey.generateFileIdKey(id);
            string fileUrl = CommonConstant.GET_File_CONTROLLER + Key; // url fo call ImageController: Task<IActionResult>  getImg(string id)
            return fileUrl;
        }

        public static Fileproperty getFileproperty(String fileUrl)
        {
            string text = GenerateKey.getFileId(fileUrl);
            string containerName = text.Split("_")[0];
            string fileId = text.Split("_")[1];// split container name and File name
            Fileproperty f = new Fileproperty();
            f.ContainerName = containerName;
            f.FileId = fileId;
            return f;
        }

        public class Fileproperty
        {
            public string ContainerName { get; set; }
            public string FileId { get; set; }
        }
        public static string getFileExt(string fileName)
        {
            string[] fileExt = fileName.Split(".");
            string fileExtStr = fileExt[fileExt.Length - 1];
            return fileExtStr;
        }

        public static string getGenerateFileName(string PhotoFlag, string FileName, string timeZone)
        {
            return (("N".Equals(PhotoFlag)) ? Path.GetFileNameWithoutExtension(FileName)+"_" : "ภาพถ่าย" + "_") + DateTime.Now.AddHours(Int32.Parse(timeZone)).ToString("yyyyMMddHHmmss", new CultureInfo("th-TH"));
        }
    }
}
