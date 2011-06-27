using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using BetterTaskList.Helpers;
using System.Collections.Generic;

namespace BetterTaskList.Controllers
{
    /// <summary>
    /// MVC Controller class for file upload and download
    /// </summary>
    public class FileManagerController : Controller
    {

        /// <summary>
        /// action for file download
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="attachmentType"></param>
        /// <param name="attachmentValue"></param>
        /// <returns></returns>
        public ActionResult Download(string attachmentType, long attachmentValue, string fileName)
        {
            try
            {
                string pfn = Server.MapPath("~/App_Data/Attachments/" + attachmentType + "/" + attachmentValue + "/" + fileName);
                if (!System.IO.File.Exists(pfn))
                {
                    throw new ArgumentException("Invalid file name or file not exists!");
                }

                return new BinaryContentResult()
                {
                    FileName = fileName,
                    ContentType = "application/octet-stream",
                    Content = System.IO.File.ReadAllBytes(pfn)
                };


            }
            catch (Exception ex)
            {
                new EmailNotificationHelpers().ErrorNotification(ex.StackTrace);
            }
            // if we got this far then there was an error
            return Content("Error");
        }

        /// <summary>
        /// Action for file upload using the pluploader
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="name"></param>
        /// <param name="attachmentType"></param>
        /// <param name="attachmentValue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Upload(int? chunk, string name, string attachmentType, string attachmentValue)
        {
            var fileUpload = Request.Files[0];
            var uploadPath = Server.MapPath("~/app_data/attachments/" + attachmentType + "/" + attachmentValue + "/");

            // create the uploadPath if it does not exist
            if (!Directory.Exists(uploadPath)) { Directory.CreateDirectory(uploadPath); }

            chunk = chunk ?? 0;
            using (var fs = new FileStream(Path.Combine(uploadPath, name), chunk == 0 ? FileMode.Create : FileMode.Append))
            {
                var buffer = new byte[fileUpload.InputStream.Length];
                fileUpload.InputStream.Read(buffer, 0, buffer.Length);
                fs.Write(buffer, 0, buffer.Length);
            }
            return Content("chunk uploaded", "text/plain");
        }


    }
}
