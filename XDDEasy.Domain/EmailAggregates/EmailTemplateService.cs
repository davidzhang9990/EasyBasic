using System;
using System.Collections.Generic;
using AutoMapper;
using XDDEasy.Contract;
using XDDEasy.Domain.ResourceAggregates;
using Common.EntityFramework.DataAccess;
using Common.Exception;
using Common.Models;
using XDDEasy.Contract.EmailContract;

namespace XDDEasy.Domain.EmailAggregates
{
    public interface IEmailTemplateService
    {
        EmailTemplateResponse GetEmailTemplate(string name, string eCulture);
        EmailTemplateResponse GetEmailTemplateById(Guid id);
        EmailTemplateResponse CreateEmailTemplate(EmailTemplateRequest request);
        void UpdateEmailTemplate(Guid id, EmailTemplateRequest request);
        void UpdateSchoolEmailTemplate(EmailTemplateRequest request);
        void DeleteEmailTemplate(Guid id);
    }
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly IRepository<EmailTemplate> _EmailTemplateRepository;
        private readonly RequestContext _requestContext;

        public EmailTemplateService(IRepository<EmailTemplate> EmailTemplateRepository, RequestContext requestContext)
        {
            _EmailTemplateRepository = EmailTemplateRepository;
            _requestContext = requestContext;
        }
        public EmailTemplateResponse GetEmailTemplate(string name, string eCulture)
        {
            var EmailTemplate = _EmailTemplateRepository.FindOne(x => x.TemplateType == name && x.TemplateCulture == eCulture);
            /*if(EmailTemplate == null)
                throw new NotFoundException(string.Format(EqlResource.Value("Exception_NotFindEmailTemplateName"), name, schoolId.Value));*/
            return EmailTemplate != null ? Mapper.Map<EmailTemplateResponse>(EmailTemplate) : null;
        }

        public EmailTemplateResponse GetEmailTemplateById(Guid id)
        {
            var EmailTemplate = _EmailTemplateRepository.GetOrThrow(id);
            return Mapper.Map<EmailTemplateResponse>(EmailTemplate);
        }

       

        public EmailTemplateResponse CreateEmailTemplate(EmailTemplateRequest request)
        {
            if (_EmailTemplateRepository.Any(x => x.TemplateType == request.TemplateType))
                throw new BadRequestException(string.Format(EasyResource.Value("Exception_AlreadyExists"), request.TemplateType));
            var EmailTemplate = Mapper.Map<EmailTemplate>(request);
            _EmailTemplateRepository.Add(EmailTemplate);
            return Mapper.Map<EmailTemplateResponse>(EmailTemplate);
        }

        public void UpdateEmailTemplate(Guid id, EmailTemplateRequest request)
        {
            var EmailTemplate = _EmailTemplateRepository.GetOrThrow(id);
            if (_EmailTemplateRepository.Any(x => x.TemplateType == EmailTemplate.TemplateType && x.Id != id))
                throw new BadRequestException(string.Format(EasyResource.Value("Exception_AlreadyExists"), request.TemplateType));

            EmailTemplate.TemplateValue = request.TemplateValue;
            _EmailTemplateRepository.Update(EmailTemplate);
        }

        public void UpdateSchoolEmailTemplate(EmailTemplateRequest request)
        {
            var EmailTemplate = _EmailTemplateRepository.FindOne(x => x.TemplateType == request.TemplateType);
            if (EmailTemplate == null)
                throw new NotFoundException(string.Format(EasyResource.Value("Exception_NotFindEmailTemplateName"), request.TemplateType, request.SchoolId.Value));
            EmailTemplate.TemplateValue = request.TemplateValue;
            _EmailTemplateRepository.Update(EmailTemplate);
        }

        public void DeleteEmailTemplate(Guid id)
        {
            var EmailTemplate = _EmailTemplateRepository.GetOrThrow(id);
            EmailTemplate.ActiveFlag = false;
            _EmailTemplateRepository.Update(EmailTemplate);
        }
    }
}
