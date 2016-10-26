using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.EntityFramework.Model;

namespace XDDEasy.Domain.PageAggregates
{
    [Table("RolePage")]
    public class RolePage : AuditEntity
    {
        public string RoleId { get; set; }
        public Guid PageId { get; set; }
        public string DisplayName { get; set; }
        public short Sequence { get; set; }
    }
}
