using System;
using System.Collections.Generic;
using System.Web.Http;
using XDDEasy.Contract;
using XDDEasy.Domain.PageAggregates;
using Common.WebApi.Attributes;

namespace XDDEasy.WebApi.Host.Controllers
{
    [RoutePrefix("api/page")]
    [Authorize]
    public class PageController : ApiController
    {
        private readonly IPageService _pageService;

        public PageController(IPageService pageService)
        {
            _pageService = pageService;
        }

        [HttpGet]
        [Route("")]
        public IEnumerable<PageResponse> GetAllPages()
        {
            return _pageService.GetAllPages();
        }

        [HttpPost]
        [Route("")]
        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        public PageResponse AddPage(CreatePageRequest request)
        {
            return _pageService.AddPage(request);
        }

        [HttpPut]
        [Route("{pageId:guid}")]
        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        public void EditPage(Guid pageId, UpdatePageRequest request)
        {
            _pageService.EditPage(pageId, request);
        }

        [HttpDelete]
        [Route("{pageId:guid}")]
        [Transaction]
        public void DeletePage(Guid pageId)
        {
            _pageService.DeletePage(pageId);
        }

        [HttpGet]
        [Route("user/{userId:guid?}")]
        public IEnumerable<RolePageResponse> GetRolePages(Guid? userId = default(Guid?))
        {
            return _pageService.GetRolePages(userId);
        }

        [HttpGet]
        [Route("role/{roleNames?}")]
        [AllowAnonymous]
        public IEnumerable<RolePageResponse> GetRolePagesByRoleName(string roleNames = null)
        {
            return _pageService.GetRolePagesByRoleName(roleNames);
        }

        [HttpPost]
        [Route("rolepage")]
        [Transaction]
        [CheckModelForNull]
        [ValidateModelState]
        public void AddRolePage(CreateRolePageRequest request)
        {
            _pageService.AddRolePage(request);
        }

        [HttpDelete]
        [Route("{pageId:guid}/role/{roleId:guid}")]
        [Transaction]
        public void DeleteRolePage(Guid pageId, Guid roleId)
        {
            _pageService.DeleteRolePage(pageId, roleId);
        }
    }
}
