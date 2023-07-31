using Pokemon.Domain.Common.Models;
using Pokemon.Domain.AuthenticationAggregates.Events;
using Pokemon.Domain.AuthenticationAggregates.ValueObjects;
using Pokemon.Domain.AuthenticationAggregates.Entities;
using System.Diagnostics.Contracts;

namespace Pokemon.Domain.AuthenticationAggregates;

public sealed class User : AggregateRoot<UserId, Guid>, IAuditableEntity
{
    private readonly List<UserRoleClaimId> _roleClaimIds = new();

    public string Username { get; private set; }
    public string Firstname { get; private set; }
    public string Lastname { get; private set; }
    public string Email { get; private set; }
    public string HashedPassword { get; private set; }
    public RefreshTokenId? RefreshTokenId { get; private set; }
    public PersonalData? PersonalData { get; private set; }
    public bool EmailConfirmed { get; private set; } = false;
    public bool TwoFactorEnabled { get; private set; } = false;
    public bool UserLockoutEnabled { get; private set; } = false;
    public bool LockedOut { get; private set; } = false;
    public int FailedLogins { get; private set; }
    public DateTime? LockoutEnd { get; private set; }

    public IReadOnlyList<UserRoleClaimId> RoleClaimIds => _roleClaimIds.AsReadOnly();

    public DateTime CreatedOnUtc { get; set; }
    public DateTime ModifiedOnUtc { get; set; }

    private User(
        string username,
        string firstname,
        string lastname,
        string email,
        string hashedPassword,
        PersonalData? personalData,
        List<UserRoleClaimId> roleClaimIds,
        UserId userId = null!) : base(userId ?? UserId.CreateUnique())
    {
        Username = username;
        Firstname = firstname;
        Lastname = lastname;
        Email = email;
        HashedPassword = hashedPassword;
        PersonalData = personalData;
        _roleClaimIds = roleClaimIds;
    }


    /// <summary>
    /// Method for creating a new User object
    /// </summary>
    /// <param name="username"></param>
    /// <param name="firstname"></param>
    /// <param name="lastname"></param>
    /// <param name="email"></param>
    /// <param name="hashedPassword"></param>
    /// <param name="personalData"></param>
    /// <returns>A User object</returns>
    public static User Create(
        string username,
        string firstname,
        string lastname,
        string email,
        string hashedPassword,
        List<UserRoleClaimId> roleClaimIds,
        PersonalData? personalData
        )
    {
        var user = new User(
            username,
            firstname,
            lastname,
            email,
            hashedPassword,
            personalData,
            roleClaimIds ?? new());

        user.AddDomainEvent(new UserCreated(user));

        return user;
    }

    /// <summary>
    ///  Method for updateing the refresh token for the user
    /// </summary>
    /// <param name="refreshTokenId"></param>
    public void UpdateRefreshToken(RefreshTokenId refreshTokenId)
    {
        RefreshTokenId = refreshTokenId;
    }


    public void AddRoleClaim(UserRoleClaimId roleClaimId)
    {
        _roleClaimIds.Add(roleClaimId);
    }


    public void RemoveRoleClaim(UserRoleClaimId roleClaimId)
    {
        _roleClaimIds.Remove(roleClaimId);
    }


    public void HandleFailedUserLoginAttempts()
    {
        const int LockoutAttempts = 5;
        const int LockoutMinutes = 1;


        if (UserLockoutEnabled)
        {
            FailedLogins ++;
        }

        if (UserLockoutEnabled && FailedLogins >= LockoutAttempts)
        {
            LockedOut = true;
            LockoutEnd = DateTime.UtcNow.AddMinutes(LockoutMinutes);
        }
    }


    public void UnlockUser()
    {
        LockedOut = false;
        LockoutEnd = null;
        FailedLogins = 0;
    }


    public void CheckUserLockoutState()
    {
        var currentTime = DateTime.UtcNow;

        HandleFailedUserLoginAttempts();

        if (UserLockoutEnabled && LockedOut)
        {
            if(LockoutEnd < currentTime)
            {
                UnlockUser();
            }
        }
    }



#pragma warning disable CS8618
    private User()
    {
    }
#pragma warning restore CS8618
}