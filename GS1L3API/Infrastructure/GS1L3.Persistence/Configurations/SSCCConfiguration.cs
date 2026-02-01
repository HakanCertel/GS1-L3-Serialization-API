using GS1L3.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS1L3.Persistence.Configurations
{
    public class SSCCConfiguration : IEntityTypeConfiguration<SSCC>
    {
        public void Configure(EntityTypeBuilder<SSCC> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.SSCCCode).IsRequired().HasMaxLength(18);
            builder.HasIndex(x => x.SSCCCode).IsUnique();

            builder.HasOne(x => x.ParentSSCC)
                   .WithMany(x => x.ChildSSCCs)
                   .HasForeignKey(x => x.ParentSSCCId)
                   .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
