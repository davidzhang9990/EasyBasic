using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using AutoMapper;
using XDDEasy.Contract;
using XDDEasy.Contract.AccountContract;
using XDDEasy.Domain.AccountAggregates;
using XDDEasy.Domain.Identity;
using Newtonsoft.Json;
using XDDEasy.Contract.ResourceContract;
using XDDEasy.Domain.ResourceAggregates;

namespace XDDEasy.WebApi.Host
{
    public class MapperConfig
    {
        public static void Configure()
        {
            UserMappings();
            ResourceMappings();
        }

        public static void UserMappings()
        {
            //Mapper.CreateMap<User, UserResponse>()
            //    .ForMember(dest => dest.Roles, opt => opt.MapFrom(source => source.UserRoles));

          

            //Mapper.CreateMap<User, AccountLoginInfo>()
            //    .ForMember(dest => dest.Password, opt => opt.MapFrom(source => source.PasswordHash))
            //     .AfterMap((student, response) =>
            //     {
            //         response.Photo = string.IsNullOrEmpty(student.Photo) ? PhotoHelper.GetRightPhoto(WebConfigurationManager.GetValue("Default_UserIcon")) : PhotoHelper.GetRightPhoto(student.Photo);
            //     });

            

            //Teacher
            //Mapper.CreateMap<UserProfile, UserProfileResponse>();
        }

        private static void ResourceMappings()
        {
            Mapper.CreateMap<ResourceRequest, Resource>();
            Mapper.CreateMap<Resource, ResourceResponse>();
        }
    }
}
