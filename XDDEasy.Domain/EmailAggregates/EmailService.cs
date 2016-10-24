using XDDEasy.Contract;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web.Helpers;
using System.Web;
using System.Globalization;
using System.Configuration;
using System.Text.RegularExpressions;
using XDDEasy.Domain.Identity;
using Common.Helper;
using XDDEasy.Contract.EmailContract;
using System.Collections.Generic;
using System.Web.Http.OData.Query;
using System;
using XDDEasy.Domain.ProfileAggregates;
using Common.Models;
using AutoMapper;
using Common.Models.Enum;

namespace XDDEasy.Domain.EmailAggregates
{
    public interface IEmailService
    {
        IEnumerable<EmailResponse> GetAllEmail();

        IEnumerable<EmailResponse> GetPaging(ODataQueryOptions<Email> options);

        IEnumerable<EmailResponse> GetEmailsByUserId(Guid? userId);

        void Add(Email email);

        EmailResponse CreateRegisterEmail(string username, string password, CreateEmailRequest request);

        EmailResponse CreateResetPassEmail(string username, string link, CreateEmailRequest request);

        EmailResponse CreateConfirmRegisterEmail(string username, string link, CreateEmailRequest request);

        EmailResponse GetEmail(Guid emailId);

    }

    public class EmailService : IEmailService
    {
        //private readonly RequestContext _requestContext;
        private readonly IEmailRepository _emailRepository;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly IProfileService _profileService;

        private readonly Guid _userId;
        private readonly IEmailSendService _emailSendService;

        public EmailService(RequestContext requestContext, IEmailRepository emailRepository, IProfileService profileService, IEmailTemplateService emailTemplateService, IEmailSendService emailSendService)
        {
            _emailRepository = emailRepository;
            _profileService = profileService;
            _emailTemplateService = emailTemplateService;
            _userId = requestContext.UserId;
            _emailSendService = emailSendService;
        }

        public IEnumerable<EmailResponse> GetAllEmail()
        {
            var allEmails = _emailRepository.GetAll();
            return Mapper.Map<IEnumerable<EmailResponse>>(allEmails);
        }

        public IEnumerable<EmailResponse> GetPaging(ODataQueryOptions<Email> options)
        {
            var emails = _emailRepository.ODataQuery(options);
            return Mapper.Map<IEnumerable<Email>, IEnumerable<EmailResponse>>(emails);
        }

        public IEnumerable<EmailResponse> GetEmailsByUserId(Guid? userId)
        {
            var decks = _emailRepository.GetEmailsByUser(userId);
            return Mapper.Map<IEnumerable<EmailResponse>>(decks);
        }

        public EmailResponse CreateRegisterEmail(string username, string password, CreateEmailRequest request)
        {
            Email email = Mapper.Map<CreateEmailRequest, Email>(request);
            email.SenderId = _userId;

            dynamic emailService = null;
            dynamic emailTemp = null;
            
            var sProfile = _profileService.GetProfile("EmailServerConfig");
            if (sProfile != null)
                emailService = Json.Decode(HttpUtility.HtmlDecode(sProfile.Value));
            
            var sProfileTemp = _emailTemplateService.GetEmailTemplate("EmailRegistrationTemplate", LangHelper.GetLanguage());
            if (sProfileTemp != null)
                emailTemp = Json.Decode(HttpUtility.HtmlDecode(sProfileTemp.TemplateValue));
            

            //替换模板内容
            var imguri = "";
            // sendBody.Replace("{{$toptitle}}", email.Body);
            //var connString = ConfigurationManager.AppSettings["StorageConnectionString"];
            //foreach (var keyValue in connString.Split(';').Select(keyValueString => keyValueString.Split('=')).Where(keyValue => keyValue[0].Trim().ToLower() == "blobendpoint"))
            //{
            //    imguri = keyValue[1].Trim();
            //}

            email.Title = sProfileTemp.TemplateTitle;
            email.Body = emailTemp.EmailTemplate;
            email.Body = Regex.Replace(email.Body, @"\{\$uri\}", imguri);
            email.Body = Regex.Replace(email.Body, @"\{\$username\}", username);
            email.Body = Regex.Replace(email.Body, @"\{\$password\}", password);
            _emailSendService.Send(emailService.AccountId, email.To, email.Title, email.Body);

            email.From = emailService.AccountId;
            _emailRepository.Add(email);
            return Mapper.Map<EmailResponse>(email);
        }

