using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Net;
using System.Text;
using System.Net.Mail;
using System.Configuration;
using BetterTaskList.Models;
using System.Collections.Generic;


namespace BetterTaskList.Helpers
{
    public class EmailNotificationHelpers
    {
        private string NotificationEmailAddressFrom = ConfigurationManager.AppSettings["NotificationsFromEmailAddress"];

        private string ReadTemplateFile(string templatePath)
        {
            string templateFilePath = HttpContext.Current.Server.MapPath(templatePath);

            StreamReader sr = new StreamReader(templateFilePath);
            string templateHtml = sr.ReadToEnd();
            sr.Close();

            return templateHtml;
        }

        private string GetApplicationUrl(string s)
        {
            Uri result = new Uri(HttpContext.Current.Request.Url, HttpContext.Current.Request.ApplicationPath + s);
            return result.ToString();
        }

        public void SendEmail(MailMessage message)
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

        //**********************************************************
        // Passwords
        //**********************************************************

        public void ForgotMyPasswordEmail(string userEmailAddress, string password)
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

        //**********************************************************
        // Ticket notifications
        //**********************************************************

        public void NewTicketEmail(Ticket ticket)
        {

            string ticketUrl = GetApplicationUrl("/Tickets/Ticket/Details/" + ticket.TicketId);

            string emailMsg = ReadTemplateFile("~/Content/Templates/NewTicket.htm");
            emailMsg = emailMsg.Replace("{TicketId}", ticket.TicketId.ToString());
            emailMsg = emailMsg.Replace("{TicketSubject}", ticket.TicketSubject);
            emailMsg = emailMsg.Replace("{TicketDescription}", ticket.TicketDescription);
            emailMsg = emailMsg.Replace("{FullName}", UserHelpers.GetUserFullName(ticket.TicketCreatorUserId));
            emailMsg = emailMsg.Replace("{TicketUrl}", ticketUrl);


            MailMessage message = new MailMessage();
            message.From = new MailAddress(NotificationEmailAddressFrom, "Progress Notifications");

            string[] multipleEmailRecepients = ticket.TicketOwnersEmailList.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string emailAddress in multipleEmailRecepients) { message.To.Add(emailAddress); }

            message.Subject = "#" + ticket.TicketId + " - " + ticket.TicketSubject;
            message.Body = emailMsg;
            message.BodyEncoding = Encoding.ASCII;
            message.IsBodyHtml = true;

            SendEmail(message);
        }

        public void TicketCommentEmail(Ticket ticket, TicketComment ticketComment)
        {

            string ticketUrl = GetApplicationUrl("/Tickets/Ticket/Details/" + ticket.TicketId);

            string emailMsg = ReadTemplateFile("~/Content/Templates/TicketComment.htm");
            emailMsg = emailMsg.Replace("{TicketId}", ticket.TicketId.ToString());
            emailMsg = emailMsg.Replace("{TicketCommentDetails}", ticketComment.TicketCommentDetails);
            emailMsg = emailMsg.Replace("{FullName}", UserHelpers.GetUserFullName(ticketComment.TicketCommentSubmitterUserId));
            emailMsg = emailMsg.Replace("{TicketUrl}", ticketUrl);


            MailMessage message = new MailMessage();
            message.From = new MailAddress(NotificationEmailAddressFrom, "Progress Notifications");

            string[] multipleEmailRecepients = ticket.TicketOwnersEmailList.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string emailAddress in multipleEmailRecepients) { message.To.Add(emailAddress); }

            message.Subject = "#" + ticket.TicketId + " - " + ticket.TicketSubject;
            message.Body = emailMsg;
            message.BodyEncoding = Encoding.ASCII;
            message.IsBodyHtml = true;

            SendEmail(message);

        }

        public void TicketCommentReplyEmail(Ticket ticket, TicketComment ticketCommentReply)
        {
            string ticketUrl = GetApplicationUrl("/Tickets/Ticket/Details/" + ticket.TicketId);

            string emailMsg = ReadTemplateFile("~/Content/Templates/TicketComment.htm");
            emailMsg = emailMsg.Replace("{TicketId}", ticket.TicketId.ToString());
            emailMsg = emailMsg.Replace("{TicketCommentDetails}", ticketCommentReply.TicketCommentDetails);
            emailMsg = emailMsg.Replace("{FullName}", UserHelpers.GetUserFullName(ticketCommentReply.TicketCommentSubmitterUserId));
            emailMsg = emailMsg.Replace("{TicketUrl}", ticketUrl);


            MailMessage message = new MailMessage();
            message.From = new MailAddress(NotificationEmailAddressFrom, "Progress Notifications");

            string[] multipleEmailRecepients = ticket.TicketOwnersEmailList.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string emailAddress in multipleEmailRecepients) { message.To.Add(emailAddress); }

            message.Subject = "#" + ticket.TicketId + " - " + ticket.TicketSubject;
            message.Body = emailMsg;
            message.BodyEncoding = Encoding.ASCII;
            message.IsBodyHtml = true;

            SendEmail(message);

        }


    }
}

