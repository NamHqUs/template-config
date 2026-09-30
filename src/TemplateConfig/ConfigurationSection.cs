using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace TemplateConfig;

internal class ConfigurationSection(IConfigurationRoot root, IConfigurationSection section) : IConfigurationSection
{
    string IConfigurationSection.Key => section.Key;
    string IConfigurationSection.Path => section.Path;
    string? IConfigurationSection.Value { get => section.Value; set => section.Value = value; }

    IChangeToken IConfiguration.GetReloadToken() => section.GetReloadToken();

    string? IConfiguration.this[string key]
    {
        get => root[AbsolutePath(key)];
        set => section[key] = value;
    }

    IEnumerable<IConfigurationSection> IConfiguration.GetChildren()
    {
        foreach (var e in section.GetChildren())
            yield return root.GetSection(AbsolutePath(e.Key));
    }

    IConfigurationSection IConfiguration.GetSection(string key)
        => root.GetSection(AbsolutePath(key));

    private string AbsolutePath(string key)
        => $"{section.Path}:{key}";

}
