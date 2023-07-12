using System.Data.Common;
using System.Numerics;
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pokemon.Domain.UserAggregate;
using Pokemon.Domain.UserAggregate.ValueObjects;

namespace Pokemon.Infrastructure.Persistence.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{

    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUsersTable(builder);
        ConfigureFriendIdsTable(builder);
    }

    private static void ConfigureFriendIdsTable(EntityTypeBuilder<User> builder)
    {
        builder.OwnsMany(f => f.FriendIds, fb =>
        {
            fb.ToTable("FriendIds");

            fb.WithOwner().HasForeignKey("UserId");

            fb.HasKey("Id");

            fb.Property(fib => fib.Value)
                .ValueGeneratedNever()
                .HasColumnName("FriendId");
        });

        builder.Metadata.FindNavigation(nameof(User.FriendIds))!
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
    }
}
