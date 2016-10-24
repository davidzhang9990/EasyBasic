using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XDDEasy.Domain.AccountAggregates;

namespace XDDEasy.Domain.Configuration
{
    public class UserModelConfig : EntityTypeConfiguration<User>
    {
        public UserModelConfig()
        {
            ToTable("Users");
            HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("idx_User_UserName") { IsUnique = true }));

            Property(u => u.Email).HasMaxLength(256);

            //HasMany(c => c.UserProfiles)
            // .WithRequired(x => x.User)
            //.HasForeignKey(x => x.UserId);
        }
    }
}
