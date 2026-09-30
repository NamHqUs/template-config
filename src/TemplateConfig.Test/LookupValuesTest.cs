using NUnit.Framework;

namespace TemplateConfig.Test;

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
    [TestCase("Service:HealthUrl", "https://orders.example.com/health")]
    [TestCase("Service:OrdersUrl", "https://orders.example.com/api/orders")]
    [TestCase("Service:StatusMessage", "Connecting to Orders API at https://orders.example.com")]
    public void Retrieve_Key_Ref(string key, string expectedValue)
        => Assert.That(_configuration[key], Is.EqualTo(expectedValue));

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