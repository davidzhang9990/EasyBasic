using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using XDDEasy.Domain.ProfileAggregates;
using Common.Helper;
using log4net;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace XDDEasy.Domain.EmailAggregates
{
    public interface IEmailSendService
    {
        Task Send(string emailFrom, string emailTo, string emailSubject, string emailContent);
    }

    public class SendGridEmailService : IEmailSendService
    {

        private SendGridAPIClient _sendClient;
        private readonly ILog _log;

        public SendGridEmailService(ILog log)
        {
            _log = log;
        }

        private SendGridAPIClient SendClient
        {
            get
            {
                if (_sendClient == null)
                {
                    var apiKey = WebConfigurationManager.GetValue("SendGridApiKey");
                    _sendClient = new SendGridAPIClient(apiKey, "https://api.sendgrid.com");
                }
                return _sendClient;
            }
        }

        public async Task Send(string emailFrom, string emailTo, string emailSubject, string emailContent)
        {
            var from = new SendGrid.Helpers.Mail.Email(emailFrom);
            var to = new SendGrid.Helpers.Mail.Email(emailTo);
            var content = new Content("text/html", emailContent);
            var mail = new Mail(from, emailSubject, to, content);
            var response = await SendClient.client.mail.send.post(requestBody: mail.Get());
            _log.Debug(response.StatusCode);
            _log.Debug(response.Body.ReadAsStringAsync().Result);
            _log.Debug(response.Headers.ToString());
        }
    }

    public class SmtpMailService : IEmailSendService
    {
        private SmtpClient _sendClient;
        private readonly IProfileService _profileService;
        private readonly ILog _log;

        public SmtpMailService(IProfileService profileService, ILog log)
        {
            _profileService = profileService;
            _log = log;
        }


        private SmtpClient SendClient
        {
            get
            {
                if (_sendClient == null)
                {
                    dynamic emailConfig = null;
                    var sProfile = _profileService.GetProfile("EmailServerConfig");
                    if (sProfile != null)
                        emailConfig = Json.Decode(HttpUtility.HtmlDecode(sProfile.Value));
                    _sendClient = new SmtpClient { DeliveryMethod = SmtpDeliveryMethod.Network };
                    if (emailConfig == null) return _sendClient;
                    _sendClient.Host = emailConfig.ServiceAddress;
                    if (string.IsNullOrEmpty(emailConfig.ServicePort))
                    {
                        _sendClient.Port = Convert.ToInt32(emailConfig.ServicePort);
                    }
                    _sendClient.Credentials = new System.Net.NetworkCredential(emailConfig.AccountId, emailConfig.AccountToken);
                }
                return _sendClient;
            }
        }

        public async Task Send(string emailFrom, string emailTo, string emailSubject, string emailContent)
        {
            var mailMessage = new MailMessage(emailFrom, emailTo, emailSubject, emailContent)
            {
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true,
                Priority = MailPriority.Low
            };
            SendClient.SendCompleted += (s, e) =>
            {
                if (e.Error != null)
                {
                    _log.Error("Sent mail failed with error: " + e.UserState.ToString());
                }
                else
                {
                    _log.Debug("Sent mail successfully!");
                }
                SendClient.Dispose();
                mailMessage.Dispose();
            };
            SendClient.Send(mailMessage);
        }
    }
}
