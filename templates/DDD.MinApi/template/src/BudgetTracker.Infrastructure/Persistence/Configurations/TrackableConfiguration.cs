using BudgetTracker.Domain.TrackableAggregate;
using BudgetTracker.Domain.TrackableAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BudgetTracker.Infrastructure.Persistence.Configurations;

public class TrackableConfiguration : IEntityTypeConfiguration<TrackableAggregate>
{
    public void Configure(EntityTypeBuilder<TrackableAggregate> builder)
    {
        ConfigureTrackablesTable(builder);
    }

    private void ConfigureTrackablesTable(EntityTypeBuilder<TrackableAggregate> builder)
    {
        builder.ToTable("Trackables");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => TrackableId.Create(value));
    }
}