        public EmailResponse CreateResetPassEmail(string username, string link, CreateEmailRequest request)
        {
            Email email = Mapper.Map<CreateEmailRequest, Email>(request);

            email.SenderId = _userId;

            dynamic emailService = null;
            dynamic emailTemp = null;
            
            var _SProfile = _profileService.GetProfile("EmailServerConfig");
            if (_SProfile != null)
                emailService = Json.Decode(HttpUtility.HtmlDecode(_SProfile.Value));
           
            var _SProfileTemp = _emailTemplateService.GetEmailTemplate("EmailResetPasswordTemplate", LangHelper.GetLanguage());
            if (_SProfileTemp != null)
                emailTemp = Json.Decode(HttpUtility.HtmlDecode(_SProfileTemp.TemplateValue));


            //替换模板内容
            var imguri = "";
            // sendBody.Replace("{{$toptitle}}", email.Body);
            //var connString = ConfigurationManager.AppSettings["StorageConnectionString"];
            //foreach (var keyValue in connString.Split(';').Select(keyValueString => keyValueString.Split('=')).Where(keyValue => keyValue[0].Trim().ToLower() == "blobendpoint"))
            //{
            //    imguri = keyValue[1].Trim();
            //}

            email.Title = _SProfileTemp.TemplateTitle;
            email.Body = emailTemp.EmailTemplate;
            email.Body = Regex.Replace(email.Body, @"\{\$uri\}", imguri);
            email.Body = Regex.Replace(email.Body, @"\{\$username\}", username);
            email.Body = Regex.Replace(email.Body, @"\{\$url\}", link);
            _emailSendService.Send(emailService.AccountId, email.To, email.Title, email.Body);

            email.From = emailService.AccountId;
            email.EmailType = EmailType.ResetPassEmail;
            _emailRepository.Add(email);
            _emailRepository.UnitOfWork.SaveChanges();
            return Mapper.Map<EmailResponse>(email);
        }

        public EmailResponse CreateConfirmRegisterEmail(string username, string link, CreateEmailRequest request)
        {
            Email email = Mapper.Map<CreateEmailRequest, Email>(request);

            email.SenderId = _userId;

            dynamic emailService = null;
            dynamic emailTemp = null;
           
            var sProfile = _profileService.GetProfile("EmailServerConfig");
            if (sProfile != null)
                emailService = Json.Decode(HttpUtility.HtmlDecode(sProfile.Value));
            
            var sProfileTemp = _emailTemplateService.GetEmailTemplate("EmailComfirmTemplate", LangHelper.GetLanguage());
            if (sProfileTemp != null)
                emailTemp = Json.Decode(HttpUtility.HtmlDecode(sProfileTemp.TemplateValue));

            if (emailService != null && sProfileTemp != null)
            {

                //替换模板内容
                email.Title = sProfileTemp.TemplateTitle;
                email.Body = emailTemp.EmailTemplate;
                email.Body = Regex.Replace(email.Body, @"\{\$username\}", username);
                email.Body = Regex.Replace(email.Body, @"\{\$url\}", link);
                _emailSendService.Send(emailService.AccountId, email.To, email.Title, email.Body);

                email.From = emailService.AccountId;
                email.EmailType = EmailType.ResetPassEmail;
                _emailRepository.Add(email);
                _emailRepository.UnitOfWork.SaveChanges();
            }

            return Mapper.Map<EmailResponse>(email);
        }

        public EmailResponse GetEmail(Guid emailId)
        {
            return Mapper.Map<EmailResponse>(_emailRepository.GetOrThrow(emailId));
        }

        public void Add(Email email)
        {
            _emailRepository.Add(email);
        }
    }
}
