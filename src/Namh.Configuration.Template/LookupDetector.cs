using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using System.Text;

namespace Namh.Configuration.Template;

internal class LookupDetector(IConfiguration configuration)
{
    internal const char START = '{';
    internal const char END = '}';
    internal const char ESCAPE = '`';
    private readonly ConcurrentDictionary<string, IConfigurationSection> _cache = new();
    public void ClearCache()
        => _cache.Clear();

    public IConfigurationSection GetLookup(string path)
        => _cache.GetOrAdd(path, key => LookupSection(key));

    private IConfigurationSection LookupSection(string path, bool isRequired = false)
    {
        var section = isRequired ? configuration.GetRequiredSection(path) : configuration.GetSection(path);

        return ParseValue(section).section;
    }

    private (IConfigurationSection section, int endIndex) ParseValue(IConfigurationSection section, int startAt = 0)
    {
        if (string.IsNullOrEmpty(section.Value))
            return (section, 0);

        var lookupValue = new StringBuilder();
        IConfigurationSection? lookupSection = null;

        var length = section.Value.Length - 1;
        for (var i = startAt; i <= length; i++)
        {
            var ch = section.Value[i];
            if (ch == START)
            {
                var (newSection, endIndex) = ParseValue(section, i + 1);
                if (endIndex > length)
                    throw new InvalidOperationException($"Unmatched '{START}' in configuration value: {section.Value}");
                lookupSection = newSection;
                lookupValue.Append(newSection.Value);
                i = endIndex;
            }
            else if (ch == END)
            {
                if (startAt == 0)
                    throw new InvalidOperationException($"Unmatched '{END}' in configuration value: {section.Value}");
                var newSection = LookupSection(lookupValue.ToString(), isRequired: true);
                return (newSection, i);
            }
            else if (ch == ESCAPE && i < length 
                && (section.Value[i + 1] == START || section.Value[i + 1] == END || section.Value[i + 1] == ESCAPE))
                lookupValue.Append(section.Value[++i]);
            else
                lookupValue.Append(ch);
        }

        var value = lookupValue.ToString();
        if (lookupSection != null && string.IsNullOrEmpty(value))
            return (lookupSection, section.Value.Length);

        var res = (section, section.Value.Length);
        section.Value = lookupValue.ToString();
        return res;
    }
}
