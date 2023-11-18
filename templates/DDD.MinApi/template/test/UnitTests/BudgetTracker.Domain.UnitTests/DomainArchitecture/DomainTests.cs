using BudgetTracker.Domain.Common.Models;
using FluentAssertions;
using NetArchTest.Rules;
using BindingFlags = System.Reflection.BindingFlags;

namespace BudgetTracker.Domain.UnitTests.DomainArchitecture;

public class DomainTests : BaseTest
{
    [Fact]
    public void DomainEvents_Should_BeSealed()
    {
        var result = Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .BeSealed()
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void DomainEvents_Should_HaveDomainEventPostFix()
    {
        var result = Types.InAssembly(DomainAssembly)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .Should()
            .HaveNameEndingWith("DomainEvent")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Aggregates_Should_HavePrivateParameterlessConstructor()
    {
        var aggregateTypes = Types.InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(AggregateRoot<,>))
            .GetTypes();

        var failingTypes = new List<Type>();
        foreach (var aggregateType in aggregateTypes)
        {
            var constructors = aggregateType.GetConstructors(
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (!constructors.Any(c => c.IsPrivate && c.GetParameters().Length == 0))
            {
                failingTypes.Add(aggregateType);
            }

            failingTypes.Should().BeEmpty();
        }
    }
}