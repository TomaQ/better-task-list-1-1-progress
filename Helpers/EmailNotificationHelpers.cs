using System;
using System.IO;
using System.Web;
using System.Text;
using System.Net.Mail;
using System.Configuration;
using BetterTaskList.Models;


namespace BetterTaskList.Helpers
{
    public class EmailNotificationHelpers
    {
        private static string NotificationEmailAddressFrom = ConfigurationManager.AppSettings["NotificationsFromEmailAddress"];
      
        #region "Old Code"

        //public static void ComposeTaskCreationEmail(int taskId)
        //{
        //    try
        //    {

        //        TaskRepository taskRepository = new TaskRepository();
        //        Task taskInfo = taskRepository.GetTask(taskId);

        //        Profile taskCreatorProfile = UserHelpers.GetUserProfile(taskInfo.TaskCreatorUserId);
        //        Profile taskAssignedToProfile = UserHelpers.GetUserProfile(taskInfo.TaskAssignedToUserId);

        //        string taskAssignedToSMSAddress = string.Empty;
        //        bool notifyTaskAssignedToViaSMS = taskAssignedToProfile.SmsOnTaskCreate;

        //        if(notifyTaskAssignedToViaSMS)
        //        {
        //            taskAssignedToSMSAddress = taskAssignedToProfile.CellPhoneNumber.Replace("-", "") + taskAssignedToProfile.CellPhoneCarrierDomainName;
        //        }

        //        string templateFilePath = HttpContext.Current.Server.MapPath("~/Content/NewTaskHtmlTemplate.html");

        //        StreamReader sr = new StreamReader(templateFilePath);
        //        string emailMsg = sr.ReadToEnd();
        //        sr.Close();

        //        emailMsg = emailMsg.Replace("{taskAssignedToFullName}", taskAssignedToProfile.FullName);
        //        emailMsg = emailMsg.Replace("{taskCreatorFullName}", taskCreatorProfile.FullName);
        //        emailMsg = emailMsg.Replace("{taskName}", taskInfo.TaskName);
        //        emailMsg = emailMsg.Replace("{taskDescription}", taskInfo.TaskDescription);
        //        emailMsg = emailMsg.Replace("{taskId}", taskInfo.TaskId.ToString());
        //        emailMsg = emailMsg.Replace("{ApplicationHostName}", ApplicationHostName);

        //        MailMessage message = new MailMessage();
        //        message.From = new MailAddress(NotificationEmailAddressFrom, "Better Task List Notifications");

        //        message.To.Add(taskInfo.TaskAssignedToEmailAddress);
        //        if(notifyTaskAssignedToViaSMS)
        //        {
        //            message.To.Add(taskAssignedToSMSAddress);
        //        }

        //        message.Subject = "New Task # " + taskInfo.TaskId + " :" + taskInfo.TaskName;
        //        message.Body = emailMsg;
        //        message.BodyEncoding = Encoding.ASCII;
        //        message.IsBodyHtml = true;

        //        SendEmail(message);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        //public static void ComposeTaskCreationEmailForNotificationList(int taskId)
        //{
        //    try
        //    {

        //        TaskRepository taskRepository = new TaskRepository();
        //        Task taskInfo = taskRepository.GetTask(taskId);

        //        Profile taskCreatorProfile = UserHelpers.GetUserProfile(taskInfo.TaskCreatorUserId);
        //        Profile taskAssignedToProfile = UserHelpers.GetUserProfile(taskInfo.TaskAssignedToUserId);

        //        string templateFilePath = HttpContext.Current.Server.MapPath("~/Content/NewTaskNotificationListHtmlTemplate.html");

        //        StreamReader sr = new StreamReader(templateFilePath);
        //        string emailMsg = sr.ReadToEnd();
        //        sr.Close();

        //        emailMsg = emailMsg.Replace("{taskAssignedToFullName}", taskAssignedToProfile.FullName);
        //        emailMsg = emailMsg.Replace("{taskCreatorFullName}", taskCreatorProfile.FullName);
        //        emailMsg = emailMsg.Replace("{taskName}", taskInfo.TaskName);
        //        emailMsg = emailMsg.Replace("{taskDescription}", taskInfo.TaskDescription);
        //        emailMsg = emailMsg.Replace("{taskId}", taskInfo.TaskId.ToString());
        //        emailMsg = emailMsg.Replace("{ApplicationHostName}", ApplicationHostName);

        //        MailMessage message = new MailMessage();
        //        message.From = new MailAddress(NotificationEmailAddressFrom, "Better Task List Notifications");
        //        string[] multipleEmailRecepients = taskInfo.TaskEmailNotificationList.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        //        foreach (string emailAddress in multipleEmailRecepients) { message.To.Add(emailAddress); }
        //        message.Subject = "Notification: " + taskInfo.TaskName;
        //        message.Body = emailMsg;
        //        message.BodyEncoding = Encoding.ASCII;
        //        message.IsBodyHtml = true;

        //        SendEmail(message);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static void ComposeTaskResolutionEmail(int taskId)
        //{
        //    try
        //    {

