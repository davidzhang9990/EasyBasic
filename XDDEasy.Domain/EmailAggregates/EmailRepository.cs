using System;
using System.Collections.Generic;
using System.Linq;
using XDDEasy.Contract.EmailContract;
using Common.EntityFramework.DataAccess;
using Common.Models;
using AutoMapper;
using System.Web.Http.OData.Query;
using XDDEasy.Contract;

namespace XDDEasy.Domain.EmailAggregates
{
    public interface IEmailRepository : IRepository<Email>
    {
        IEnumerable<EmailResponse> GetAllEmail();

        Email GetEmailById(Guid emailId);

        IEnumerable<Email> GetEmailsByUser(Guid? userId);

        IEnumerable<Email> ODataQuery(ODataQueryOptions<Email> options);
    }

    public class EmailRepository : GenericRepository<Email>, IEmailRepository
    {
        private readonly RequestContext _requestContext;

        public EmailRepository(RequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        public IEnumerable<EmailResponse> GetAllEmail()
        {
            var allEmails = GetQuery();
            return Mapper.Map<IEnumerable<EmailResponse>>(allEmails);
        }

        public IEnumerable<Email> ODataQuery(ODataQueryOptions<Email> options)
        {
            return options.ApplyTo(GetQuery()) as IEnumerable<Email>;
        }

        public Email GetEmailById(Guid emailId)
        {
            return GetAll().FirstOrDefault(x => x.Id == emailId);
        }

        public IEnumerable<Email> GetEmailsByUser(Guid? userId)
        {
            if (!userId.HasValue)
                userId = _requestContext.UserId;
            return GetQuery(x => x.SenderId == userId);
        }
    }
}
