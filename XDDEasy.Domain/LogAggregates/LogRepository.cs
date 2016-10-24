using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.OData.Query;
using XDDEasy.Contract.LogContract;
using Common.EntityFramework.DataAccess;
using Common.Models;

namespace XDDEasy.Domain.LogAggregates
{
    public interface ILogRepository : IRepository<Log>
    {
        void SaveLog(string message, int type);
        void SaveLog(CreateLogRequest request);
        IEnumerable<Log> Query(ODataQueryOptions<Log> options);
    }

    public class LogRepository : GenericRepository<Log>, ILogRepository
    {
        private readonly RequestContext _context;

        public LogRepository(RequestContext context)
        {
            _context = context;
        }

        public void SaveLog(string message, int type)
        {
            var log = new Log
            {
                Id = Guid.NewGuid(),
                Address = HttpContext.Current.Request.UserHostAddress,
                Content = message,
                Type = type,
                UserAgent = HttpContext.Current.Request.Headers["User-Agent"],
                UserId = _context.UserId.ToString()
            };
            Add(log);
        }

        public void SaveLog(CreateLogRequest request)
        {
            var log = new Log
            {
                Id = Guid.NewGuid(),
                Address = HttpContext.Current.Request.UserHostAddress,
                Content = request.Content,
                Type = request.Type == 0 ? 1 : request.Type,
                UserAgent = HttpContext.Current.Request.Headers["User-Agent"],
                UserId = _context.UserId.ToString(),
                Property = request.Property
            };
            Add(log);
        }

        public IEnumerable<Log> Query(ODataQueryOptions<Log> options)
        {

            return options.ApplyTo(GetQuery()) as IEnumerable<Log>;
        }
    }
}
