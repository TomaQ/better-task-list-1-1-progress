using System.IO;
using System.Net;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace BetterTaskList.Helpers
{
    public class FileManagerHelpers
    {
        public static int AttachmentCount(string attachmentType, string attachmentValue)
        {
            string attachmentsDir = HttpContext.Current.Server.MapPath("~/App_Data/Attachments/" + attachmentType + "/" + attachmentValue + "/");

            int attachmentCount = 0;  // default value

            if (Directory.Exists(attachmentsDir))
            {
                attachmentCount = (from f in System.IO.Directory.GetFiles(attachmentsDir, "*.*", SearchOption.TopDirectoryOnly) select System.IO.Path.GetFileName(f)).Count();
            }

            return attachmentCount;
        }

        public static bool HasAttachmentsAvailable(string attachmentType, string attachmentValue)
        {
            string attachmentsDir = HttpContext.Current.Server.MapPath("~/App_Data/Attachments/" + attachmentType + "/" + attachmentValue + "/");

            int attachmentCount = 0;  // temp default value

            if (Directory.Exists(attachmentsDir))
            {
                attachmentCount = (from f in System.IO.Directory.GetFiles(attachmentsDir, "*.*", SearchOption.TopDirectoryOnly) select System.IO.Path.GetFileName(f)).Count();
            }
            // if the count of attachmentCount is greater then 0 then return true else false
            return attachmentCount > 0 ? true : false;            
        }


        public static List<string> AttachmentList(string attachmentType, string attachmentValue)
        {
            string attachmentsDir = HttpContext.Current.Server.MapPath("~/App_Data/Attachments/" + attachmentType + "/" + attachmentValue + "/");

            IEnumerable<string> fileList = new List<string>();

            if (Directory.Exists(attachmentsDir))
            {
                fileList = from f in System.IO.Directory.GetFiles(attachmentsDir, "*.*", SearchOption.TopDirectoryOnly) select System.IO.Path.GetFileName(f);
            }

            return fileList.ToList();

        }

        /// <summary>
        /// Key = File name Value = File extension
        /// </summary>
        /// <param name="attachmentType"></param>
        /// <param name="attachmentValue"></param>
        /// <returns></returns>
        public static Dictionary<string, string> AttachmentListDictionary(string attachmentType, string attachmentValue)
        {
            // compose the directory path we will be working with
            string attachmentsDir = HttpContext.Current.Server.MapPath("~/App_Data/Attachments/" + attachmentType + "/" + attachmentValue + "/");

            // initialize 
            IEnumerable<string> fileList = new List<string>();
            Dictionary<string, string> fileDictionary = new Dictionary<string, string>();


            // make sure the directory exist
            if (Directory.Exists(attachmentsDir))
            {
                // compose a list of files that exist inside the directory we are working with
                fileList = from f in System.IO.Directory.GetFiles(attachmentsDir, "*.*", SearchOption.TopDirectoryOnly) select System.IO.Path.GetFileName(f);

                // iterate thru the file list and grab the extension of each file
                foreach (var file in fileList)
                {
                    string fileExtension = Path.GetExtension(attachmentsDir + file);
                    fileDictionary.Add(file, fileExtension);
                }
            }

            return fileDictionary;
        }

    }

    public class BinaryContentResult : ActionResult
    {



        public BinaryContentResult()
        {
        }

        public string ContentType { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {

            context.HttpContext.Response.ClearContent();
            context.HttpContext.Response.ContentType = ContentType;

            context.HttpContext.Response.AddHeader("content-disposition",

            "attachment; filename=" + FileName);

            context.HttpContext.Response.BinaryWrite(Content);
            context.HttpContext.Response.End();
        }


    }

}
