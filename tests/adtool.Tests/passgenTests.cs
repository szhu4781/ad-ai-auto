using System;
using System.Linq;
using adtool.AD;
using Xunit;

public class PasswordGeneratorTests
{
    [Fact]
    public void Generate_DefaultLength_Returns16Characters()
    {
        var gen = new PasswordGenerator();
        var password = gen.Generate();

        Assert.Equal(16, password.Length);
    }

    [Fact]
    public void Generate_ContainsAtLeastOneUppercase()
    {
        var gen = new PasswordGenerator();
        var password = gen.Generate();

        Assert.Contains(password, char.IsUpper);
    }

    [Fact]
    public void Generate_ContainsAtLeastOneLowercase()
    {
        var gen = new PasswordGenerator();
        var password = gen.Generate();

        Assert.Contains(password, char.IsLower);
    }
    
    [Fact]
    public void Generate_ContainsAtLeastOneDigit()
    {
        var gen = new PasswordGenerator();
        var password = gen.Generate();

        Assert.Contains(password, char.IsDigit);
    }

    [Fact]
    public void Generate_ContainsAtLeastOneSymbol()
    {
        var gen = new PasswordGenerator();
        var password = gen.Generate();

        Assert.True(password.Any(c => "!@#$%^&*".Contains(c)), "Password should contain at least one symbol.");
    }

    [Fact]
    public void Generate_ThrowsIfLengthTooShort()
    {
        var gen = new PasswordGenerator();

        Assert.Throws<ArgumentException>(() => gen.Generate(5));
    }

    [Fact]
    public void Generate_TwoCallsProduceDifferentPasswords()
    {
        var gen = new PasswordGenerator();
        var p1 = gen.Generate();
        var p2 = gen.Generate();

        Assert.NotEqual(p1, p2);
    }
}