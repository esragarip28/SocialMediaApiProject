using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DemoApplication1.ResultJsonClass;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
namespace DemoApplication1.ExternalServices.Cloudinary
{
    public class CloudinaryService
    {

        CloudinaryDotNet.Cloudinary cloudinary;
        public CloudinaryService()
        {
            Account account = new Account(
            Credentials.cloudName, Credentials.apiKey, Credentials.secretKey
            );
            cloudinary = new CloudinaryDotNet.Cloudinary(account);
            cloudinary.Api.Secure = true;

        }


        public ImageUploadResult UploadImage(string path)
        {

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(path)
            };
            var uploadResult = cloudinary.Upload(uploadParams); //it return imageuploadresult(json object)
            return uploadResult;


        }

        public string GetUploadedImageUrl(IFormFile file)
        {

            if (file.Length > 0)
            {

                string filePath = Path.GetTempFileName();

                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
                CloudinaryService cloudinary = new CloudinaryService();
                var uploadResult = cloudinary.UploadImage(filePath);
                return uploadResult.SecureUrl.ToString();

            }
            return "Please select an image to share on instagram";


        }
        /// <summary>
        /// instead of UploadLarge you can use only Upload.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>

        public string GetUploadedVideoUrl(IFormFile file)
        {
            if (file.Length > 0)
            {
                string filePath = Path.GetTempFileName();

                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);

                }
                var uploadParams = new VideoUploadParams()
                {
                    File = new FileDescription(filePath)
                };
                

                var uploadResult = cloudinary.UploadLarge(uploadParams);
                return uploadResult.SecureUrl.ToString();
            }
            return "please select any video to share";
        }
    }
}