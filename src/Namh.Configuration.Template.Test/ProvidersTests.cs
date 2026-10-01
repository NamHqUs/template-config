using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Namh.Configuration.Template.Test;

[TestFixture]
public class ProvidersTests : TestBase
{
    [Test]
    public void Providers_ExposesTheUnderlyingConfigurationProviders()
    {
        Assert.That(_configuration.Providers, Is.Not.Empty);
        Assert.That(_configuration.Providers,
            Is.All.InstanceOf<IConfigurationProvider>());
    }
}