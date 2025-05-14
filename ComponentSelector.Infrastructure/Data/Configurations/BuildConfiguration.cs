using ComponentSelector.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComponentSelector.Infrastructure.Data.Configurations;

public class BuildConfiguration : IEntityTypeConfiguration<Build>
{
    public void Configure(EntityTypeBuilder<Build> builder)
    {
        builder.HasOne(b => b.CPU)
            .WithMany()
            .HasForeignKey(b => b.CPUId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.Motherboard)
            .WithMany()
            .HasForeignKey(b => b.MotherboardId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.RAM)
            .WithMany()
            .HasForeignKey(b => b.RAMId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.Storage)
            .WithMany()
            .HasForeignKey(b => b.StorageId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.GPU)
            .WithMany()
            .HasForeignKey(b => b.GPUId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.PSU)
            .WithMany()
            .HasForeignKey(b => b.PSUId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(b => b.Case)
            .WithMany()
            .HasForeignKey(b => b.CaseId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
