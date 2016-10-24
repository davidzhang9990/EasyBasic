using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using XDDEasy.Domain.Identity;
using System.Runtime.Serialization;
using Common.Models;

namespace XDDEasy.Domain.AccountAggregates
{
    public class User : IdentityUser
    {
        private List<EnumRole> _userRoles = new List<EnumRole>();

        public string TrueName { get; set; }

        public string TwoPassword { get; set; }

        public int Sex { get; set; }

        public string CardNo { get; set; }

        public DateTime BirthDay { get; set; }

        public string Address { get; set; }

        public string ParentNumber { get; set; }

        public string RecNumber { get; set; }

        public string AgentNumber { get; set; }

        public string KeyString { get; set; }

        public int IsApproved { get; set; }

        public double AllCoin { get; set; }

        public double BuyCoin { get; set; }

        public int CurrLen { get; set; }

        public int RecLen { get; set; }

        public DateTime DateApproved { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }
        [Required]
        public DateTime DateUpdated { get; set; }
        [Required]
        public Guid AddedBy { get; set; }
        [Required]
        public Guid UpdatedBy { get; set; }

        public bool ActiveFlag { get; set; }

        [NotMapped]
        public List<EnumRole> UserRoles
        {
            get { return _userRoles; }
            set { _userRoles = value; }
        }

        public virtual List<EnumRole> GetUserRoles(EasyUserManager manager)
        {
            var roleArray = manager.GetRoles(Id);
            _userRoles = roleArray.Select(x => (EnumRole)Enum.Parse(typeof(EnumRole), x.ToString(), true)).ToList();
            return _userRoles;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // 请注意，authenticationType 必须与 CookieAuthenticationOptions.AuthenticationType 中定义的相应项匹配
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在此处添加自定义用户声明
            return userIdentity;
        }
    }
}
