using BaseTemplate.Domain.Entities;
using BaseTemplate.Repository.Elastic.Abstraction;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BaseTemplate.Repository.Contexts
{
    public class ExampleContext : DbContext
    {
        private readonly IAuditService _auditService;

        public ExampleContext(DbContextOptions<ExampleContext> options, IAuditService auditService)
            : base(options)
        {
            _auditService = auditService;
        }

        public DbSet<Example> Examples { get; set; }
        public DbSet<User> Users { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public async override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            await _auditService.HandleAuditLogs(ChangeTracker);
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }

}
