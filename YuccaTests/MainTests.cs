using FluentAssertions;
using Xunit;
using Yucca.Operations.App;

namespace YuccaTests;

public class MainTests
{
    [Fact]
    public void About_ShouldReturnCorrectHeader()
    {
        var expectedHeader = "About Yucca";

        var actualHeader = About.GetAboutInfo().Header;

        actualHeader.Should().Be(expectedHeader);
    }

    [Fact]
    public void About_ShouldReturnCorrectTitle()
    {
        var expectedTitle = "Yucca Accounts Manager";

        var actualTitle = About.GetAboutInfo().Title;

        actualTitle.Should().Be(expectedTitle);
    }

    [Fact]
    public void About_ShouldReturnCorrectAttributionToOriginal()
    {
        var expectedDescription = "Based on the original version by Edward Vella and Bernard Gatt © 2004-2007 Syntax Rebels. All rights reserved.";

        var actualDescription = About.GetAboutInfo().Description;

        actualDescription.Should().Contain(expectedDescription);
    }

    [Fact]
    public void About_ShouldReturnCorrectVersion()
    {
        var expectedVersion = "Version 3.0.0"; 

        var actualDescription = About.GetAboutInfo().Description;

        actualDescription.Should().Contain(expectedVersion);
    }

    [Fact]
    public void About_ShouldReturnFunctionalDescription()
    {
        var expectedVersion = "An app for small to medium-sized businesses that need a comprehensive tool for managing inventory, sales, clients, suppliers, and financial operations."; 

        var actualDescription = About.GetAboutInfo().Description;

        actualDescription.Should().Contain(expectedVersion);
    }
}