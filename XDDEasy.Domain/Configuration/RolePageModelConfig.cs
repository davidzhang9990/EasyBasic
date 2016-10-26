using System;
using System.Data.Entity.ModelConfiguration;
using XDDEasy.Domain.PageAggregates;

namespace XDDEasy.Domain.Configuration
{
    public class RolePageModelConfig : EntityTypeConfiguration<RolePage>
    {
        public RolePageModelConfig()
        {
            HasKey(c => new { c.RoleId, c.PageId });
        }
    }
}
