# TemplateConfig

TemplateConfig resolves `{Path}` references in .NET configuration values. Install the `Namh.Configuration.Template` package, register your usual providers, and call `BuildTemplateConfig()`:

```csharp
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .BuildTemplateConfig();
```

`BuildTemplateConfig()` returns an `IConfigurationRoot`, so existing configuration APIs such as indexers, `GetSection()`, `GetChildren()`, and `Reload()` continue to work while referenced values are resolved when read. The `Get<T>()` extension reads and binds a typed value from a configuration section.

The examples below use [`appsettings.json`](https://github.com/NamHqUs/template-config/blob/main/src/Namh.Configuration.Template.Test/appsettings.json).

## Examples

### 1. Look up

References can point into parent and sibling fields recursively:

```jsonc
{
  "Primitive": "Pri",                                 // -> Result: "Pri"
  "Ref-Primitive1": "--{Primitive}--",                // -> Result: "--Pri--"
  "Ref-Primitive2": "{Primitive}.{Ref-Primitive1}",   // -> Result: "Pri.--Pri--"
  "Ref-Primitive-Loop": "--{{Primitive}mitive}--",    // -> Result: "--Pri--"

  "Array": [ "A", "B", "{Primitive}" ],               // -> Result: [ "A", "B", "Pri" ]
}
```

```csharp
var value = configuration["Ref-Primitive-Loop"]       // -> Result: "--Pri--"
```

Use `Get<T>()` to bind a resolved value to a .NET type:

```csharp
var values = configuration.Get<string[]>("Ref-Array");
// -> Result: [ "1", "2", "--Pri--" ]
```

Sections resolve referenced values through their indexer and child sections:

```csharp
var field = configuration.GetSection("Root:Child")["Field2"];
// -> Result: "-F1:Pri-"
```

### 2. Escape a character

Use a backtick to keep braces literal:

```json
{
  "Primitive": "Pri",
  "Escaped": "--`{Primitive`}--"
}
```

`Escaped` resolves to `--{Primitive}--`.

### 3. Error syntax

An unmatched brace throws `InvalidOperationException` when the value is read:

```json
{
  "MissingClose": "--{Primitive--",
  "MissingOpen": "--Primitive}--"
}
```

## Build and test

```bash
dotnet build src/Template.slnx
dotnet test src/Template.slnx
```