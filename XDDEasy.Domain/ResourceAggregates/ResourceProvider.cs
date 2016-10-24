using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.OData.Query;
using AutoMapper;
using Common.EntityFramework.DataAccess;
using Common.Exception;
using XDDEasy.Contract.ResourceContract;

namespace XDDEasy.Domain.ResourceAggregates
{
    public interface IResourceProvider
    {
        object GetResource(string name, string culture);
        Resource Add(ResourceRequest resourceRequest);
        void Delete(Guid resourceId);
        void Delete(string culture, string name);
        IEnumerable<Resource> Query(ODataQueryOptions<Resource> options);
        IList<Resource> ReadResources();
        Resource ReadResource(string name, string culture);
        IList<Resource> GetResourcesByCulture(string culture);
        void Update(Guid id, ResourceRequest resourceRequests);
    }
    public class ResourceProvider : IResourceProvider
    {
        // Cache list of resources
        private static Dictionary<string, Resource> _resources = null;
        private static readonly object LockResources = new object();
        private readonly IRepository<Resource> _resourceRepository;

        public ResourceProvider(IRepository<Resource> resourceRepository)
        {
            Cache = true; // By default, enable caching for performance
            _resourceRepository = resourceRepository;
        }

        private bool Cache { get; set; } // Cache resources ?

        /// <summary>
        /// Returns a single resource for a specific culture
        /// </summary>
        /// <param name="name">Resorce name (ie key)</param>
        /// <param name="culture">Culture code</param>
        /// <returns>Resource</returns>
        public object GetResource(string name, string culture)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(EasyResource.Value("Exception_ResourceNameNotEmpty"));

            if (string.IsNullOrWhiteSpace(culture))
                throw new ArgumentException(EasyResource.Value("Exception_CultureNameNotEmpty"));

            // normalize
            culture = culture.ToLowerInvariant();
            name = name.ToLowerInvariant();

            if (Cache && _resources == null)
            {
                // Fetch all resources

                lock (LockResources)
                {

                    if (_resources == null)
                    {
                        _resources = ReadResources().ToDictionary(r => string.Format("{0}.{1}", r.Culture.ToLowerInvariant(), r.Name.ToLowerInvariant()));
                    }
                }
            }

            if (Cache && _resources.ContainsKey(string.Format("{0}.{1}", culture, name)))
            {
                return _resources[string.Format("{0}.{1}", culture, name)].Value;
            }
            return ReadResource(name, culture).Value;
        }

        public Resource Add(ResourceRequest resourceRequest)
        {
            var existResource = _resourceRepository.First(x => x.Name == resourceRequest.Name && x.Culture == resourceRequest.Culture);
            if (existResource == null)
            {
                var resource = Mapper.Map<Resource>(resourceRequest);
                _resourceRepository.Add(resource);
                _resourceRepository.UnitOfWork.SaveChanges();
                return resource;
            }
            else
            {
                throw new BadRequestException(EasyResource.Value("Exception_ResourceAlreadyExist"));
            }
        }

        public void Delete(Guid resourceId)
        {
            var existResource = _resourceRepository.GetByKey(resourceId);
            if (existResource != null)
                _resourceRepository.Delete(existResource);
            _resourceRepository.UnitOfWork.SaveChanges();

        }

        public void Delete(string culture, string name)
        {
            var existResource = _resourceRepository.First(x => x.Name == name && x.Culture == culture);
            if (existResource != null)
                _resourceRepository.Delete(existResource);
            _resourceRepository.UnitOfWork.SaveChanges();

        }

        public IEnumerable<Resource> Query(ODataQueryOptions<Resource> options)
        {
            return options.ApplyTo(_resourceRepository.GetQuery()) as IEnumerable<Resource>;
        }


        /// <summary>
        /// Returns all resources for all cultures. (Needed for caching)
        /// </summary>
        /// <returns>A list of resources</returns>
        public IList<Resource> ReadResources()
        {
            return _resourceRepository.GetAll().ToList();
        }


        /// <summary>
        /// Returns a single resource for a specific culture
        /// </summary>
        /// <param name="name">Resorce name (ie key)</param>
        /// <param name="culture">Culture code</param>
        /// <returns>Resource</returns>
        public Resource ReadResource(string name, string culture)
        {
            var resource = _resourceRepository.First(x => x.Name == name && x.Culture == culture);
            return resource ?? new Resource();
        }

        public IList<Resource> GetResourcesByCulture(string culture)
        {
            return _resourceRepository.GetQuery(x => x.Culture == culture).ToList();
        }

        public void Update(Guid id, ResourceRequest resourceRequest)
        {
            var resource = _resourceRepository.GetByKey(id);
            Mapper.Map(resourceRequest, resource);
            _resourceRepository.Update(resource);
            _resourceRepository.UnitOfWork.SaveChanges();
        }
    }
}
