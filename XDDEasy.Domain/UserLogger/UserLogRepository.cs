using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using XDDEasy.Contract.UserLog;
using Common.EntityFramework.DataAccess;

namespace XDDEasy.Domain.UserLogger
{
    public interface IUserLogRepository : IRepository<UserLog>
    {
        void Add(UserLogResponse userLogResponse);
        IEnumerable<UserLogResponse> Query();
        IEnumerable<UserLogResponse> Query(string uerId);
    }
    public class UserLogRepository : GenericRepository<UserLog>, IUserLogRepository
    {

        public void Add(UserLogResponse userLogResponse)
        {
            var userlog = new UserLog()
            {
                UserId = userLogResponse.UserId,
                Content = userLogResponse.Content,
                Path = userLogResponse.Path,
                UserAgent = userLogResponse.UserAgent,
                Ip = userLogResponse.Ip,
                Host = userLogResponse.Host
            };
            Save(userlog);
            UnitOfWork.SaveChanges();
        }

        public IEnumerable<UserLogResponse> Query()
        {
            return Mapper.Map<List<UserLogResponse>>(GetQuery().ToList());
        }

        public IEnumerable<UserLogResponse> Query(string uerId)
        {
            return Mapper.Map<List<UserLogResponse>>(GetQuery(x => x.UserId.Equals(uerId)).ToList());
        }

    }
}
