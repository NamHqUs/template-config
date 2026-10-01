using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Namh.Configuration.Template.Test;

public abstract class TestBase
{
    protected IConfigurationRoot _configuration = null!;

    [SetUp]
    public void SetUpConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");
        _configuration = builder.BuildTemplateConfig();
    }
}