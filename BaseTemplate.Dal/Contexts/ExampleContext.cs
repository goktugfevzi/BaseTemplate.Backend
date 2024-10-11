using BaseTemplate.Schema.Entities;
using BaseTemplate.Schema.Entities.Common;
using BaseTemplate.Shared.Abstractions;
using BaseTemplate.Shared.Dtos.AuditDtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Dal.Contexts
{
    public class ExampleContext : DbContext
    {
        private readonly IUserClaimService userClaimService;

        public ExampleContext(DbContextOptions<ExampleContext> options) : base(options) { }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<Example> Examples { get; set; }
        public DbSet<User> Users { get; set; }

        private async Task BeforeSaveChanges()
        {
            ChangeTracker.DetectChanges();
            SetCommonValues();
            //var auditEntries = new List<AuditEntry>();
            //foreach (var entry in ChangeTracker.Entries())
            //{
            //    if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
            //        continue;
            //    var auditEntry = new AuditEntry(entry);
            //    auditEntry.TableName = entry.Entity.GetType().Name;
            //    auditEntry.IpAdress = userClaimService.GetCurrentUserIpAddress();
            //    auditEntries.Add(auditEntry);
            //    foreach (var property in entry.Properties)
            //    {
            //        string propertyName = property.Metadata.Name;
            //        if (property.Metadata.IsPrimaryKey())
            //        {
            //            auditEntry.KeyValues[propertyName] = property.CurrentValue;
            //            continue;
            //        }
            //        switch (entry.State)
            //        {
            //            case EntityState.Added:
            //                auditEntry.AuditType = AuditType.Create;
            //                auditEntry.NewValues[propertyName] = property.CurrentValue;
            //                auditEntry.UserId = (Guid)(entry.Property("CreatedBy").CurrentValue != null ? entry.Property("CreatedBy").CurrentValue : Guid.Empty);
            //                break;
            //            case EntityState.Deleted:
            //                auditEntry.AuditType = AuditType.Delete;
            //                auditEntry.OldValues[propertyName] = property.OriginalValue;
            //                auditEntry.UserId = (Guid)(entry.Property("UpdatedBy").CurrentValue != null ? entry.Property("UpdatedBy").CurrentValue : Guid.Empty);
            //                break;
            //            case EntityState.Modified:
            //                if (property.IsModified)
            //                {
            //                    auditEntry.ChangedColumns.Add(propertyName);
            //                    auditEntry.AuditType = AuditType.Update;
            //                    auditEntry.OldValues[propertyName] = property.OriginalValue;
            //                    auditEntry.NewValues[propertyName] = property.CurrentValue;
            //                    auditEntry.UserId = (Guid)(entry.Property("UpdatedBy").CurrentValue != null ? entry.Property("UpdatedBy").CurrentValue : Guid.Empty);
            //                }
            //                break;
            //        }
            //    }
            //}
            //foreach (var auditEntry in auditEntries)
            //{
            //    Audits.Add(auditEntry.ToAudit());
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public void SetCommonValues(CancellationToken token = default)
        {
            var datas = ChangeTracker.Entries<BaseEntity>();
            foreach (var data in datas)
            {
                switch (data.State)
                {
                    case EntityState.Modified:
                        //data.Entity.UpdatedBy = userClaimService.GetCurrentUserId();
                        data.Entity.UpdatedDate = DateTime.UtcNow;
                        break;
                    case EntityState.Added:
                        //data.Entity.CreatedBy = userClaimService.GetCurrentUserId();
                        data.Entity.CreatedDate = DateTime.UtcNow;
                        break;
                    default:
                        break;
                }
            }
        }
        public async override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            await BeforeSaveChanges();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
