using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using XDDEasy.Contract.UserLog;

namespace XDDEasy.Domain.UserLogger
{
    public interface IUserLogServices
    {
        void Add(UserLogResponse userLogResponse);
        IEnumerable<UserLogResponse> Query();
        IEnumerable<UserLogResponse> Query(string uerId);
    }
    public class UserLogServices : IUserLogServices
    {
        private readonly IUserLogRepository _iuserLogRepository;
        public UserLogServices(IUserLogRepository iUserLogRepository)
        {
            _iuserLogRepository = iUserLogRepository;
        }
        public void Add(UserLogResponse userLogResponse)
        {
            _iuserLogRepository.Add(userLogResponse);
        }

        public IEnumerable<UserLogResponse> Query()
        {
            return _iuserLogRepository.Query();
        }

        public IEnumerable<UserLogResponse> Query(string uerId)
        {
            return _iuserLogRepository.Query(uerId);
        }
    }
}
