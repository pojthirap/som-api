using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MyFirstAzureWebApp.Business;
using MyFirstAzureWebApp.Business.org;
using MyFirstAzureWebApp.common;
using MyFirstAzureWebApp.Entity.custom;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Models.importfile;
using MyFirstAzureWebApp.Models.profile;
using MyFirstAzureWebApp.Models.record;
using MyFirstAzureWebApp.SearchCriteria;
using MyFirstAzureWebApp.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static MyFirstAzureWebApp.Utils.FilesUtils;

namespace MyFirstAzureWebApp.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class ImageController : BaseController
    {
        // See https://www.learmoreseekmore.com/2021/02/dotnet5-web-api-managing-files-using-azure-blob-storage.html
        // create a Blob Container name is upload-file first
        private readonly IFileManagerLogic _fileManagerLogic;
        private IRecordAppFormFile recordAppFormFileImp;
        private IRecordSaFormFile recordSaFormFileImp;
        private IRecordMeterFile recordMeterFileImp;
        private IImportExcelFile importExcelFileImp;
        private IImportErrorFileLog importErrorFileLogImp;

        public ImageController(IFileManagerLogic fileManagerLogic, IWebHostEnvironment hostingEnvironment)
        {
            _fileManagerLogic = fileManagerLogic;
            recordAppFormFileImp = new RecordAppFormFileImp();
            recordSaFormFileImp = new RecordSaFormFileImp();
            recordMeterFileImp = new RecordMeterFileImp();
            importExcelFileImp = new ImportExcelFileImp(hostingEnvironment);
            importErrorFileLogImp = new ImportErrorFileLogImp();
        }





        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// <p>RecAppFormId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("uploadFileAppForm")]
        public async Task<ResponseResult> uploadFileAppForm(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, [FromForm] UploadFileAppFormModel uploadFileAppFormModel)
        {
            try
            {
                onAfterReceiveRequest("uploadFileAppForm", "uploadFileAppForm", uploadFileAppFormModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                RecordAppFormFile recordAppFormFile = null;
                if (uploadFileAppFormModel.ImageFile != null)
                {
                    int fileId = await recordAppFormFileImp.GetRecordAppFromFileSeq();
                    // Upload File
                    string timeZone = await getConfigAsync("HOUR_TIME_ZONE");
                    string generateFileName = FilesUtils.getGenerateFileName(uploadFileAppFormModel.PhotoFlag, uploadFileAppFormModel.ImageFile.FileName, timeZone);
                    string fileExtStr = FilesUtils.getFileExt(uploadFileAppFormModel.ImageFile.FileName);
                    await _fileManagerLogic.Upload(uploadFileAppFormModel, fileId.ToString(), CommonConstant.IMG_CONTAINER_SOM_RECORD_APP_FORM_FILE, fileExtStr, generateFileName, null);
                    //Insert Data
                    RecordAppFormFileModel recordAppFormFileModel = new RecordAppFormFileModel();
                    recordAppFormFileModel.FileId = fileId.ToString();
                    //recordAppFormFileModel.RecAppFormId = uploadFileAppFormModel.RecAppFormId;
                    recordAppFormFileModel.FileExt = fileExtStr;
                    recordAppFormFileModel.FileName = generateFileName;//recordAppFormFileModel.FileId;
                    recordAppFormFileModel.FileSize = uploadFileAppFormModel.ImageFile.Length.ToString();
                    recordAppFormFileModel.PhotoFlag = uploadFileAppFormModel.PhotoFlag;
                    recordAppFormFile = await recordAppFormFileImp.uploadFileAppForm(recordAppFormFileModel, userProfileForBack);
                    recordAppFormFile.FileId = fileId;
                }

                SearchResultBase<RecordAppFormFile> searchResult = new SearchResultBase<RecordAppFormFile>();
                List<RecordAppFormFile> list = new List<RecordAppFormFile>();
                list.Add(recordAppFormFile);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("uploadFileAppForm", "uploadFileAppForm", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("uploadFileAppForm", "uploadFileAppForm", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// <p>RecSaFormId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("uploadFileSaForm")]
        public async Task<ResponseResult> uploadFileSaForm(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, [FromForm] UploadFileSaFormModel uploadFileSaFormModel)
        {
            try
            {
                onAfterReceiveRequest("uploadFileSaForm", "uploadFileSaForm", uploadFileSaFormModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                RecordSaFormFile recordSaFormFile = null;
                if (uploadFileSaFormModel.ImageFile != null)
                {
                    int fileId = await recordSaFormFileImp.GetRecordSaFromFileSeq();
                    // Upload File
                    string timeZone = await getConfigAsync("HOUR_TIME_ZONE");
                    string generateFileName = FilesUtils.getGenerateFileName(uploadFileSaFormModel.PhotoFlag, uploadFileSaFormModel.ImageFile.FileName, timeZone);
                    string fileExtStr = FilesUtils.getFileExt(uploadFileSaFormModel.ImageFile.FileName);
                    await _fileManagerLogic.Upload(uploadFileSaFormModel, fileId.ToString(), CommonConstant.IMG_CONTAINER_SOM_RECORD_SA_FORM_FILE, fileExtStr, generateFileName, null);
                    //Insert Data
                    RecordSaFormFileModel recordSaFormFileModel = new RecordSaFormFileModel();
                    recordSaFormFileModel.FileId = fileId.ToString();
                    //recordSaFormFileModel.RecSaFormId = uploadFileSaFormModel.RecSaFormId;
                    recordSaFormFileModel.FileExt = fileExtStr;
                    recordSaFormFileModel.FileName = generateFileName;// recordSaFormFileModel.FileId;
                    recordSaFormFileModel.FileSize = uploadFileSaFormModel.ImageFile.Length.ToString();
                    recordSaFormFileModel.PhotoFlag = uploadFileSaFormModel.PhotoFlag;
                    recordSaFormFile = await recordSaFormFileImp.uploadFileSaForm(recordSaFormFileModel, userProfileForBack);
                    recordSaFormFile.FileId = fileId;
                }

                SearchResultBase<RecordSaFormFile> searchResult = new SearchResultBase<RecordSaFormFile>();
                List<RecordSaFormFile> list = new List<RecordSaFormFile>();
                list.Add(recordSaFormFile);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("uploadFileSaForm", "uploadFileSaForm", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("uploadFileSaForm", "uploadFileSaForm", resR);
                return resR;
            }
        }




        /// <summary>
        /// </summary>
        /// <remarks> 
        /// <a href="http://119.59.116.129/~saleonmobi/salesvisit/visit/" target="_blank">For Page salesvisit/visit/</a>
        /// <p>Required Paramerer</p>
        /// <p>RecSaFormId</p>
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("uploadFileMeter")]
        public async Task<ResponseResult> uploadFileMeter(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, [FromForm] UploadFileMeterModel uploadFileMeterModel)
        {
            try
            {
                onAfterReceiveRequest("uploadFileMeter", "uploadFileMeter", uploadFileMeterModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                RecordMeterFile recordMeterFile = null;
                if (uploadFileMeterModel.ImageFile != null)
                {
                    int fileId = await recordMeterFileImp.GetRecordMeterFileSeq();
                    // Upload File
                    string timeZone = await getConfigAsync("HOUR_TIME_ZONE");
                    string generateFileName = FilesUtils.getGenerateFileName(uploadFileMeterModel.PhotoFlag, uploadFileMeterModel.ImageFile.FileName, timeZone);
                    string fileExtStr = FilesUtils.getFileExt(uploadFileMeterModel.ImageFile.FileName);
                    await _fileManagerLogic.Upload(uploadFileMeterModel, fileId.ToString(), CommonConstant.IMG_CONTAINER_SOM_RECORD_METER_FILE, fileExtStr, generateFileName, null);
                    //Insert Data
                    RecordMeterFileModel recordMeterFileModel = new RecordMeterFileModel();
                    recordMeterFileModel.FileId = fileId.ToString();
                    recordMeterFileModel.FileExt = fileExtStr;
                    recordMeterFileModel.FileName = generateFileName;// recordMeterFileModel.FileId;
                    recordMeterFileModel.FileSize = uploadFileMeterModel.ImageFile.Length.ToString();
                    recordMeterFile = await recordMeterFileImp.uploadFileMeter(recordMeterFileModel, userProfileForBack);
                    recordMeterFile.FileId = fileId;
                }

                SearchResultBase<RecordMeterFile> searchResult = new SearchResultBase<RecordMeterFile>();
                List<RecordMeterFile> list = new List<RecordMeterFile>();
                list.Add(recordMeterFile);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("uploadFileMeter", "uploadFileMeter", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("uploadFileMeter", "uploadFileMeter", resR);
                return resR;
            }
        }








        /// <summary>
        /// </summary>
        /// <remarks> 
        /// Import Propect
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("importProspect")]
        public async Task<ResponseResult> importProspect(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, [FromForm] ImportProspectModel importProspectModel)
        {
            try
            {
                onAfterReceiveRequest("importProspect", "importProspect", importProspectModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                ImportProspectCustom impCus = null;
                if (importProspectModel.ImageFile != null)
                {

                    impCus = await importExcelFileImp.importProspect(_fileManagerLogic, importErrorFileLogImp, importProspectModel, userProfileForBack);
                    if (!String.IsNullOrEmpty(impCus.PathFileError))
                    {
                        impCus.PathFileError = FilesUtils.getFileUrl(CommonConstant.IMG_CONTAINER_SOM_IMPORT_ERROR_FILE, impCus.PathFileError);
                    }
                    
                }

                SearchResultBase<ImportProspectCustom> searchResult = new SearchResultBase<ImportProspectCustom>();
                List<ImportProspectCustom> list = new List<ImportProspectCustom>();
                list.Add(impCus);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("importProspect", "importProspect", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("importProspect", "importProspect", resR);
                return resR;
            }
        }



        /// <summary>
        /// </summary>
        /// <remarks> 
        /// Import Employee To Sale Group
        /// </remarks>
        /// <returns></returns>
        /// <respose code="200"></respose>
        [HttpPost("importEmpToSaleGroup")]
        public async Task<ResponseResult> importEmpToSaleGroup(
        [FromHeader(Name = "Accept-Language")] string language,
        [FromHeader(Name = "User-Agent")] string agent, [FromForm] ImportEmpToSaleGroupModel importEmpToSaleGroupModel)
        {
            try
            {
                onAfterReceiveRequest("importEmpToSaleGroup", "importEmpToSaleGroup", importEmpToSaleGroupModel);
                string _bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
                UserProfileForBack userProfileForBack = await getUserPrifileForBack(_bearer_token, language);
                ImportEmpToSaleGroupCustom impCus = null;
                if (importEmpToSaleGroupModel.ImageFile != null)
                {

                    impCus = await importExcelFileImp.importEmpToSaleGroup(_fileManagerLogic, importErrorFileLogImp, importEmpToSaleGroupModel, userProfileForBack);
                    if (!String.IsNullOrEmpty(impCus.PathFileError))
                    {
                        impCus.PathFileError = FilesUtils.getFileUrl(CommonConstant.IMG_CONTAINER_SOM_IMPORT_ERROR_FILE, impCus.PathFileError);
                    }

                }

                SearchResultBase<ImportEmpToSaleGroupCustom> searchResult = new SearchResultBase<ImportEmpToSaleGroupCustom>();
                List<ImportEmpToSaleGroupCustom> list = new List<ImportEmpToSaleGroupCustom>();
                list.Add(impCus);
                searchResult.totalRecords = list.Count;
                searchResult.records = list;
                ResponseResult res = ResponseResult.warp(searchResult);
                res.agent = agent;
                onBeforeSendResponse("importEmpToSaleGroup", "importEmpToSaleGroup", res);
                return res;
            }
            catch (Exception e)
            {
                ResponseResult resR = ResponseResult.warpError(e);
                resR.agent = agent;
                onBeforeSendResponse("importEmpToSaleGroup", "importEmpToSaleGroup", resR);
                return resR;
            }
        }



        [Route("upload")]
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] FileModel model)
        {
            if (model.ImageFile != null)
            {
                await _fileManagerLogic.Upload(model);
            }
            return Ok();
        }

        [Route("download")]
        [HttpGet]
        public async Task<IActionResult> Download(string fileName)
        {
            var imagBytes = await _fileManagerLogic.Get(fileName);
            return new FileContentResult(imagBytes, "application/octet-stream")
            {
                FileDownloadName = Guid.NewGuid().ToString() + ".webp",
            };
        }

        [AllowAnonymous]
        [Route("downloadByTag")]
        [HttpGet]
        public async Task<IActionResult> DownloadByTag(string tag)
        {
            var imagBytes = await _fileManagerLogic.GetByTag(tag);
            return new FileContentResult(imagBytes, "application/octet-stream")
            {
                FileDownloadName = Guid.NewGuid().ToString() + ".webp",
            };
        }

        [AllowAnonymous]
        [HttpGet("getFile/{id}", Name = "getFile")]
        public async Task<IActionResult> getFile(string id)
        {
            //string n_ = "W-1.jpg";
            Fileproperty fp = FilesUtils.getFileproperty(id);
            //var fileBytes = await _fileManagerLogic.Get(fp.FileId, fp.ContainerName);
            FileBlobProperty fbp = await _fileManagerLogic.Get(fp.FileId, fp.ContainerName);
            //string fileDownloadName = fp.FileId +"."+ fbp.Extension;
            string fileDownloadName = fbp.Name;
            return File(fbp.FileByte, fbp.ContentType, fileDownloadName);

            ///return "data:image/png;base64," + Convert.ToBase64String(imagBytes);

            /*return new FileContentResult(imagBytes, "application/octet-stream")
            {
                FileDownloadName = Guid.NewGuid().ToString() + ".webp",
            };*/
        }


        [AllowAnonymous]
        [Route("downloadByTagMulti")]
        [HttpGet]
        public async Task<List<byte[]>> DownloadByTagMulti(string tag)
        {
            var imagBytes = await _fileManagerLogic.GetByTagMulti(tag);
            return imagBytes;
            /*return new FileContentResult(imagBytes, "application/octet-stream")
            {
                FileDownloadName = Guid.NewGuid().ToString() + ".webp",
            };*/
        }
    }
}
