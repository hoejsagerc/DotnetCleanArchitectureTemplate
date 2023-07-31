using Pokemon.Domain.AuthenticationAggregates.ValueObjects;
using Pokemon.Domain.Common.Models;

namespace Pokemon.Domain.AuthenticationAggregates.Entities;

public sealed class PersonalData : Entity<PersonalDataId>, IAuditableEntity
{
    public string? Country { get; private set; }
    public string? StreetAddress { get; private set; }
    public int? ZipCode { get; private set; }
    public string? Gender { get; private set; }
    public string? PhoneCountryCode { get; private set; }
    public string? PhoneNumber { get; private set; }
    public DateTime? BirthDay { get; private set; }

    public DateTime CreatedOnUtc { get; set; }
    public DateTime ModifiedOnUtc { get; set; }

    private PersonalData(
        string? country,
        string? streetAddress,
        int? zipCode,
        string? sex,
        string? phoneCountryCode,
        string? phoneNumber,
        DateTime? birthday,
        PersonalDataId? id = null) : base(id ?? PersonalDataId.CreateUnique())
    {
        Country = country;
        StreetAddress = streetAddress;
        ZipCode = zipCode;
        Gender = sex;
        PhoneCountryCode = phoneCountryCode;
        PhoneNumber = phoneNumber;
        BirthDay = birthday;
    }

    public static PersonalData Create(
        string? country,
        string? streetAddress,
        int? zipCode,
        string? sex,
        string? phoneCountryCode,
        string? phoneNumber,
        DateTime? birthDay)
    {
        var data = new PersonalData(
            country,
            streetAddress,
            zipCode,
            sex,
            phoneCountryCode,
            phoneNumber,
            birthDay);

        return data;
    }

    private PersonalData()
    {
    }
}
