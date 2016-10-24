using System;
using System.ComponentModel.DataAnnotations.Schema;
using Common.EntityFramework.Model;

namespace XDDEasy.Domain.UserLogger
{
    [Table("UserLog")]
    public class UserLog : Entity
    {
        public string UserId { get; set; }
        public string Path { get; set; }
        public string UserAgent { get; set; }
        public string Content { get; set; }
        public string Host { get; set; }
        public string Ip { get; set; }

    }
}
