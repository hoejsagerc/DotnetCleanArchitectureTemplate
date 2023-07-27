using ErrorOr;
using Pokemon.Domain.Common.Models.Identities;
using Pokemon.Domain.Common.DomainErrors;

namespace Pokemon.Domain.AuthenticationAggregates.ValueObjects;

public sealed class PersonalDataId : AggregateRootId<Guid>
{
    public PersonalDataId(Guid value) : base(value)
    {

    }

    public static PersonalDataId CreateUnique()
    {
        return new PersonalDataId(Guid.NewGuid());
    }

    public static PersonalDataId Create(Guid personalDataId)
    {
        return new PersonalDataId(personalDataId);
    }

    public static ErrorOr<PersonalDataId> Create(string personalDataId)
    {
        if (!Guid.TryParse(personalDataId, out var guid))
        {
            return Errors.PersonalData.InvalidPersonalDataId;
        }

        return new PersonalDataId(guid);
    }
}