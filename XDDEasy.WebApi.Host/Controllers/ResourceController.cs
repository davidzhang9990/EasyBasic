using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Extensions;
using System.Web.Http.OData.Query;
using AutoMapper;
using Common.EntityFramework.DataAccess;
using XDDEasy.Domain.ResourceAggregates;
using Common.WebApi.Attributes;
using log4net.Util;
using Newtonsoft.Json;
using WebApi.OutputCache.V2;
using XDDEasy.Contract.ResourceContract;

namespace XDDEasy.WebApi.Host.Controllers
{
    [RoutePrefix("api/resource")]
    public class ResourceController : ApiController
    {
        private readonly IResourceProvider _resourceProvider;

        public ResourceController(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
        }

        [HttpPost]
        [ValidateModelState]
        [CheckModelForNull]
        [Route("")]
        public ResourceResponse Post(ResourceRequest resourceRequest)
        {
            var resources = _resourceProvider.Add(resourceRequest);
            return Mapper.Map<ResourceResponse>(resources);
        }

        [HttpPut]
        [ValidateModelState]
        [CheckModelForNull]
        [Route("{id:guid}")]
        public void Put(Guid id, ResourceRequest resourceRequests)
        {
            _resourceProvider.Update(id, resourceRequests);
        }

        [HttpDelete]
        [Route("{resourceId:guid}")]
        public void Delete(Guid resourceId)
        {
            _resourceProvider.Delete(resourceId);
        }

        [HttpGet]
        [Route("{resourceId:guid}")]
        public ResourceResponse Get(Guid resourceId)
        {
            var resource = _resourceProvider.GetResource(resourceId);
            return Mapper.Map<ResourceResponse>(resource);
        }

        [HttpDelete]
        [Route("cultrue/{culture}/name/{name}")]
        public void Delete(string culture, string name)
        {
            _resourceProvider.Delete(culture, name);
        }

        [HttpGet]
        [Route("")]
        public List<ResourceResponse> Query(ODataQueryOptions<Resource> options)
        {
            var resources = _resourceProvider.Query(options);
            return Mapper.Map<List<ResourceResponse>>(resources);
        }

        [Authorize]
        [HttpGet]
        [Route("paging")]
        public IEnumerable<ResourceResponse> GetPaging(ODataQueryOptions<Resource> options)
        {
            var resource = _resourceProvider.Query(options);
            var studentResponses = Mapper.Map<IEnumerable<ResourceResponse>>(resource);
            var result = new PageResult<ResourceResponse>(
               studentResponses,
               Request.ODataProperties().NextLink,
               Request.ODataProperties().TotalCount);
            return result;
        }

        [HttpGet]
        [Route("culture/{culture}")]
        [CacheOutput(ClientTimeSpan = 5000, ServerTimeSpan = 5000)]
        public HttpResponseMessage GetResourcesByCulture(string culture)
        {
            var resources = _resourceProvider.GetResourcesByCulture(culture).Select(x => new { x.Name, x.Value }).Distinct();
            var data = resources.ToDictionary(p => p.Name, x => x.Value);
            var json = JsonConvert.SerializeObject(data);
            var responseBody = string.Format("var {0} = {1}", "EqlResource", json);
            var response = Request.CreateResponse(System.Net.HttpStatusCode.OK);
            response.Content = new StringContent(responseBody, System.Text.Encoding.UTF8, "text/plain");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-javascript");
            return response;
        }
    }
}
