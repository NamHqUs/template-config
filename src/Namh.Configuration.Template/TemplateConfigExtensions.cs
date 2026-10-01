using Namh.Configuration.Template;

#pragma warning disable IDE0130
namespace Microsoft.Extensions.Configuration;       // use the same namespace as IConfigurationBuilder to make it easier to use
#pragma warning restore IDE0130
public static class TemplateConfigExtensions
{
    public static IConfigurationRoot BuildTemplateConfig(this IConfigurationBuilder builder)
    {
        var configuration = builder.Build();
        return new ConfigurationRoot(configuration);
    }
}
