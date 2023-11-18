namespace BudgetTracker.Domain.Common.Models;

public interface IAuditableEntity
{
    DateTime CreatedOnUtc { get; set; }
    DateTime ModifiedOnUtc { get; set; }
}