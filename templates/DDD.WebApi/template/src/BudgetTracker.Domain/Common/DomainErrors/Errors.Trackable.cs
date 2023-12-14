using System.Runtime.InteropServices.JavaScript;
using ErrorOr;

namespace BudgetTracker.Domain.Common.DomainErrors;

public static partial class Errors
{
    public static class Trackable
    {
        public static Error NotFound => Error.NotFound(
            code: "Trackable.NotFound",
            description: "The trackable was not found.");

        public static Error AlreadyExists => Error.Conflict(
            code: "Trackable.AlreadyExists",
            description: "Trackable already exists in the system.");

        public static Error ValidationError => Error.Validation(
            code: "Trackable.Validation",
            description: "The validation of the trackable failed.");
    }
}