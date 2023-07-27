using ErrorOr;

namespace Pokemon.Domain.Common.DomainErrors;

public static partial class Errors
{
    public static class PersonalData
    {
        public static Error InvalidPersonalDataId => Error.Validation(
            code: "PersonalDataId.Invalid",
            description: "PersonalData Id is invalid.");

        public static Error InvalidPersonalData => Error.Validation(
            code: "PersonalData.Invalid",
            description: "PersonalData is invalid");

        public static Error NotFound => Error.NotFound(
            code: "PersonalData.NotFound",
            description: "PersonalData not found.");
    }
}