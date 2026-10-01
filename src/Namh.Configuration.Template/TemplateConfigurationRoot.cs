using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Namh.Configuration.Template;

internal class TemplateConfigurationRoot(IConfigurationRoot configurationRoot) : IConfigurationRoot, IConfiguration
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
            yield return new TemplateConfigurationSection(this, e);
    }

    IConfigurationSection IConfiguration.GetSection(string key)
    {
        var section = _lookupDetector.GetLookup(key);

        return new TemplateConfigurationSection(this, section);
    }

    #endregion
}
