using GS1L3.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GS1L3.Persistence.Context
{
    public class GS1L3DbContext:DbContext
    {
        public GS1L3DbContext(DbContextOptions<GS1L3DbContext> options) : base(options){}

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<SerialNumber> SerialNumbers { get; set; }
        public DbSet<SSCC> SSCCs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Mevcut assembly içindeki tüm IEntityTypeConfiguration arayüzlerini otomatik uygular
            builder.ApplyConfigurationsFromAssembly(typeof(GS1L3DbContext).Assembly);

            // Global Query Filter: Silinmiş (Soft Delete) kayıtları otomatik gizler
            builder.Entity<Customer>().HasQueryFilter(c => !c.IsDeleted);

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var datas = ChangeTracker.Entries<BaseEntity>();
            foreach (var data in datas)
            {
                switch (data.State)
                {
                    case EntityState.Modified:
                        data.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Added:
                        data.Entity.CreatedAt = DateTime.UtcNow;
                        data.Entity.IsActive = true;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);

        }

    }
}
