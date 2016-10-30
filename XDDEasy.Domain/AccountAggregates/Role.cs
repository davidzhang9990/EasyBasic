using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XDDEasy.Domain.AccountAggregates
{
    public class Role : IdentityRole
    {
        public string Desc { get; set; }

        [Required]
        public DateTime DateAdded { get; set; }
        [Required]
        public DateTime DateUpdated { get; set; }
        [Required]
        public Guid AddedBy { get; set; }
        [Required]
        public Guid UpdatedBy { get; set; }

        public bool ActiveFlag { get; set; }
    }
}
