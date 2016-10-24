using System;
using System.ComponentModel.DataAnnotations;

namespace Common.EntityFramework.Model
{
    public interface IAuditEntity
    {
        [Required]
        DateTime DateAdded { get; set; }
        [Required]
        DateTime DateUpdated { get; set; }
        [Required]
        Guid AddedBy { get; set; }
        [Required]
        Guid UpdatedBy { get; set; }
    }
    public class AuditEntity : IAuditEntity
    {
        [Required]
        public DateTime DateAdded { get; set; }
        [Required]
        public DateTime DateUpdated { get; set; }
        [Required]
        public Guid AddedBy { get; set; }
        [Required]
        public Guid UpdatedBy { get; set; }
    }
}
