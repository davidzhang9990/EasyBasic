using System;
using System.ComponentModel.DataAnnotations.Schema;
using Common.EntityFramework.Model;

namespace XDDEasy.Domain.LogAggregates
{
    [Table("Log")]
    public class Log : AuditEntity
    {
        public Log()
        {
            Id = Guid.NewGuid();
        }

        [System.ComponentModel.DataAnnotations.Key]
        public Guid Id { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public string UserId { get; set; }
        public string UserAgent { get; set; }
        public string Address { get; set; }
        public string Property { get; set; }
    }
}
