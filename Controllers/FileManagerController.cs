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
        /// Ation for file upload
        /// </summary>
        /// <param name="attachmentType"></param>
        /// <param name="attachmentValue"></param>
        /// <returns></returns>
        public ActionResult Upload(string attachmentType, string attachmentValue)
        {
            try
            {
                var uploadDir = Server.MapPath("~/App_Data/Attachments/" + attachmentType + "/" + attachmentValue + "/");

                // if the directory does not exist created (so that we can avoid an error below)
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                foreach (string f in Request.Files.Keys)
                {

                    if (Request.Files[f].ContentLength > 0)
                        Request.Files[f].SaveAs(uploadDir + Path.GetFileName(Request.Files[f].FileName));
                }

                return Content("Success");

            }
            catch (Exception ex)
            {
                new EmailNotificationHelpers().ErrorNotification(ex.StackTrace);
            }

            // we got this far then something is broken
            return Content("Error");
        }

    }
}
