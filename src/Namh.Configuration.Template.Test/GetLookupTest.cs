using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Namh.Configuration.Template.Test;

[TestFixture]
internal class GetLookupTest : TestBase
{
    [TestCase("Array", new string[] {"1", "2", "--Pri--"})]
    [TestCase("Ref-Array", new string[] { "1", "2", "--Pri--" })]
    public void Get_Ref_Array(string key, string[] expected)
    {
        var values = _configuration.Get<string[]>(key);
        Assert.That(values, Is.EqualTo(expected));
    }

    [Test]
    public void Get_ConcurrentReferences_DoesNotMutateProviderSections()
    {
        var values = new string[1000];

        Parallel.For(0, values.Length, index =>
        {
            values[index] = _configuration["Ref-Primitive1"]!;
        });

        Assert.That(values, Is.All.EqualTo("--Pri--"));
    }
}
