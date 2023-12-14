using FluentAssertions;
using NetArchTest.Rules;

namespace BudgetTracker.Domain.UnitTests.DomainArchitecture;

public class ArchitectureTests : BaseTest
{
    [Fact]
    public void Domain_Should_NotHaveDependencyOnApplication()
    {
        var result =Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn("BudgetTracker.Application")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Domain_Should_NotHaveDependencyOnInfrastructure()
    {
        var result =Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn("BudgetTracker.Infrastructure")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
    
    [Fact]
    public void Domain_Should_NotHaveDependencyOnApi()
    {
        var result =Types.InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOn("BudgetTracker.Api")
            .GetResult();

        result.IsSuccessful.Should().BeTrue();
    }
}