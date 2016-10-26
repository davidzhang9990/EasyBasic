using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.EntityFramework.Model;

namespace XDDEasy.Domain.PageAggregates
{
    [Table("Pages")]
    public class Page : ActiveEntity
    {
        public string Name { get; set; }
        public string ActionName { get; set; }
        public string ControlName { get; set; }
        public string Area { get; set; }
        public string ShowPage { get; set; }
        public string Description { get; set; }
    }
}
