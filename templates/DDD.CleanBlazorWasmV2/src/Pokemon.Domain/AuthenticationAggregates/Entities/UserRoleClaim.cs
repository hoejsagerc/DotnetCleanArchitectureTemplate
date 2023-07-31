using Pokemon.Domain.AuthenticationAggregates.ValueObjects;
using Pokemon.Domain.Common.Models;

namespace Pokemon.Domain.AuthenticationAggregates.Entities;

public sealed class UserRoleClaim : Entity<UserRoleClaimId>, IAuditableEntity
{
    public UserRoleId UserRoleId { get; private set; }

    public DateTime CreatedOnUtc { get; set; }
    public DateTime ModifiedOnUtc { get; set; }


    private UserRoleClaim(
        UserRoleId userRoleId,
        UserRoleClaimId? id = null) : base(id ?? UserRoleClaimId.CreateUnique())
    {
        UserRoleId = userRoleId;
    }


    public static UserRoleClaim Create(
        UserRoleId userRoleId)
    {
        var userRoleClaim = new UserRoleClaim(
            userRoleId);

        return userRoleClaim;
    }


#pragma warning disable CS8618
    private UserRoleClaim()
    {
    }
#pragma warning restore CS8618
}
