using System;
using System.Collections.Generic;
using System.Text;

namespace LuxeCatalog.Business.Services.Interfaces
{
    public interface IStorageService
    {
        Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string folder);
        Task<bool> DeleteAsync(string fileUrl);
    }
}
