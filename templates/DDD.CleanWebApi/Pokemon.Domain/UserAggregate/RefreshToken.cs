using System.Security.Cryptography;
using Pokemon.Domain.Common.Models;
using Pokemon.Domain.UserAggregate.Events;
using Pokemon.Domain.UserAggregate.ValueObjects;

namespace Pokemon.Domain.UserAggregate;

public sealed class RefreshToken : Entity<RefreshTokenId>, IAuditableEntity
{
    public string Token { get; private set; }
    public DateTime Expires { get; private set; }
    public UserId UserId { get; private set; }
    public bool Revoked { get; private set; }

    public DateTime CreatedOnUtc { get; set; }
    public DateTime ModifiedOnUtc { get; set; }


    private RefreshToken(
        string token,
        DateTime expires,
        UserId userId,
        bool revoked,
        RefreshTokenId? id = null) : base(id ?? RefreshTokenId.CreateUnique())
    {
        Token = token;
        Expires = expires;
        UserId = userId;
        Revoked = revoked;
    }

    public static RefreshToken Create(DateTime expires, UserId userId)
    {
        var tokenSignature = Convert.ToBase64String(
            RandomNumberGenerator.GetBytes(64));

        var token = new RefreshToken(
            token: tokenSignature,
            expires: expires,
            userId: userId,
            revoked: false);

        token.AddDomainEvent(new RefreshTokenCreated(token));

        return token;
    }

    public void Revoke()
    {
        Revoked = true;
    }
}