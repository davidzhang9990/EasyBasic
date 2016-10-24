using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;
using Common.EntityFramework.Model;
using Common.Models.Enum;

namespace XDDEasy.Domain.EmailAggregates
{
    [Table("Email")]
    public class Email : Entity
    {
        protected Email()
        {
            EmailType = EmailType.RegistrationEmail;
        }

        [Required]
        public string From { get; set; }

        [Required]
        public string To { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public EmailType EmailType { get; set; }

        public Guid? SenderId { get; set; }

        public Guid SchoolId { get; set; }
    }
}
