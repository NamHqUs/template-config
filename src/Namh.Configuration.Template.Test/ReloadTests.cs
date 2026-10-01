using NUnit.Framework;

namespace Namh.Configuration.Template.Test;

[TestFixture]
public class ReloadTests : TestBase
{
    [Test]
    public void Reload_ReloadsProvidersWithoutChangingConfiguredValues()
    {
        _configuration.Reload();

        Assert.That(_configuration["Primitive"], Is.EqualTo("Pri"));
        Assert.That(_configuration["Root:Field2"], Is.EqualTo("Pri"));
    }
}