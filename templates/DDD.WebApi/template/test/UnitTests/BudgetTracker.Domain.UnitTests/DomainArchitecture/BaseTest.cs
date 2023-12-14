using System.Reflection;
using BudgetTracker.Domain.Common.Models;

namespace BudgetTracker.Domain.UnitTests.DomainArchitecture;

public abstract class BaseTest
{
    public static readonly Assembly DomainAssembly = typeof(AggregateRoot<,>).Assembly;
}