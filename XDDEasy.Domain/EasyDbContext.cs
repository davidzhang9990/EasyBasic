using Common.EntityFramework.Config;
using Common.EntityFramework.Model;
using Common.Models;
using log4net;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XDDEasy.Domain.AccountAggregates;
using XDDEasy.Domain.Configuration;
using XDDEasy.Domain.EmailAggregates;
using XDDEasy.Domain.LogAggregates;
using XDDEasy.Domain.PageAggregates;
using XDDEasy.Domain.ProfileAggregates;
using XDDEasy.Domain.ResourceAggregates;
using XDDEasy.Domain.UserLogger;

namespace XDDEasy.Domain
{
    [DbConfigurationType(typeof(EfDbConfiguration))]
    public class EasyDbContext : IdentityDbContext<User>
    {
        public EasyDbContext()
            : this("Name=XDDEasyContext")
        {
        }

        public EasyDbContext(string contextName)
            : base(contextName)
        {
        }

        public static EasyDbContext Create()
        {
            var context = new EasyDbContext();
            var logger = LogManager.GetLogger("EntityFramework.SQL");
            DbLogFormatter.Logger = logger;
            context.Database.Log = logger.Debug;
            //context.EnableFilter("ActiveFlag");
            DbLogFormatter.Logger.Debug("EasyDbContext has been created.");
            return context;
        }

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    DbLogFormatter.Logger.Debug("EasyDbContext has been disposed.");
        //}

        public RequestContext RequestContext { get; set; }

        public DbSet<Profile> Profiles { get; set; }

        public DbSet<UserLog> UserLogs { get; set; }

        public DbSet<Log> Logs { get; set; }
        public DbSet<Resource> Resources { get; set; }

        public DbSet<Page> Pages { get; set; }
        public DbSet<RolePage> RolePages { get; set; }

        public DbSet<Email> Emails { get; set; }

        public DbSet<EmailTemplate> EmailTemplates { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new UserModelConfig());

            modelBuilder.Configurations.Add(new RolePageModelConfig());

            modelBuilder.Entity<IdentityUser>().ToTable("Users");
            modelBuilder.Entity<User>().ToTable("Users");

            #region config identity
            var role = modelBuilder.Entity<IdentityRole>().ToTable("Role");
            role.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("idx_Role_Name") { IsUnique = true }));
            role.HasMany(r => r.Users).WithRequired().HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.UserId, r.RoleId }).ToTable("UserRole");

            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim");

            modelBuilder.Entity<IdentityUserLogin>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey, l.UserId })
                .ToTable("UserLogin");

            #endregion
        }

        public override int SaveChanges()
        {
            UpdateProperties();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            try
            {
                UpdateProperties();
                return base.SaveChangesAsync(cancellationToken);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    DbLogFormatter.Logger.ErrorFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        DbLogFormatter.Logger.ErrorFormat("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        protected void UpdateProperties()
        {
            foreach (var auditableEntity in ChangeTracker.Entries<IAuditEntity>())
            {
                if (auditableEntity.State == EntityState.Added ||
                    auditableEntity.State == EntityState.Modified)
                {
                    var userId = RequestContext.UserId;
                    if (userId == Guid.Empty)
                    {
                        var context = RequestContext.GetFromCallContext();
                        if (context != null)
                            userId = context.UserId;
                    }
                    auditableEntity.Entity.DateUpdated = DateTime.UtcNow;
                    auditableEntity.Entity.UpdatedBy = userId;

                    // pupulate created date and created by columns for
                    // newly added record.
                    if (auditableEntity.State == EntityState.Added)
                    {
                        auditableEntity.Entity.DateAdded = DateTime.UtcNow;
                        auditableEntity.Entity.AddedBy = userId;
                    }
                    else
                    {
                        // we also want to make sure that code is not inadvertly
                        // modifying created date and created by columns 
                        auditableEntity.Property(p => p.DateAdded).IsModified = false;
                        auditableEntity.Property(p => p.AddedBy).IsModified = false;
                    }
                }
            }
            //foreach (var dbEntityEntry in ChangeTracker.Entries())
            //{
            //    if (dbEntityEntry.State == EntityState.Added
            //        && dbEntityEntry.CurrentValues.PropertyNames.Any(x => x.Contains("SchoolId"))
            //        && RequestContext.SchoolId != Guid.Empty
            //        && dbEntityEntry.Property("SchoolId").CurrentValue == null)
            //    {
            //        var schoolId = RequestContext.SchoolId;
            //        if (schoolId == Guid.Empty)
            //        {
            //            var context = RequestContext.GetFromCallContext();
            //            if (context != null)
            //                schoolId = context.SchoolId;
            //        }
            //        dbEntityEntry.Property("SchoolId").CurrentValue = schoolId;
            //    }
            //}
        }
    }
}
