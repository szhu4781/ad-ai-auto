using System.Collections.Generic;
using adtool.Models;
using adtool.JSONCheck;
using Xunit;

public class RequestValidatorTests
{
    private readonly HashSet<string> _validOUs = new HashSet<string> { "OU=Sales,DC=lab,DC=local" };

    [Fact]
    public void Validate_ValidRequest_ReturnsValid()
    {
        var validator = new RequestValidator(_validOUs);
        var request = new OnboardingRequest
        {
            FirstName = "Jane",
            LastName = "Doe",
            Department = "Sales",
            OU = "OU=Sales,DC=lab,DC=local"
        };

        var result = validator.Validate(request);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_UnknownOU_ReturnsInvalid()
    {
        var validator = new RequestValidator(_validOUs);
        var request = new OnboardingRequest
        {
            FirstName = "Jane",
            LastName = "Doe",
            Department = "Sales",
            OU = "OU=Cleaning,DC=lab,DC=local"
        };

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("not a recognized"));
    }

    [Fact]
    public void Validate_MissingFields_ReturnsAllErrors()
    {
        var validator = new RequestValidator(_validOUs);
        var request = new OnboardingRequest();

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Equal(4, result.Errors.Count);
    }

    [Fact]
    public void Validate_NullRequests_ReturnsInvalid()
    {
        var validator = new RequestValidator(_validOUs);

        var result = validator.Validate(null);

        Assert.False(result.IsValid);
    }
}