using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.EntityFramework.Model;

namespace XDDEasy.Domain.ResourceAggregates
{
    [Table("Resource")]
    public class Resource : AuditEntity
    {
        public Resource()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public string Culture { get; set; }
        public string Type { get; set; }
        public string Group { get; set; }
    }
}
