using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Namh.Configuration.Template;

internal sealed class ResolvedConfigurationSection(
    IConfigurationSection section,
    string? resolvedValue) : IConfigurationSection
{
    private string? _resolvedValue = resolvedValue;

    string IConfigurationSection.Key => section.Key;
    string IConfigurationSection.Path => section.Path;
    string? IConfigurationSection.Value
    {
        get => _resolvedValue;
        set
        {
            section.Value = value;
            _resolvedValue = value;
        }
    }

    IChangeToken IConfiguration.GetReloadToken() => section.GetReloadToken();

    string? IConfiguration.this[string key]
    {
        get => section[key];
        set => section[key] = value;
    }

    IEnumerable<IConfigurationSection> IConfiguration.GetChildren()
        => section.GetChildren();

    IConfigurationSection IConfiguration.GetSection(string key)
        => section.GetSection(key);
}
