using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pokemon.Domain.AuthenticationAggregates;
using Pokemon.Domain.AuthenticationAggregates.ValueObjects;

namespace Pokemon.Infrastructure.Persistence.Configurations;

public class RefreshTokenConfigurations : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                Value => RefreshTokenId.Create(Value));

        builder.Property(r => r.UserId)
            .HasColumnName("UserId")
            .HasConversion(
                id => id.Value,
                Value => UserId.Create(Value));
    }
}
