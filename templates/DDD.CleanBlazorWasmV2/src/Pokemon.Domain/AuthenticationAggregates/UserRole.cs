using Pokemon.Domain.AuthenticationAggregates.ValueObjects;
using Pokemon.Domain.Common.Models;

namespace Pokemon.Domain.AuthenticationAggregates;

public sealed class UserRole : AggregateRoot<UserRoleId, Guid>, IAuditableEntity
{
    public UserId UserId { get; private set; }
    public string Role { get; private set; }

    public DateTime CreatedOnUtc { get; set; }
    public DateTime ModifiedOnUtc { get; set; }

    private UserRole(
        UserId userId,
        string role,
        UserRoleId userRoleId = null!) : base(userRoleId ?? UserRoleId.CreateUnique())
    {
        UserId = userId;
        Role = role;
    }


    public static UserRole Create(
        UserId userId,
        string role)
    {
        var userRole = new UserRole(
            userId,
            role
        );

        return userRole;
    }

#pragma warning disable CS8618
    private UserRole()
    {
    }
#pragma warning restore CS8618
}
