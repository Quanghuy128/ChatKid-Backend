using ChatKid.GoogleServices.GoogleCloudStorage.ViewModel;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.GoogleServices.GoogleCloudStorage
{
    public class CloudStorageService : ICloudStorageService
    {
        private readonly StorageClient storageClient;
        private readonly string bucketName;
        public CloudStorageService(IConfiguration configuration)
        {
            var googleCredential = GoogleCredential.FromFile(configuration.GetValue<string>("GoogleCredentialFile"));
            storageClient = StorageClient.Create(googleCredential);
            bucketName = configuration.GetValue<string>("GoogleCloudStorageBucket");
        }
        public async Task<FileViewModel> UploadFile(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                Guid id = Guid.NewGuid();
                string extension = Path.GetExtension(file.FileName);
                string objectName = $"{id}{extension}";
                await storageClient.UploadObjectAsync(bucketName, objectName, null, memoryStream);
                return new FileViewModel
                {
                    Name = objectName,
                    Url = $"https://storage.googleapis.com/{bucketName}/{objectName}"
                };
            }
        }
    }
}
