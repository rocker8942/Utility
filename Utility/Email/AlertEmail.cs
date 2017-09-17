using System.Collections.Specialized;
using System.IO;

namespace Utility
{
    /// <summary>
    ///     Send alert emails with the given property
    /// </summary>
    public class AlertEmail
    {
        private AlertEmailProperty emailProperty;

        /// <summary>
        ///     Initiate the class with email properties
        /// </summary>
        /// <param name="emailProperty"></param>
        public AlertEmail(AlertEmailProperty emailProperty)
        {
            this.emailProperty = emailProperty;
        }

        /// <summary>
        ///     Send alert with the given property
        /// </summary>
        public void SendAlertEmail()
        {
            // Create SMTP object
            var smtp = new Email(emailProperty.SMTPhost);

            // Set variables
            var from = emailProperty.EmailFrom;
            var to = new StringCollection();
            foreach (var item in emailProperty.EmailTo)
                to.Add(item);
            var subject = emailProperty.Subject;
            string body;

            // use template, if provided.
            if (!string.IsNullOrEmpty(emailProperty.Template))
            {
                body = File.ReadAllText(emailProperty.Template);
                body = body.Replace(emailProperty.SubjectHolder, emailProperty.Subject);
                if (emailProperty.isHtml)
                    emailProperty.Body = emailProperty.Body.Replace("\n", "<br>");

                body = body.Replace(emailProperty.ContentHolder, emailProperty.Body);
            }
            else
            {
                body = emailProperty.Body;
            }

            smtp.IsBodyHtml = emailProperty.isHtml;

            // Send email
            smtp.SendEmail(to, from, subject, body);
        }

        /// <summary>
        ///     Send alert with the given property and the message overwritten by the parameter
        /// </summary>
        /// <param name="message">Email message to send. This could be a full html content or main message in a html template</param>
        public void SendAlertEmail(string message)
        {
            emailProperty.Body = message;
            SendAlertEmail();
        }
    }
}