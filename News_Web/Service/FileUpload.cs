using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using News_Common;
using News_Web.IService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace News_Web.Service
{
    public class FileUpload : IFileUpload
    {
        private IWebHostEnvironment _webHostEnvironment;
        private IHttpContextAccessor _httpContextAccessor;

        public FileUpload(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> DeleteFile(string fileName, string ImageType)
        {
            try
            {

                if (fileName != "no_photo.png")
                {
                    if (ImageType == SD.NewsImageType)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "NewsImages", fileName);

                        if (File.Exists(path))
                        {
                            File.Delete(path);
                            return true;
                        }
                        return false;

                    }
                    else
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "UserImages", fileName);

                        if (File.Exists(path))
                        {
                            File.Delete(path);
                            return true;
                        }
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public async Task<string> GetFileAddress(string fileName)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "UserImages", fileName);

                if (File.Exists(path))
                {

                    var Url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}/";
                    var UploadedfilePath = $"{Url}/images/UserImages/{fileName}";
                    return UploadedfilePath;

                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<string> UploadFile(IFormFile file, string ImageType)
        {

            try
            {
                if (ImageType == SD.NewsImageType)
                {
                    FileInfo fileInfo = new FileInfo(file.FileName);
                    if (fileInfo.Extension == ".png" ||
                        fileInfo.Extension == ".jpg" ||
                        fileInfo.Extension == ".jpeg")
                    {

                        var fileName = Guid.NewGuid().ToString() + fileInfo.Extension;
                        var FolderDirectory = $"{_webHostEnvironment.WebRootPath}\\images\\NewsImages";
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "NewsImages", fileName);
                        // Path.Combine(_webHostEnvironment.WebRootPath, "\\AdminContent\\NewsImages", fileName); 
                        var memoryStream = new MemoryStream();
                        await file.OpenReadStream().CopyToAsync(memoryStream);

                        if (!Directory.Exists(FolderDirectory))
                        {
                            Directory.CreateDirectory(FolderDirectory);
                        }


                        await using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                        {
                            memoryStream.WriteTo(fs);
                        }


                        var Url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}/";
                        var UploadedfilePath = $"{Url}/images/NewsImages/{fileName}";
                        return UploadedfilePath;

                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    FileInfo fileInfo = new FileInfo(file.FileName);
                    if (fileInfo.Extension == ".png" ||
                        fileInfo.Extension == ".jpg" ||
                        fileInfo.Extension == ".jpeg")
                    {

                        var fileName = Guid.NewGuid().ToString() + fileInfo.Extension;
                        var FolderDirectory = $"{_webHostEnvironment.WebRootPath}\\images\\UserImages";
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "UserImages", fileName);
                        // Path.Combine(_webHostEnvironment.WebRootPath, "\\AdminContent\\NewsImages", fileName); 
                        var memoryStream = new MemoryStream();
                        await file.OpenReadStream().CopyToAsync(memoryStream);

                        if (!Directory.Exists(FolderDirectory))
                        {
                            Directory.CreateDirectory(FolderDirectory);
                        }


                        await using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                        {
                            memoryStream.WriteTo(fs);
                        }


                        var Url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}/";
                        var UploadedfilePath = $"{Url}/images/UserImages/{fileName}";
                        return UploadedfilePath;

                    }
                    else
                    {
                        return null;
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
