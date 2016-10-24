using System.ComponentModel.DataAnnotations.Schema;
using Common.EntityFramework.Model;

namespace XDDEasy.Domain.EmailAggregates
{
    [Table("EmailTemplate")]
    public class EmailTemplate : ActiveEntity
    {
        public string TemplateCulture { get; set; }

        public string TemplateTitle { get; set; }

        public string TemplateType { get; set; }

        public string TemplateValue { get; set; }
    }
}
