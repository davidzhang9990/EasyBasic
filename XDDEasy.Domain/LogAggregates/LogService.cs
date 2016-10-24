using System.Collections.Generic;
using System.Web;
using System.Web.Http.OData.Query;
using XDDEasy.Contract.LogContract;
using Common.EntityFramework.DataAccess;
using Common.Models;

namespace XDDEasy.Domain.LogAggregates
{
    public interface ILogService
    {
        void SaveLog(string message, int type);
        void SaveLog(CreateLogRequest request);
        IEnumerable<Log> Query(ODataQueryOptions<Log> options);
    }

    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;

        public LogService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public void SaveLog(string message, int type)
        {
            _logRepository.SaveLog(message, type);
        }

        public void SaveLog(CreateLogRequest request)
        {
            _logRepository.SaveLog(request);
        }

        public IEnumerable<Log> Query(ODataQueryOptions<Log> options)
        {

            return _logRepository.Query(options);
        }
    }
}
