using NUnit.Framework;

namespace Namh.Configuration.Template.Test;

[TestFixture]
public class LookupValuesTest : TestBase
{
    [TestCase("Primitive", "Pri")]
    [TestCase("Ref-Primitive", "Pri")]
    [TestCase("Ref-Primitive1", "--Pri--")]
    [TestCase("Ref-Primitive2", "--Pri--Pri--")]
    [TestCase("Ref-Primitive-Loop", "--Pri--")]
    public void Retrieve_Primitive(string key, string expectedValue)
        => Assert.That(_configuration[key], Is.EqualTo(expectedValue));

    [TestCase("Root:Field1", "F1")]
    [TestCase("Root:Field2", "Pri")]
    [TestCase("Root:Child:Field1", "F1")]
    [TestCase("Root:Child:Field2", "-F1:Pri-")]
    public void Retrieve_Path(string key, string expectedValue)
        => Assert.That(_configuration[key], Is.EqualTo(expectedValue));

    [TestCase("Array", "0", "1")]
    [TestCase("Array", "1", "2")]
    [TestCase("Array", "2", "--Pri--")]
    public void Retrieve_Array(string key, string index, string expectedValue)
        => Assert.That(_configuration[$"{key}:{index}"], Is.EqualTo(expectedValue));

    [TestCase("Array", "0", "1")]
    [TestCase("Array", "1", "2")]
    [TestCase("Array", "2", "--Pri--")]
    public void Retrieve_Array_Section(string key, string index, string expectedValue)
    {
        var section = _configuration.GetSection(key);
        Assert.That(section[index], Is.EqualTo(expectedValue));
    }

    [TestCase("Escaped1", "--{abc}--")]
    [TestCase("Escaped2", "--{Primitive}--")]
    public void Escaped(string key, string expectedValue)
    => Assert.That(_configuration[key], Is.EqualTo(expectedValue));

    [TestCase("Error1", "Unmatched '{' in configuration value")]
    [TestCase("Error2", "Unmatched '{' in configuration value")]
    [TestCase("Error3", "Unmatched '{' in configuration value")]
    [TestCase("Error4", "Unmatched '}' in configuration value")]
    [TestCase("Error5", "Unmatched '}' in configuration value")]
    [TestCase("Error6", "Unmatched '}' in configuration value")]
    public void Syntax_Error(string key, string expectedValue)
    {
        try
        {
            var value = _configuration[key];
            Assert.Fail($"Expected InvalidOperationException for key '{key}' but got value '{value}'");
        }
        catch (InvalidOperationException ex)
        {
            Assert.That(ex.Message, Does.Contain(expectedValue));
        }
    }
}