        //        TaskRepository taskRepository = new TaskRepository();
        //        Task taskInfo = taskRepository.GetTask(taskId);

        //        Profile taskCreatorProfile = UserHelpers.GetUserProfile(taskInfo.TaskCreatorUserId);
        //        Profile taskAssignedToProfile = UserHelpers.GetUserProfile(taskInfo.TaskAssignedToUserId);

        //        string templateFilePath = HttpContext.Current.Server.MapPath("~/Content/TaskResolvedHtmlTemplate.html");

        //        StreamReader sr = new StreamReader(templateFilePath);
        //        string emailMsg = sr.ReadToEnd();
        //        sr.Close();

        //        emailMsg = emailMsg.Replace("{taskAssignedToFullName}", taskAssignedToProfile.FullName);
        //        emailMsg = emailMsg.Replace("{taskName}", taskInfo.TaskName);
        //        emailMsg = emailMsg.Replace("{taskDescription}", taskInfo.TaskDescription);
        //        emailMsg = emailMsg.Replace("{taskId}", taskInfo.TaskId.ToString());
        //        emailMsg = emailMsg.Replace("{taskResolutionDetails}", taskInfo.TaskResolutionDetails);
        //        emailMsg = emailMsg.Replace("{ApplicationHostName}", ApplicationHostName);

        //        MailMessage message = new MailMessage();
        //        message.From = new MailAddress(NotificationEmailAddressFrom, "Better Task List Notifications");
        //        message.To.Add(taskInfo.TaskCreatorEmailAddress);
        //        message.Subject = "Notification: " + taskInfo.TaskName;
        //        message.Body = emailMsg;
        //        message.BodyEncoding = Encoding.ASCII;
        //        message.IsBodyHtml = true;

        //        SendEmail(message);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static void ComposeTaskResolutionEmailForNotificationList(int taskId)
        //{
        //    try
        //    {
        //        TaskRepository taskRepository = new TaskRepository();
        //        Task taskInfo = taskRepository.GetTask(taskId);

        //        Profile taskAssignedToProfile = UserHelpers.GetUserProfile(taskInfo.TaskAssignedToUserId);

        //        string templateFilePath = HttpContext.Current.Server.MapPath("~/Content/TaskResolvedNotificationListHtmlTemplate.html");

        //        StreamReader sr = new StreamReader(templateFilePath);
        //        string emailMsg = sr.ReadToEnd();
        //        sr.Close();

        //        emailMsg = emailMsg.Replace("{taskAssignedToFullName}", taskAssignedToProfile.FullName);
        //        emailMsg = emailMsg.Replace("{taskName}", taskInfo.TaskName);
        //        emailMsg = emailMsg.Replace("{taskDescription}", taskInfo.TaskDescription);
        //        emailMsg = emailMsg.Replace("{taskId}", taskInfo.TaskId.ToString());
        //        emailMsg = emailMsg.Replace("{taskResolutionDetails}", taskInfo.TaskResolutionDetails);
        //        emailMsg = emailMsg.Replace("{ApplicationHostName}", ApplicationHostName);

        //        MailMessage message = new MailMessage();
        //        message.From = new MailAddress(NotificationEmailAddressFrom, "Better Task List Notifications");
        //        string[] multipleEmailRecepients = taskInfo.TaskEmailNotificationList.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        //        foreach (string emailAddress in multipleEmailRecepients) { message.To.Add(emailAddress);}
        //        message.Subject = "Notification: " + taskInfo.TaskName;
        //        message.Body = emailMsg;
        //        message.BodyEncoding = Encoding.ASCII;
        //        message.IsBodyHtml = true;

        //        SendEmail(message);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static void ComposeCommentCreationEmail(int taskId, int taskCommentId)
        //{
        //    try
        //    {
        //        TaskRepository taskRepository = new TaskRepository();
        //        Task taskInfo = taskRepository.GetTask(taskId);
        //        Profile taskAssignedToProfile = UserHelpers.GetUserProfile(taskInfo.TaskAssignedToUserId);

        //        TaskCommentRepository taskCommentRepository = new TaskCommentRepository();
        //        TaskComment commentInfo = taskCommentRepository.GetComment(taskCommentId);
        //        Profile taskCommentSubmitterProfile = UserHelpers.GetUserProfile(commentInfo.TaskCommentSubmitterUserId);

        //        string templateFilePath = HttpContext.Current.Server.MapPath("~/Content/NewCommentHtmlTemplate.html");

        //        StreamReader sr = new StreamReader(templateFilePath);
        //        string emailMsg = sr.ReadToEnd();
        //        sr.Close();

