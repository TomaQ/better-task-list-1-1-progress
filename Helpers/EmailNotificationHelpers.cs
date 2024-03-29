﻿using System;
using System.Collections;
using System.IO;
using System.Web;
using System.Net;
using System.Text;
using System.Net.Mail;
using System.Configuration;
using BetterTaskList.Models;
using System.Collections.Generic;
using Stream = BetterTaskList.Models.Stream;


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

        private string GetCustomApplicationUrl(bool includeHttp, bool includeUrlAuthority, bool includeAppPath, string appendString)
        {
            StringBuilder customUrl = new StringBuilder();

            if (includeHttp)
                customUrl.Append("http://");

            if (includeUrlAuthority)
                customUrl.Append(HttpContext.Current.Request.Url.Authority);

            if (includeAppPath)
                customUrl.Append(HttpContext.Current.Request.ApplicationPath);

            customUrl.Append(appendString);

            return customUrl.ToString();
        }

        private void SendEmail(MailMessage message)
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
        // Application Errors notifications
        //**********************************************************

        public void ErrorNotification(string error)
        {

            string emailMsg = error;

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("notification@yovsolutions.com");
            mailMessage.To.Add("geovanimartinez@yovasolutions.com");
            mailMessage.Subject = "BetterTaskList - Runtime Error";
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.Body = emailMsg;
            SendEmail(mailMessage);

        }

        //**********************************************************
        // Passwords
        //**********************************************************

        public void ForgotMyPasswordEmail(string userEmailAddress, string password)
        {
            try
            {

                string applicationUrl = GetCustomApplicationUrl(true, true, true, "");

                string emailMsg = ReadTemplateFile("~/Content/Templates/PasswordReset.htm");
                emailMsg = emailMsg.Replace("{NewPassword}", password);
                emailMsg = emailMsg.Replace("{ApplicationUrl}", applicationUrl);

                MailMessage message = new MailMessage();
                message.From = new MailAddress(NotificationEmailAddressFrom, "Make progress every day");
                message.To.Add(userEmailAddress);
                message.Subject = "Notification: Password Recovery";
                message.Body = emailMsg;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;

                SendEmail(message);

            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        //**********************************************************
        // Add coworker confirmation
        //**********************************************************

        public void AddCoWorkerEmail(long coWorkerId, string requesterFullName, string coWorkerEmailAddress)
        {
            string customApplicationUrl = GetCustomApplicationUrl(true, true, true, "");

            string emailMsg = ReadTemplateFile("~/Content/Templates/AddCoWorker.htm");
            emailMsg = emailMsg.Replace("{FullName}", requesterFullName);
            emailMsg = emailMsg.Replace("{ApplicationUrl}", customApplicationUrl);
            emailMsg = emailMsg.Replace("{CoWorkerId}", coWorkerId.ToString());

            MailMessage message = new MailMessage();
            message.From = new MailAddress(NotificationEmailAddressFrom, "Make progress every day");

            message.To.Add(coWorkerEmailAddress);

            message.Subject = "Coworker Request";
            message.Body = emailMsg;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            SendEmail(message);
        }

        //**********************************************************
        // User registration email
        //**********************************************************

        public void UserRegistrationEmail(string userEmailAddress)
        {
            try
            {
                // Get the registrant first name
                string registrantFirstName = UserHelpers.GetFirstName(userEmailAddress);

                // Compose email
                string applicationUrl = GetCustomApplicationUrl(true, true, true, "");

                // Update the template file with the proper values
                string emailMsg = ReadTemplateFile("~/Content/Templates/UserRegistration.htm");
                emailMsg = emailMsg.Replace("{FirstName}", registrantFirstName);
                emailMsg = emailMsg.Replace("{ApplicationUrl}", applicationUrl);

                
                MailMessage message = new MailMessage();
                message.From = new MailAddress(NotificationEmailAddressFrom, "Make progress everyday");

                message.To.Add(userEmailAddress);

                message.Subject = "Progress registration";
                message.Body = emailMsg;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;

                SendEmail(message);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        //**********************************************************
        // Status wall post
        //**********************************************************

        public void StatusPost(string currentUserUserName, long streamId, string statusMessage)
        {
            // Get the poster first name
            string statusOwnerFirstName = UserHelpers.GetFirstName(currentUserUserName);

            // Get poster friends emails
            string[] friendsEmailAddresses = UserHelpers.GetUserFriendsEmailAdresses(currentUserUserName);

            // Compose email
            string applicationUrl = GetCustomApplicationUrl(true, true, true, "");

            string emailMsg = ReadTemplateFile("~/Content/Templates/StatusPost.htm");
            emailMsg = emailMsg.Replace("{StatusOwnerFirstName}", statusOwnerFirstName);
            emailMsg = emailMsg.Replace("{StreamId}", streamId.ToString());
            emailMsg = emailMsg.Replace("{StatusMessage}", statusMessage);
            emailMsg = emailMsg.Replace("{ApplicationUrl}", applicationUrl);


            MailMessage message = new MailMessage();
            message.From = new MailAddress(NotificationEmailAddressFrom, "Make progress every day");

            foreach (string emailAddress in friendsEmailAddresses) { message.To.Add(emailAddress); }

            message.Subject = statusOwnerFirstName + "- Status Message";
            message.Body = emailMsg;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            SendEmail(message);

        }


        public void StatusPostComment(string currentUserUserName, long streamId, string statusCommentMessage)
        {
            // Below we
            // 1. Notify the stream creator of the comment
            // 2. Notify those that have commented already on the stream (exclude the current comment submitter)

            //Grab the UserId of the stream creator
            var stream = new StreamRepository().GetStream(streamId);

            // Get the stream owner firstname & email address
            string statusOwnerFirstName = UserHelpers.GetFirstName(stream.StreamCreatorUserId);
            string statusOwnerEmailAddress = UserHelpers.GetUserEmailAddress(stream.StreamCreatorUserId);

            // Get a list of users who have commented on this status 
            var commentatorsAdresses = new StreamCommentRepository().GetStatusCommentatorsEmailAddresses(streamId);

            // remove the current user from the list of commentatos since notifiying him would be redundant
            commentatorsAdresses.Remove(currentUserUserName); //FYI: EmailAddress is reused as the username in this app.

            string commenterFirstName = UserHelpers.GetFirstName(currentUserUserName);

            // Compose email
            string applicationUrl = GetCustomApplicationUrl(true, true, true, "");

            string emailMsg = ReadTemplateFile("~/Content/Templates/StatusPostComment.htm");
            emailMsg = emailMsg.Replace("{StatusCommenterFirstName}", commenterFirstName);
            emailMsg = emailMsg.Replace("{StatusMessage}", statusCommentMessage);
            emailMsg = emailMsg.Replace("{StreamId}", streamId.ToString());
            emailMsg = emailMsg.Replace("{ApplicationUrl}", applicationUrl);

            MailMessage message = new MailMessage();
            message.From = new MailAddress(NotificationEmailAddressFrom, "Make progress every day");

            foreach (string emailAddress in commentatorsAdresses) { message.To.Add(emailAddress); }

            message.Subject = commenterFirstName + "- commented on status";
            message.Body = emailMsg;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            SendEmail(message);


        }

        //**********************************************************
        // CoWorker profile wall post
        //**********************************************************

        public void CoWorkerWallPostEmail(string wallWriterUserName, Guid wallOwnerUserId, long streamId, string wallMessage)
        {
            string customApplicationUrl = GetCustomApplicationUrl(true, true, true, "");

            string wallOwnerFirstName = UserHelpers.GetFirstName(wallOwnerUserId);
            string wallWriterFirstName = UserHelpers.GetFirstName(wallWriterUserName);
            string wallOwnerEmailAddress = UserHelpers.GetUserEmailAddress(wallOwnerUserId);

            string emailMsg = ReadTemplateFile("~/Content/Templates/WallPost.htm");
            emailMsg = emailMsg.Replace("{WallOwnerFirstName}", wallOwnerFirstName);
            emailMsg = emailMsg.Replace("{WallWriterFirstName}", wallWriterFirstName);
            emailMsg = emailMsg.Replace("{WallMessage}", wallMessage);
            emailMsg = emailMsg.Replace("{StreamId}", streamId.ToString());

            emailMsg = emailMsg.Replace("{ApplicationUrl}", customApplicationUrl);
            //emailMsg = emailMsg.Replace("{CoWorkerId}", coWorkerId.ToString());

            MailMessage message = new MailMessage();
            message.From = new MailAddress(NotificationEmailAddressFrom, "Make progress every day");

            message.To.Add(wallOwnerEmailAddress);

            message.Subject = "Wallpost notification";
            message.Body = emailMsg;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            SendEmail(message);

        }


        //**********************************************************
        // Ticket notifications
        //**********************************************************

        public void NewTicketEmail(Ticket ticket)
        {
            string applicationUrl = GetCustomApplicationUrl(true, true, true, "");
            string ticketUrl = GetCustomApplicationUrl(true, true, true, "/Projects/Ticket/Details/" + ticket.TicketId);

            string emailMsg = ReadTemplateFile("~/Content/Templates/NewTicket.htm");
            emailMsg = emailMsg.Replace("{TicketId}", ticket.TicketId.ToString());
            emailMsg = emailMsg.Replace("{TicketSubject}", ticket.TicketSubject);
            emailMsg = emailMsg.Replace("{TicketDescription}", ticket.TicketDescription);
            emailMsg = emailMsg.Replace("{FullName}", UserHelpers.GetUserFullName(ticket.TicketCreatorUserId));
            emailMsg = emailMsg.Replace("{ApplicationUrl}", applicationUrl);
            emailMsg = emailMsg.Replace("{TicketUrl}", ticketUrl);



            MailMessage message = new MailMessage();
            message.From = new MailAddress(NotificationEmailAddressFrom, "Make progress every day");

            if (!string.IsNullOrEmpty(ticket.TicketOwnersEmailList))
            {
                // parse the list of email addresses in the TO field
                string[] toEmailAddresses = ticket.TicketOwnersEmailList.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string emailAddress in toEmailAddresses) { message.To.Add(emailAddress); }
            }

            if(!string.IsNullOrEmpty(ticket.TicketEmailNotificationList)) 
            {
            // parse the list of email address in the CC field
            string[] ccEmailAddresses = ticket.TicketEmailNotificationList.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string ccEmailAddress in ccEmailAddresses) { message.CC.Add(ccEmailAddress); }
            }

            message.Subject = "#" + ticket.TicketId + " - " + ticket.TicketSubject;
            message.Body = emailMsg;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            SendEmail(message);
        }

        public void TicketCommentEmail(Ticket ticket, TicketComment ticketComment)
        {
            string applicationUrl = GetCustomApplicationUrl(true, true, true, "");
            string ticketUrl = GetCustomApplicationUrl(true, true, true, "/Projects/Ticket/Details/" + ticket.TicketId + "?cid=" + ticketComment.TicketCommentId + "#" + ticketComment.TicketCommentId);

            string emailMsg = ReadTemplateFile("~/Content/Templates/TicketComment.htm");
            emailMsg = emailMsg.Replace("{TicketCommentDetails}", ticketComment.TicketCommentDetails);
            emailMsg = emailMsg.Replace("{FirstName}", UserHelpers.GetFirstName(ticketComment.TicketCommentSubmitterUserId));
            emailMsg = emailMsg.Replace("{TicketSubject}", ticket.TicketSubject);
            emailMsg = emailMsg.Replace("{TicketId}", ticket.TicketId.ToString());
            emailMsg = emailMsg.Replace("{ApplicationUrl}", applicationUrl);
            emailMsg = emailMsg.Replace("{TicketUrl}", ticketUrl);


            MailMessage message = new MailMessage();
            message.From = new MailAddress(NotificationEmailAddressFrom, "Make progress every day");

            // obtain the ticket creator email address
            string ticketCreatorEmailAddress = UserHelpers.GetUserEmailAddress(ticket.TicketCreatorUserId);
            message.To.Add(ticketCreatorEmailAddress);

            // parse the TicketOwnersEmailList and add them to the email message TO field
            if (!string.IsNullOrEmpty(ticket.TicketOwnersEmailList))
            {
                string[] toEmailAddresses = ticket.TicketOwnersEmailList.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string toEmail in toEmailAddresses) { message.To.Add(toEmail); }
            }

            // parse the TicketEmailNotificationList and add any record to the CC field of the email
            if (!string.IsNullOrEmpty(ticket.TicketEmailNotificationList))
            {
                string[] ccEmailAddresses = ticket.TicketEmailNotificationList.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string ccEmail in ccEmailAddresses) { message.CC.Add(ccEmail); }
            }

            message.Subject = "#" + ticket.TicketId + " - " + ticket.TicketSubject;
            message.Body = emailMsg;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            SendEmail(message);

        }

        public void TicketCommentReplyEmail(Ticket ticket, TicketComment ticketCommentReply)
        {
            string applicationUrl = GetCustomApplicationUrl(true, true, true, "");
            string ticketUrl = GetCustomApplicationUrl(true, true, true, "/Projects/Ticket/Details/" + ticket.TicketId + ticket.TicketId + "?cid=" + ticketCommentReply.TicketCommentId + "#" + ticketCommentReply.TicketCommentId);

            string emailMsg = ReadTemplateFile("~/Content/Templates/TicketComment.htm");
            emailMsg = emailMsg.Replace("{TicketSubject}", ticket.TicketSubject);
            emailMsg = emailMsg.Replace("{TicketId}", ticket.TicketId.ToString());
            emailMsg = emailMsg.Replace("{TicketCommentDetails}", ticketCommentReply.TicketCommentDetails);
            emailMsg = emailMsg.Replace("{FirstName}", UserHelpers.GetFirstName(ticketCommentReply.TicketCommentSubmitterUserId));
            emailMsg = emailMsg.Replace("{ApplicationUrl}", applicationUrl);
            emailMsg = emailMsg.Replace("{TicketUrl}", ticketUrl);


            MailMessage message = new MailMessage();
            message.From = new MailAddress(NotificationEmailAddressFrom, "Make progress every day");

            // obtain the ticket creator email address
            string ticketCreatorEmailAddress = UserHelpers.GetUserEmailAddress(ticket.TicketCreatorUserId);
            message.To.Add(ticketCreatorEmailAddress);

            // parse the TicketOwnersEmailList and add them to the email message TO field
            if (!string.IsNullOrEmpty(ticket.TicketOwnersEmailList))
            {
                string[] toEmailAddresses = ticket.TicketOwnersEmailList.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string toEmail in toEmailAddresses) { message.To.Add(toEmail); }
            }

            // parse the TicketEmailNotificationList and add any record to the CC field of the email
            if (!string.IsNullOrEmpty(ticket.TicketEmailNotificationList))
            {
                string[] ccEmailAddresses = ticket.TicketEmailNotificationList.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string ccEmail in ccEmailAddresses) { message.CC.Add(ccEmail); }
            }

            message.Subject = "#" + ticket.TicketId + " - " + ticket.TicketSubject;
            message.Body = emailMsg;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            SendEmail(message);

        }

        public void TicketResolvedEmail(Ticket ticket)
        {
            string applicationUrl = GetCustomApplicationUrl(true, true, true, "");
            string ticketUrl = GetCustomApplicationUrl(true, true, true, "/Projects/Ticket/Details/" + ticket.TicketId);

            string emailMsg = ReadTemplateFile("~/Content/Templates/TicketResolved.htm");
            emailMsg = emailMsg.Replace("{ApplicationUrl}", applicationUrl);
            emailMsg = emailMsg.Replace("{TicketId}", ticket.TicketId.ToString());
            emailMsg = emailMsg.Replace("{TicketSubject}", ticket.TicketSubject);

            emailMsg = emailMsg.Replace("{TicketCreatorFullName}", UserHelpers.GetFirstName(ticket.TicketCreatorUserId));
            emailMsg = emailMsg.Replace("{TicketResolverFullName}", UserHelpers.GetFirstName(ticket.TicketResolvedByUserId.Value));

            emailMsg = emailMsg.Replace("{TicketDescription}", ticket.TicketDescription);
            emailMsg = emailMsg.Replace("{TicketResolution}", ticket.TicketResolutionDetails);
            emailMsg = emailMsg.Replace("{TicketUrl}", ticketUrl);

            MailMessage message = new MailMessage();
            message.From = new MailAddress(NotificationEmailAddressFrom, "Make progress every day");

            // add the ticket creator to the email TO field values
            string ticketCreatorEmail = UserHelpers.GetEmailAddress(ticket.TicketCreatorUserId);
            message.To.Add(ticketCreatorEmail);

            // parse the TicketOwnersEmailList and add them to the email message TO field
            if (!string.IsNullOrEmpty(ticket.TicketOwnersEmailList))
            {
                string[] toEmailAddresses = ticket.TicketOwnersEmailList.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string toEmail in toEmailAddresses) { message.To.Add(toEmail); }
            }

            // parse the TicketEmailNotificationList and add any record to the CC field of the email
            if (!string.IsNullOrEmpty(ticket.TicketEmailNotificationList))
            {
                string[] ccEmailAddresses = ticket.TicketEmailNotificationList.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string ccEmail in ccEmailAddresses) { message.CC.Add(ccEmail); }
            }

            message.Subject = "Ticket #" + ticket.TicketId + " - " + ticket.TicketSubject;
            message.Body = emailMsg;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            SendEmail(message);
        }

    }
}

