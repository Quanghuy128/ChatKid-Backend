using ChatKid.GoogleServices.GoogleCloudStorage.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatKid.GoogleServices.GoogleCloudStorage
{
    public interface ICloudStorageService
    {
        Task<FileViewModel> UploadFile(IFormFile file);
    }
}
