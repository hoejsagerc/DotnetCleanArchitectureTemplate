using System.Reflection.Metadata;
using Pokemon.Domain.AuthenticationAggregates.ValueObjects;

namespace Pokemon.Domain.AuthenticationAggregates.Entities;

public sealed class PersonalData : Entity<PersonalDataId>
{
    public string? Country { get; private set; }
    public string? StreetAddress { get; private set; }
    public int? ZipCode { get; private set; }
    public string? Sex { get; private set; }
    public string? PhoneCountryCode { get; private set; }
    public string? PhoneNumber { get; private set; }
    public DateTime? BirthDay { get; private set; }

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
        Sex = sex;
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
}
