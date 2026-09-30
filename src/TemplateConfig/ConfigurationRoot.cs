using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace TemplateConfig;

internal class ConfigurationRoot(IConfigurationRoot configurationRoot) : IConfigurationRoot, IConfiguration
{
    internal readonly LookupDetector _lookupDetector = new(configurationRoot);

    #region IConfigurationRoot

    IEnumerable<IConfigurationProvider> IConfigurationRoot.Providers => configurationRoot.Providers;
    void IConfigurationRoot.Reload()
    {
        _lookupDetector.ClearCache();
        configurationRoot.Reload();
    }

    #endregion

    #region IConfiguration

    IChangeToken IConfiguration.GetReloadToken() => configurationRoot.GetReloadToken();
    string? IConfiguration.this[string key]
    {
        get => _lookupDetector.GetLookup(key).Value;
        set => configurationRoot[key] = value;
    }

    IEnumerable<IConfigurationSection> IConfiguration.GetChildren()
    {
        foreach (var e in configurationRoot.GetChildren())
            yield return new ConfigurationSection(this, e);
    }

    IConfigurationSection IConfiguration.GetSection(string key)
    {
        var section = _lookupDetector.GetLookup(key);

        return new ConfigurationSection(this, section);
    }

    #endregion
}
