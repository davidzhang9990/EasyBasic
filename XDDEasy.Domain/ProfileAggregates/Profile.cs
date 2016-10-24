using Common.EntityFramework.Model;
using System.ComponentModel.DataAnnotations.Schema;

namespace XDDEasy.Domain.ProfileAggregates
{
    [Table("Profile")]
    public class Profile : ActiveEntity
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
