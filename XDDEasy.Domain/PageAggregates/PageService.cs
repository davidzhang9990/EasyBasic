using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common.EntityFramework.DataAccess;
using XDDEasy.Contract;
using XDDEasy.Domain.Identity;
using Common.Models;
using Microsoft.AspNet.Identity;

namespace XDDEasy.Domain.PageAggregates
{
    public interface IPageService
    {
        IEnumerable<PageResponse> GetAllPages();
        PageResponse AddPage(CreatePageRequest request);
        void EditPage(Guid pageId, UpdatePageRequest request);
        void DeletePage(Guid pageId);
        IEnumerable<RolePageResponse> GetRolePages(Guid? userId);
        void AddRolePage(CreateRolePageRequest request);
        void DeleteRolePage(Guid pageId, Guid roleId);
        IEnumerable<RolePageResponse> GetRolePagesByRoleName(string roleName);
    }

    public class PageService : IPageService
    {
        private readonly IRepository<Page> _pageRepository;
        private readonly IRepository<RolePage> _rolePageRepository;
        private readonly RequestContext _requestContext;
        private readonly EasyUserManager _userManager;
        private readonly EasyRoleManager _roleManager;

        public PageService(IRepository<Page> pageRepository, IRepository<RolePage> rolePageRepository,
            RequestContext requestContext, EasyUserManager userManager, EasyRoleManager roleManager)
        {
            _pageRepository = pageRepository;
            _rolePageRepository = rolePageRepository;
            _requestContext = requestContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IEnumerable<PageResponse> GetAllPages()
        {
            var pages = _pageRepository.GetAll();
            return Mapper.Map<IEnumerable<PageResponse>>(pages);
        }

        public PageResponse AddPage(CreatePageRequest request)
        {
            var page = Mapper.Map<Page>(request);
            _pageRepository.Add(page);
            return Mapper.Map<PageResponse>(page);
        }

        public void EditPage(Guid pageId, UpdatePageRequest request)
        {
            var page = _pageRepository.GetOrThrow(pageId);
            Mapper.Map<UpdatePageRequest, Page>(request, page);
            _pageRepository.Update(page);
        }

        public void DeletePage(Guid pageId)
        {
            var page = _pageRepository.GetOrThrow(pageId);
            page.ActiveFlag = false;
            _pageRepository.Update(page);
        }

        public IEnumerable<RolePageResponse> GetRolePages(Guid? userId)
        {
            if (!userId.HasValue)
                userId = _requestContext.UserId;
            var user = _userManager.FindById(userId.ToString());
            var roleIds = user.Roles.Select(x => x.RoleId);
            var rolePages = from rolePage in _rolePageRepository.GetQuery(x => roleIds.Contains(x.RoleId))
                            group rolePage by rolePage.PageId
                                into groups
                                select groups.OrderBy(x => x.Sequence).FirstOrDefault();
            var pages = from page in _pageRepository.GetQuery()
                        join rolePage in rolePages on page.Id equals rolePage.PageId
                        orderby rolePage.Sequence
                        select new RolePageResponse
                        {
                            DisplayName = rolePage.DisplayName,
                            PageId = page.Id,
                            RoleId = rolePage.RoleId,
                            ActionName = page.ActionName,
                            ControlName = page.ControlName,
                            Area = page.Area,
                            ShowPage = page.ShowPage
                        };
            return pages;
        }

        public void AddRolePage(CreateRolePageRequest request)
        {
            var rolePage = Mapper.Map<RolePage>(request);
            _rolePageRepository.Add(rolePage);
        }

        public void DeleteRolePage(Guid pageId, Guid roleId)
        {
            var rolePage = _rolePageRepository.FindOne(x => x.PageId == pageId && x.RoleId == roleId.ToString());
            _rolePageRepository.Delete(rolePage);
        }

        public IEnumerable<RolePageResponse> GetRolePagesByRoleName(string roleNames)
        {
            List<string> roleIds;
            if (string.IsNullOrEmpty(roleNames))
                roleIds = _requestContext.RoleIds;
            else
            {
                var roleNameArray = roleNames.Split(',');
                roleIds = roleNameArray.Select(x => _roleManager.FindByName(x).Id).ToList();
            }
            var rolePages = from rolePage in _rolePageRepository.GetQuery(x => roleIds.Contains(x.RoleId))
                            group rolePage by rolePage.PageId
                                into groups
                                select groups.OrderBy(x => x.Sequence).FirstOrDefault();
            var pages = from page in _pageRepository.GetQuery()
                        join rolePage in rolePages on page.Id equals rolePage.PageId
                        orderby rolePage.Sequence
                        select new RolePageResponse
                        {
                            DisplayName = rolePage.DisplayName,
                            PageId = page.Id,
                            RoleId = rolePage.RoleId,
                            ActionName = page.ActionName,
                            ControlName = page.ControlName,
                            Area = page.Area,
                            ShowPage = page.ShowPage
                        };
            return pages;
        }
    }
}
