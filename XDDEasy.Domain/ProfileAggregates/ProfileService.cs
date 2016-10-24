using System;
using System.Collections.Generic;
using AutoMapper;
using XDDEasy.Contract;
using XDDEasy.Domain.ResourceAggregates;
using Common.EntityFramework.DataAccess;
using Common.Exception;
using Common.Models;

namespace XDDEasy.Domain.ProfileAggregates
{
    public interface IProfileService
    {
        ProfileResponse GetProfile(string name);
        ProfileResponse GetProfileById(Guid id);
        ProfileResponse CreateProfile(ProfileRequest request);
        void UpdateProfile(Guid id, ProfileRequest request);
        void UpdateSchoolProfile(ProfileRequest request);
        void DeleteProfile(Guid id);
    }
    public class ProfileService : IProfileService
    {
        private readonly IRepository<Profile> _profileRepository;
        private readonly RequestContext _requestContext;

        public ProfileService(IRepository<Profile> profileRepository, RequestContext requestContext)
        {
            _profileRepository = profileRepository;
            _requestContext = requestContext;
        }
        public ProfileResponse GetProfile(string name)
        {
            var profile = _profileRepository.FindOne(x => x.Name == name);
            /*if(profile == null)
                throw new NotFoundException(string.Format(EqlResource.Value("Exception_NotFindProfileName"), name, schoolId.Value));*/
            return profile != null ? Mapper.Map<ProfileResponse>(profile) : null;
        }

        public ProfileResponse GetProfileById(Guid id)
        {
            var profile = _profileRepository.GetOrThrow(id);
            return Mapper.Map<ProfileResponse>(profile);
        }

        public ProfileResponse CreateProfile(ProfileRequest request)
        {
            if (_profileRepository.Any(x => x.Name == request.Name && x.Value == request.Value))
                throw new BadRequestException(string.Format(EasyResource.Value("Exception_AlreadyExists"), request.Name, request.Value));
            var profile = Mapper.Map<Profile>(request);
            _profileRepository.Add(profile);
            return Mapper.Map<ProfileResponse>(profile);
        }

        public void UpdateProfile(Guid id, ProfileRequest request)
        {
            var profile = _profileRepository.GetOrThrow(id);
            if (_profileRepository.Any(x => x.Name == profile.Name && x.Value == request.Value && x.Id != id))
                throw new BadRequestException(string.Format(EasyResource.Value("Exception_AlreadyExists"), request.Name, request.Value));

            profile.Value = request.Value;
            _profileRepository.Update(profile);
        }

        public void UpdateSchoolProfile(ProfileRequest request)
        {
            var profile = _profileRepository.FindOne(x => x.Name == request.Name);
            if (profile == null)
                throw new NotFoundException(string.Format(EasyResource.Value("Exception_NotFindProfileName"), request.Name, request.SchoolId.Value));
            profile.Value = request.Value;
            _profileRepository.Update(profile);
        }

        public void DeleteProfile(Guid id)
        {
            var profile = _profileRepository.GetOrThrow(id);
            profile.ActiveFlag = false;
            _profileRepository.Update(profile);
        }
    }
}