        //        emailMsg = emailMsg.Replace("{taskName}", taskInfo.TaskName);
        //        emailMsg = emailMsg.Replace("{taskCommentTimeStamp}", string.Format("{0:f}", commentInfo.TaskCommentTimeStamp));
        //        emailMsg = emailMsg.Replace("{taskCommentSubmitterFullName}", taskCommentSubmitterProfile.FullName);
        //        emailMsg = emailMsg.Replace("{taskCommentDetails}", commentInfo.TaskCommentDetails);
        //        emailMsg = emailMsg.Replace("{taskId}", taskInfo.TaskId.ToString());
        //        emailMsg = emailMsg.Replace("{ApplicationHostName}", ApplicationHostName);

        //        MailMessage message = new MailMessage();
        //        message.From = new MailAddress(NotificationEmailAddressFrom, "Better Task List Notifications");
        //        message.To.Add(taskInfo.TaskCreatorEmailAddress);
        //        message.Subject = "Notification: " + taskInfo.TaskName;
        //        message.Body = emailMsg;
        //        message.BodyEncoding = Encoding.ASCII;
        //        message.IsBodyHtml = true;

        //        SendEmail(message);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static void ComposeCommentCreationEmailForNotificationList(int taskId, int taskCommentId)
        //{
        //    try
        //    {
        //        TaskRepository taskRepository = new TaskRepository();
        //        Task taskInfo = taskRepository.GetTask(taskId);

        //        // Geovani Martinez: I put the if statement below in case this procedure was called blindly (without checking the value of TaskEmailNotificationList)
        //        // This situation occurs in the POST: PostComment action, I found it overkill to create an instance of the Task class just to check the value of 
        //        // Task.TaskEmailNotification so I just put the if statement below, so the program wont crash in the event there are no values defined in TaskEmailNotificationList
        //        if (!string.IsNullOrEmpty(taskInfo.TaskEmailNotificationList))
        //        {
        //            Profile taskAssignedToProfile = UserHelpers.GetUserProfile(taskInfo.TaskAssignedToUserId);

        //            TaskCommentRepository taskCommentRepository = new TaskCommentRepository();
        //            TaskComment commentInfo = taskCommentRepository.GetComment(taskCommentId);
        //            Profile taskCommentSubmitterProfile = UserHelpers.GetUserProfile(commentInfo.TaskCommentSubmitterUserId);

        //            string templateFilePath = HttpContext.Current.Server.MapPath("~/Content/NewCommentNotificationListHtmlTemplate.html");

        //            StreamReader sr = new StreamReader(templateFilePath);
        //            string emailMsg = sr.ReadToEnd();
        //            sr.Close();

        //            emailMsg = emailMsg.Replace("{taskAssignedToFullName}", taskAssignedToProfile.FullName);
        //            emailMsg = emailMsg.Replace("{taskName}", taskInfo.TaskName);
        //            emailMsg = emailMsg.Replace("{taskCommentTimeStamp}", string.Format("{0:f}", commentInfo.TaskCommentTimeStamp));
        //            emailMsg = emailMsg.Replace("{taskCommentSubmitterFullName}", taskCommentSubmitterProfile.FullName);
        //            emailMsg = emailMsg.Replace("{taskCommentDetails}", commentInfo.TaskCommentDetails);
        //            emailMsg = emailMsg.Replace("{taskId}", taskInfo.TaskId.ToString());
        //            emailMsg = emailMsg.Replace("{ApplicationHostName}", ApplicationHostName);

        //            MailMessage message = new MailMessage();
        //            message.From = new MailAddress(NotificationEmailAddressFrom, "Better Task List Notifications");
        //            string[] multipleEmailRecepients = taskInfo.TaskEmailNotificationList.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        //            foreach (string emailAddress in multipleEmailRecepients) { message.To.Add(emailAddress); }
        //            message.Subject = "Notification: " + taskInfo.TaskName;
        //            message.Body = emailMsg;
        //            message.BodyEncoding = Encoding.ASCII;
        //            message.IsBodyHtml = true;

        //            SendEmail(message);

        //        } // if (!string.IsNullOrEmpty(taskinfo.TaskEmailNotificationList))

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #endregion

        public static void ForgotMyPasswordEmail(string userEmailAddress, string password)
        {
            try
            {
                string emailMsg = ReadTemplateFile("~/Content/Templates/PasswordReset.htm");
                emailMsg = emailMsg.Replace("{NewPassword}", password);

                MailMessage message = new MailMessage();
                message.From = new MailAddress(NotificationEmailAddressFrom, "Progress Notifications");
                message.To.Add(userEmailAddress);
                message.Subject = "Notification: Password Recovery";
                message.Body = emailMsg;
                message.BodyEncoding = Encoding.ASCII;
                message.IsBodyHtml = true;

                SendEmail(message);

            }
            catch (Exception exception)
            {
                throw exception;  
            }
        }

        private static string ReadTemplateFile(string templatePath)
        {
            string templateFilePath = HttpContext.Current.Server.MapPath(templatePath);

            StreamReader sr = new StreamReader(templateFilePath);
            string templateHtml = sr.ReadToEnd();
            sr.Close();

            return templateHtml;
        }

        public static void SendEmail(MailMessage message)
        {
            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.Send(message);
            }
            catch (SmtpException exception)
            {
                throw exception;
            }
        }

    }
}

