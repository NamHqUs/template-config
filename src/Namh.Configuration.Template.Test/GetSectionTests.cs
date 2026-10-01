using NUnit.Framework;

namespace Namh.Configuration.Template.Test;

[TestFixture]
public class GetSectionTests : TestBase
{
    [TestCase("Root", "Root", "Root")]
    [TestCase("Root:Child", "Child", "Root:Child")]
    [TestCase("Root:Child:Field1", "Field1", "Root:Child:Field1")]
    public void GetSection_ReturnsSectionKeyAndPath(string path, string expectedKey, string expectedPath)
    {
        var section = _configuration.GetSection(path);

        Assert.That(section.Key, Is.EqualTo(expectedKey));
        Assert.That(section.Path, Is.EqualTo(expectedPath));
    }

    [TestCase("Root:Field2", "Pri")]
    [TestCase("Root:Child:Field2", "-F1:Pri-")]
    public void GetSection_ReturnsResolvedReferencedValues(string path, string expectedValue)
        => Assert.That(_configuration.GetSection(path).Value, Is.EqualTo(expectedValue));

    [Test]
    public void GetSection_ReturnsNullValueAndRequestedPathForMissingSection()
    {
        var section = _configuration.GetSection("Missing:Nested");

        Assert.That(section.Key, Is.EqualTo("Nested"));
        Assert.That(section.Path, Is.EqualTo("Missing:Nested"));
        Assert.That(section.Value, Is.Null);
        Assert.That(section.GetChildren(), Is.Empty);
    }

    [Test]
    public void GetSection_ReturnsNestedSectionsAndSupportsSectionIndexerAndValueSetter()
    {
        var section = _configuration.GetSection("Root:Child");
        var nestedSection = section.GetSection("Field1");

        Assert.That(section.GetChildren().Select(child => child.Key),
            Is.EquivalentTo(["Field1", "Field2"]));
        Assert.That(section["Field2"], Is.EqualTo("-F1:Pri-"));
        Assert.That(nestedSection.Value, Is.EqualTo("F1"));

        nestedSection.Value = "Updated through section";

        Assert.That(_configuration["Root:Child:Field1"], Is.EqualTo("Updated through section"));
    }
}