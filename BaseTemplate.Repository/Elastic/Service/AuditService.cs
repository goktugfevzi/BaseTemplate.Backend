using BaseTemplate.Domain.Entities;
using BaseTemplate.Domain.Entities.Common;
using BaseTemplate.Repository.Elastic.Abstraction;
using BaseTemplate.Shared.Abstractions;
using BaseTemplate.Shared.Dtos.AuditDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BaseTemplate.Repository.Elastic.Service
{
    public class AuditService : IAuditService
    {
        private readonly IUserClaimService _userClaimService;
        private readonly IElasticService _elasticService;

        public AuditService(IUserClaimService userClaimService, IElasticService elasticService)
        {
            _userClaimService = userClaimService;
            _elasticService = elasticService;
        }

        public async Task HandleAuditLogs(ChangeTracker changeTracker)
        {
            changeTracker.DetectChanges();
            SetCommonValues(changeTracker);

            var auditEntries = GetAuditEntries(changeTracker);
            foreach (var auditEntry in auditEntries)
            {
                await _elasticService.AddOrUpdateAudit(auditEntry.ToAudit());
            }
        }

        public List<AuditEntry> GetAuditEntries(ChangeTracker changeTracker)
        {
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in changeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged) continue;

                var auditEntry = CreateAuditEntry(entry);
                auditEntries.Add(auditEntry);
            }
            return auditEntries;
        }

        public AuditEntry CreateAuditEntry(EntityEntry entry)
        {
            var auditEntry = new AuditEntry(entry);
            auditEntry.TableName = entry.Entity.GetType().Name;
            auditEntry.IpAddress = _userClaimService.GetCurrentUserIpAddress() ?? "Kullanıcı bulunamadı";

            foreach (var property in entry.Properties)
            {
                string propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.AuditType = AuditType.Create;
                        auditEntry.NewValues[propertyName] = property.CurrentValue;
                        auditEntry.UserId = (Guid)(entry.Property("CreatedBy").CurrentValue ?? Guid.Empty);
                        break;
                    case EntityState.Deleted:
                        auditEntry.AuditType = AuditType.Delete;
                        auditEntry.OldValues[propertyName] = property.OriginalValue;
                        auditEntry.UserId = (Guid)(entry.Property("UpdatedBy").CurrentValue ?? Guid.Empty);
                        break;
                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            auditEntry.ChangedColumns.Add(propertyName);
                            auditEntry.AuditType = AuditType.Update;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            auditEntry.UserId = (Guid)(entry.Property("UpdatedBy").CurrentValue ?? Guid.Empty);
                        }
                        break;
                }
            }

            return auditEntry;
        }

        public void SetCommonValues(ChangeTracker changeTracker)
        {
            var datas = changeTracker.Entries<BaseEntity>();
            foreach (var data in datas)
            {
                switch (data.State)
                {
                    case EntityState.Modified:
                        data.Entity.UpdatedDate = DateTime.UtcNow;
                        data.Entity.UpdatedBy = _userClaimService.GetCurrentUserId() != Guid.Empty ? _userClaimService.GetCurrentUserId() : Guid.Empty;


                        break;
                    case EntityState.Added:
                        data.Entity.CreatedBy = _userClaimService.GetCurrentUserId() != Guid.Empty ? _userClaimService.GetCurrentUserId() : Guid.Empty;
                        data.Entity.CreatedDate = DateTime.UtcNow;
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
