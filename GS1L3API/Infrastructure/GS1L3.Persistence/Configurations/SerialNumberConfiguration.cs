using GS1L3.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS1L3.Persistence.Configurations
{
    public class SerialNumberConfiguration : IEntityTypeConfiguration<SerialNumber>
    {
        public void Configure(EntityTypeBuilder<SerialNumber> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.SN).IsRequired().HasMaxLength(20);
            builder.HasIndex(x => x.SN).IsUnique();

            builder.HasOne(x => x.WorkOrder)
                   .WithMany(x => x.SerialNumbers)
                   .HasForeignKey(x => x.WorkOrderId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SSCC)
                   .WithMany(x => x.SerialNumbers)
                   .HasForeignKey(x => x.SSCCId)
                   .OnDelete(DeleteBehavior.SetNull); 
        }
    }
}
