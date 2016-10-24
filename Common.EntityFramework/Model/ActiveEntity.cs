using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Common.EntityFramework.Model
{
    public abstract class ActiveEntity : Entity
    {
        protected ActiveEntity()
        {
            ActiveFlag = true;
        }

        [Required]
        [DefaultValue(true)]
        public bool ActiveFlag { get; set; }
    }
}
