using System.Data.Common;
using System.Numerics;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pokemon.Domain.AuthenticationAggregates;
using Pokemon.Domain.AuthenticationAggregates.ValueObjects;

namespace Pokemon.Infrastructure.Persistence.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{

    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUsersTable(builder);
        ConfigureUserRoleClaimIdsTable(builder);
    }

    private static void ConfigureUserRoleClaimIdsTable(EntityTypeBuilder<User> builder)
    {
        builder.OwnsMany(c => c.RoleClaimIds, rc =>
        {
            rc.ToTable("UserRoleClaimIds");

            rc.WithOwner().HasForeignKey("UserId");

            rc.HasKey("Id");

            rc.Property(i => i.Value)
                .HasColumnName("UserRoleClaimId")
                .ValueGeneratedNever();
        });

        builder.Metadata.FindNavigation(nameof(User.RoleClaimIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureUsersTable(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                Value => UserId.Create(Value));

        builder.Property(u => u.RefreshTokenId!)
            .HasColumnName("RefreshTokenId")
            .HasConversion(
                id => id.Value,
                value => RefreshTokenId.Create(value));

        builder.OwnsOne(u => u.PersonalData, pd =>
        {
            pd.ToTable("UserPersonalData");

            pd.HasKey(pd => pd.Id);

            pd.Property(pd => pd.Id)
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    Value => PersonalDataId.Create(Value));

        });
    }
}
