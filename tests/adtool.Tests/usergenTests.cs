using System.Collections.Generic;
using adtool.AD;
using Xunit;

public class UsernameGeneratorTests
{
    [Fact]
    public void Generate_NoCollision_ReturnsBaseName()
    {
        var existing = new HashSet<string>();
        var gen = new UsernameGenerator(u => existing.Contains(u));

        var result = gen.Generate("Jane", "Doe");

        Assert.Equal("jdoe", result);
    }

    [Fact]
    public void Generate_WithCollision_AppendsSuffix()
    {
        var existing = new HashSet<string>{ "jsmith" };
        var gen = new UsernameGenerator(u => existing.Contains(u));

        var result = gen.Generate("John", "Smith");

        Assert.Equal("jsmith1", result);
    }

    [Fact]
    public void Generate_WithMultipleCollision_IncrementsSuffix()
    {
        var existing = new HashSet<string>{ "jsmith", "jsmith1", "jsmith2" };
        var gen = new UsernameGenerator(u => existing.Contains(u));

        var result = gen.Generate("John", "Smith");

        Assert.Equal("jsmith3", result);
    }

    [Fact]
    public void Generate_SanitizesNonAlphanumericCharacters()
    {
        var existing = new HashSet<string>();
        var gen = new UsernameGenerator(u => existing.Contains(u));

        var result = gen.Generate("O'Brien", "Smith-Jones");

        Assert.Equal("osmithjones", result);
    }

    [Fact]
    public void ThrowOnEmptyFirstName()
    {
        var gen = new UsernameGenerator(u => false);

        Assert.Throws<System.ArgumentException>(() => gen.Generate("", "Smith"));
    }
}