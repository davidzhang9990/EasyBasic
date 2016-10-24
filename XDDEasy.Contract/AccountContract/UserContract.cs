using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Common.Models;
using Newtonsoft.Json;

namespace XDDEasy.Contract.AccountContract
{
    public class UserResponse
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public DateTime DateUpdated { get; set; }

        private List<EnumRole> _roles = new List<EnumRole>();
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public List<EnumRole> Roles
        {
            get { return _roles; }
            set { _roles = value; }

        }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string EmailConfirmed { get; set; }

        [DataMember]
        public Int16 Sex { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Zip { get; set; }
        [DataMember]
        public string Photo { get; set; }
        [DataMember]
        public Guid? SchoolId { get; set; }
        [DataMember]
        public string SchoolName { get; set; }

        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public bool IsShareToFacebook { get; set; }
    }

    [DataContract]
    [JsonObject]
    public class UpdateUserRequest
    {
        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        /*      [DataMember]
              public string FullName { get; set; }*/
        [DataMember]
        public Int16 Sex { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Zip { get; set; }
        [DataMember]
        public string Photo { get; set; }
        [DataMember]
        public string Email { get; set; }
    }

    public class CreateUserRequest : UpdateUserRequest
    {
        private List<EnumRole> _roles = new List<EnumRole>();

        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public Guid? SchoolId { get; set; }

        [DataMember]
        public List<EnumRole> Roles
        {
            get { return _roles; }
            set { _roles = value; }

        }
        [DataMember]
        public string Email { get; set; }
    }

    [DataContract]
    public class UpdatePasswordRequest
    {
        [DataMember]
        public string Password { get; set; }
    }

    [DataContract]
    public class StudentLoginInfo
    {
        [DataMember]
        public Guid UserId { get; set; }
        [DataMember]
        public Guid StudentId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string Photo { get; set; }
    }


    public class AccountLoginInfo
    {
        [DataMember]
        public Guid UserId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string Photo { get; set; }
    }

    [JsonObject]
    [DataContract]
    public class UserProfileRequest
    {
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
        [DataMember(IsRequired = true)]
        public string Value { get; set; }
    }

    public enum EnumUserProfileName
    {
        IsShareToFaceBook
    }
}
