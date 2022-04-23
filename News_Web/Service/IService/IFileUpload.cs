using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News_Web.IService
{
    public interface IFileUpload
    {
        public Task<string> UploadFile(IFormFile file,string ImageType);


        public Task<bool> DeleteFile(string fileName, string ImageType);
        
        public Task<string> GetFileAddress(string fileName);

    }
}
