using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace TemplateConfig.Test;

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