using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Extensions;
using System.Web.Http.OData.Query;
using AutoMapper;
using Common.Exception;
using Newtonsoft.Json.Linq;
using XDDEasy.Domain.AccountAggregates;
using XDDEasy.Contract.AccountContract;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace XDDEasy.WebApi.Host.Controllers
{
    public class AccountController : ApiController
    {
        private readonly IAccountService _accountService;
        private const string HostUrlFormat = "{0}://{1}/";

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [Route("api/account/odata")]
        public PageResult<UserResponse> GetPaging(ODataQueryOptions<User> options)
        {
            var results = _accountService.GetPagingUsers(options);
            var result = new PageResult<UserResponse>(
                results,
                Request.ODataProperties().NextLink,
                Request.ODataProperties().TotalCount);
            return result;
        }

        [HttpGet]
        [Route("api/login/{userName}/{password}")]
        [Obsolete]
        public UserResponse LoginDeleted(string userName, string password)
        {
            return _accountService.Login(userName, password);
        }

        [HttpGet]
        [Route("api/login")]
        public UserResponse Login(string userName, string password = "")
        {
            password = password ?? "";
            return _accountService.Login(userName, password);
        }

        [HttpPost]
        [Route("api/logout")]
        public void Logout()
        {
            _accountService.Logout();
        }
    }
}
