using BaseTemplate.Shared.Dtos.AuditDtos;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseTemplate.Repository.Elastic.Abstraction
{
    public interface IAuditService
    {
        void SetCommonValues(ChangeTracker changeTracker);

        Task HandleAuditLogs(ChangeTracker changeTracker);
        List<AuditEntry> GetAuditEntries(ChangeTracker changeTracker);
        AuditEntry CreateAuditEntry(EntityEntry entry);

    }
}
