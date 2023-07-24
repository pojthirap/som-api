using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MyFirstAzureWebApp.Models;
using MyFirstAzureWebApp.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MyFirstAzureWebApp.Business
{
    //https://docs.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet
    //https://dev.to/pietervdw/searching-azure-blob-storage-files-using-blob-index-lok
    //https://docs.microsoft.com/en-us/azure/storage/blobs/storage-blob-index-how-to?tabs=net

    //https://docs.microsoft.com/en-us/azure/storage/blobs/storage-blob-index-how-to?tabs=net
    public class FileManagerLogic : IFileManagerLogic
    {
        private Logger log = LogManager.GetCurrentClassLogger();
        private readonly BlobServiceClient _blobServiceClient;
        public FileManagerLogic(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task Delete(string imageName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("upload-file");

            var blobClient = blobContainer.GetBlobClient(imageName);

            await blobClient.DeleteAsync();
        }
        
        public async Task<FileBlobProperty> Get(string imageName, string container)
        {
            Console.WriteLine("Get File Name:"+ imageName+" Container:"+ container);
            log.Debug("Get File Name:" + imageName + " Container:" + container);
            var blobContainer = _blobServiceClient.GetBlobContainerClient(container);

            var blobClient = blobContainer.GetBlobClient(imageName);
            var downloadContent = await blobClient.DownloadAsync();
            Response<GetBlobTagResult> res = blobClient.GetTags();
            GetBlobTagResult tagsRes = res.Value;

            string ContentType = tagsRes.Tags["ContentType"];
            string Extension = tagsRes.Tags["Extension"];
            string Name = tagsRes.Tags["Name"];
            Name = Base64Utils.Base64StringDecode(Name);
            FileBlobProperty fbp = new FileBlobProperty();
            fbp.ContentType = ContentType;
            fbp.Extension = Extension;
            fbp.Name = Name;

            /*foreach (KeyValuePair<string, string> tag in tagsRes.Tags)
            {
                Console.WriteLine($"{tag.Key}={tag.Value}");
                log.Debug("Tag:" + $"{tag.Key}={tag.Value}");
            }*/
            //
            Console.WriteLine("Listing blobs...");

            // List all blobs in the container
            /*await foreach (BlobItem blobItem in blobContainer.GetBlobsAsync())
            {
                Console.WriteLine("\t" + blobItem.Name);
            }*/
            //
            using (MemoryStream ms = new MemoryStream())
            {
                await downloadContent.Value.Content.CopyToAsync(ms);
                String uri = blobClient.Uri.AbsoluteUri;
                String uri_path = blobClient.Uri.AbsolutePath;
                fbp.FileByte = ms.ToArray();
                return fbp;
                //return ms.ToArray();
            }
        }

        public async Task<byte[]> Get(string imageName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("upload-file");

            var blobClient = blobContainer.GetBlobClient(imageName);
            var downloadContent = await blobClient.DownloadAsync();
            //
            Console.WriteLine("Listing blobs...");

            // List all blobs in the container
            await foreach (BlobItem blobItem in blobContainer.GetBlobsAsync())
            {
                Console.WriteLine("\t" + blobItem.Name);
            }
            //
            using (MemoryStream ms = new MemoryStream())
            {
                await downloadContent.Value.Content.CopyToAsync(ms);
                String uri = blobClient.Uri.AbsoluteUri;
                String uri_path = blobClient.Uri.AbsolutePath;
                return ms.ToArray();
            }
        }


        public async Task<byte[]> GetByTag(string tag)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("som-record-meter-file");


            // Blob index queries and selection
            String singleEqualityQuery = @"""Extension"" = 'jpg'";
            String andQuery = @"""Archive"" = 'false' AND ""Priority"" = '01'";
            //String rangeQuery = @"""Date"" >= '2021-06-20' AND ""Date"" <= '2021-06-23'";
            String rangeQuery = @"""Date"" >= '2021-06-24' AND ""Date"" <= '2021-06-29'";
            String containerScopedQuery = @"@container = 'mycontainer' AND ""Archive"" = 'false'";
            String QueryID = @"""template"" = '001'";

            String queryToUse = QueryID;

            // Find Blobs given a tags query
            Console.WriteLine("Find Blob by Tags query: " + queryToUse + Environment.NewLine);

            List<TaggedBlobItem> blobs = new List<TaggedBlobItem>();
            await foreach (TaggedBlobItem taggedBlobItem in _blobServiceClient.FindBlobsByTagsAsync(queryToUse))
            {
                blobs.Add(taggedBlobItem);
                Console.WriteLine(taggedBlobItem.BlobName);
            }
            Console.WriteLine("============= XXXXXXXXXXXXXX ========================");
            string imageName = "";
            foreach (var filteredBlob in blobs)
            {
                Console.WriteLine($"BlobIndex result: ContainerName= {filteredBlob.BlobContainerName}, " +
                    $"BlobName= {filteredBlob.BlobName}");
                imageName = filteredBlob.BlobName;
                Console.WriteLine(filteredBlob.BlobName);
            }
            //return null;
            var blobClient = blobContainer.GetBlobClient(imageName);
            var downloadContent = await blobClient.DownloadAsync();
            //
            Console.WriteLine("Listing blobs...");

            // List all blobs in the container
            await foreach (BlobItem blobItem in blobContainer.GetBlobsAsync())
            {
                Console.WriteLine("\t" + blobItem.Name);
            }
            //
            using (MemoryStream ms = new MemoryStream())
            {
                await downloadContent.Value.Content.CopyToAsync(ms);
                String uri = blobClient.Uri.AbsoluteUri;
                String uri_path = blobClient.Uri.AbsolutePath;
                return ms.ToArray();
            }
        }

        public async Task<byte[]> GetByContainer(string tag)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("upload-file");


            // Blob index queries and selection
            String singleEqualityQuery = @"""Extension"" = 'jpg'";
            String andQuery = @"""Archive"" = 'false' AND ""Priority"" = '01'";
            //String rangeQuery = @"""Date"" >= '2021-06-20' AND ""Date"" <= '2021-06-23'";
            String rangeQuery = @"""Date"" >= '2021-06-24' AND ""Date"" <= '2021-06-29'";
            String containerScopedQuery = @"@container = 'mycontainer' AND ""Archive"" = 'false'";
            String QueryID = @"""template"" = '001'";

            String queryToUse = QueryID;

            // Find Blobs given a tags query
            Console.WriteLine("Find Blob by Tags query: " + queryToUse + Environment.NewLine);

            List<TaggedBlobItem> blobs = new List<TaggedBlobItem>();
            await foreach (TaggedBlobItem taggedBlobItem in _blobServiceClient.FindBlobsByTagsAsync(queryToUse))
            {
                blobs.Add(taggedBlobItem);
                Console.WriteLine(taggedBlobItem.BlobName);
            }
            Console.WriteLine("============= XXXXXXXXXXXXXX ========================");
            string imageName = "";
            foreach (var filteredBlob in blobs)
            {
                Console.WriteLine($"BlobIndex result: ContainerName= {filteredBlob.BlobContainerName}, " +
                    $"BlobName= {filteredBlob.BlobName}");
                imageName = filteredBlob.BlobName;
                Console.WriteLine(filteredBlob.BlobName);
            }
            //return null;
            var blobClient = blobContainer.GetBlobClient(imageName);
            var downloadContent = await blobClient.DownloadAsync();
            //
            Console.WriteLine("Listing blobs...");

            // List all blobs in the container
            await foreach (BlobItem blobItem in blobContainer.GetBlobsAsync())
            {
                Console.WriteLine("\t" + blobItem.Name);
            }
            //
            using (MemoryStream ms = new MemoryStream())
            {
                await downloadContent.Value.Content.CopyToAsync(ms);
                String uri = blobClient.Uri.AbsoluteUri;
                String uri_path = blobClient.Uri.AbsolutePath;
                return ms.ToArray();
            }
        }

        public async Task Upload(FileModel model, string fileName, string container, string fileExt, string fileNameOriginal, string fullPath)
        {

            Console.WriteLine("Upload File Name:" + fileName + " Container:" + container);
            log.Debug("Upload File Name:" + fileName + " Container:" + container);
            try
            {
                string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
                string dateNowUtc = DateTime.UtcNow.ToString("yyyy-MM-dd");
                string contentType = model.ImageFile.ContentType;
                fileNameOriginal = Base64Utils.Base64StringEncode(fileNameOriginal);
                Dictionary<string, string> tags1 = new Dictionary<string, string>
                {
                                  { "ID", fileName },
                                  { "ContentType", contentType },
                                  { "Extension", fileExt },
                                  { "Name", fileNameOriginal },
                                  { "Date", dateNow }
                };

                var blobContainer = _blobServiceClient.GetBlobContainerClient(container);

                var blobClient = blobContainer.GetBlobClient(fileName);


                if (String.IsNullOrEmpty(fullPath))
                {
                    var data_ = await blobClient.UploadAsync(model.ImageFile.OpenReadStream());
                }
                else
                {
                    var data_ = await blobClient.UploadAsync(fullPath);
                }
                var dataTag_ = await  blobClient.SetTagsAsync(tags1);
                Console.WriteLine("DataTag:" + dataTag_);
                log.Debug("DataTag:" + dataTag_);
                //Console.WriteLine(data_);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                log.Debug(ex.Message);
                throw;
            }
        }


        public async Task Upload(FileModel model)
        {
            try
            {
                string dateNow = DateTime.Now.ToString("yyyy-MM-dd");
                string dateNowUtc = DateTime.UtcNow.ToString("yyyy-MM-dd");
                string contentType = model.ImageFile.ContentType;
                string[] extensionArr = model.ImageFile.FileName.Split(".");
                string extension = extensionArr[extensionArr.Length - 1];
                string timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString();
                Dictionary<string, string> tags1 = new Dictionary<string, string>
                {
                                  { "ID", timestamp },
                                  { "ContentType", contentType },
                                  { "Extension", extension },
                                  { "Name", model.ImageFile.FileName },
                                  { "Date", dateNow }
                                  //{ "Date", "2020-04-20" }
                };

                var blobContainer = _blobServiceClient.GetBlobContainerClient("upload-file");

                var blobClient = blobContainer.GetBlobClient(model.ImageFile.FileName);


                var data_ = await blobClient.UploadAsync(model.ImageFile.OpenReadStream());
                var dataTag_ = await blobClient.SetTagsAsync(tags1);
                Console.WriteLine(data_);
                Console.WriteLine(dataTag_);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }



        public async Task<List<byte[]>> GetByTagMulti(string imageName)
        {
            var blobContainer = _blobServiceClient.GetBlobContainerClient("upload-file");

            //var blobClient = blobContainer.GetBlobClient(imageName);
            //var downloadContent = await blobClient.DownloadAsync();


            //var containerClient = blobServiceClient.GetBlobContainerClient("mycontainer");
            //var blobs = containerClient.GetBlobs();

            var results = new List<string>();
            var resultsByte = new List<byte[]>();
            //byte[] resultsByte = Array.Empty<byte>();

            /*foreach (var blob in blobs)
            {
                var blobClient = containerClient.GetBlobClient(blob.Blob.Name);
                using var stream = new MemoryStream();
                await blobClient.DownloadToAsync(stream);
                stream.Position = 0;
                using var streamReader = new StreamReader(stream);
                var result = await streamReader.ReadToEndAsync();
                results.Add(result);
            }*/

            //
            Console.WriteLine("Listing blobs...");

            // List all blobs in the container
            await foreach (BlobItem blobItem in blobContainer.GetBlobsAsync())
            {
                Console.WriteLine("\t" + blobItem.Name);
                var blobClient = blobContainer.GetBlobClient(blobItem.Name);
                using var stream = new MemoryStream();
                await blobClient.DownloadToAsync(stream);
                stream.Position = 0;
                using var streamReader = new StreamReader(stream);
                var result = await streamReader.ReadToEndAsync();
                results.Add(result);

                resultsByte.Add(stream.ToArray());
            }
            Console.WriteLine(results);
            //return resultsByte[0];
            return resultsByte;
            //
            /*using (MemoryStream ms = new MemoryStream())
            {
                await downloadContent.Value.Content.CopyToAsync(ms);
                String uri = blobClient.Uri.AbsoluteUri;
                String uri_path = blobClient.Uri.AbsolutePath;
                return ms.ToArray();
            }*/
        }

    }
}
