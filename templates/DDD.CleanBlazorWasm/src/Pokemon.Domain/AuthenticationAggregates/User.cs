using Pokemon.Domain.Common.Models;
using Pokemon.Domain.AuthenticationAggregates.Events;
using Pokemon.Domain.AuthenticationAggregates.ValueObjects;

namespace Pokemon.Domain.AuthenticationAggregates;

public sealed class User : AggregateRoot<UserId, Guid>, IAuditableEntity
{
    private readonly List<UserId> _friendIds = new();
    public string Username { get; private set; }
    public string Firstname { get; private set; }
    public string Lastname { get; private set; }
    public string Email { get; private set; }
    public string HashedPassword { get; private set; }
    public RefreshTokenId? RefreshTokenId { get; private set; }
    public IReadOnlyList<UserId> FriendIds => _friendIds.AsReadOnly();

    public DateTime CreatedOnUtc { get; set; }
    public DateTime ModifiedOnUtc { get; set; }

    private User(
        string username,
        string firstname,
        string lastname,
        string email,
        string hashedPassword,
        UserId userId = null!) : base(userId ?? UserId.CreateUnique())
    {
        Username = username;
        Firstname = firstname;
        Lastname = lastname;
        Email = email;
        HashedPassword = hashedPassword;
    }


    /// <summary>
    /// Method for creating a new User object
    /// </summary>
    /// <param name="username"></param>
    /// <param name="firstname"></param>
    /// <param name="lastname"></param>
    /// <param name="email"></param>
    /// <param name="hashedPassword"></param>
    /// <returns>A User object</returns>
    public static User Create(
        string username,
        string firstname,
        string lastname,
        string email,
        string hashedPassword)
    {
        var user = new User(
            username,
            firstname,
            lastname,
            email,
            hashedPassword);

        user.AddDomainEvent(new UserCreated(user));

        return user;
    }


    /// <summary>
    /// Method for adding a user to another users friends list
    /// </summary>
    /// <param name="friendId"></param>
    public void AddFriendId(UserId friendId)
    {
        _friendIds.Add(friendId);
    }


    /// <summary>
    ///  Method for removing a user from another users friends list
    /// </summary>
    /// <param name="friendId"></param>
    public void RemoveFriendId(UserId friendId)
    {
        _friendIds.Remove(friendId);
    }

    /// <summary>
    ///  Method for updateing the refresh token for the user
    /// </summary>
    /// <param name="refreshTokenId"></param>
    public void UpdateRefreshToken(RefreshTokenId refreshTokenId)
    {
        RefreshTokenId = refreshTokenId;
    }

#pragma warning disable CS8618
    private User()
    {
    }
#pragma warning restore CS8618
}