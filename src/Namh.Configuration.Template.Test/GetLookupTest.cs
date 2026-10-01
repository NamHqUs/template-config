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
}
