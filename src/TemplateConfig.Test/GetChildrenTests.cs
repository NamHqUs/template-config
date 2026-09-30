using NUnit.Framework;

namespace TemplateConfig.Test;

[TestFixture]
public class GetChildrenTests : TestBase
{
    [Test]
    public void GetChildren_ReturnsAllRootSections()
    {
        var childKeys = _configuration.GetChildren().Select(section => section.Key);

        Assert.That(childKeys, Is.EquivalentTo(new[]
        {
            "Primitive", "Ref-Primitive", "Ref-Primitive1", "Ref-Primitive2", "Ref-Primitive-Loop", "Root",
            "Escaped1", "Escaped2",
            "Error1", "Error2", "Error3", "Error4", "Error5", "Error6"

        }));
    }

    [TestCase("Root", "Field1", "F1")]
    [TestCase("Root", "Field2", "Pri")]
    [TestCase("Root:Child", "Field1", "F1")]
    [TestCase("Root:Child", "Field2", "-F1:Pri-")]
    public void GetChildren_SectionsResolveReferencedValues(string path, string childKey, string expectedValue)
    {
        var parent = _configuration.GetSection(path);
        var child = parent.GetChildren().Single(section => section.Key == childKey);

        Assert.That(child.Value, Is.EqualTo(expectedValue));
    }

    [Test]
    public void GetChildren_ReturnsNoChildrenForLeafSection()
        => Assert.That(_configuration.GetSection("Primitive").GetChildren(), Is.Empty);
}