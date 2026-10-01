# TemplateConfig

TemplateConfig resolves `{Path}` references in .NET configuration values. Register your usual providers and call `BuildTemplateConfig()`:

```csharp
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .BuildTemplateConfig();
```

The examples below use [`appsettings.json`](https://github.com/NamHqUs/template-config/blob/main/src/TemplateConfig.Test/appsettings.json).

## Examples

### 1. Look up

References can point into parent and sibling fields recursively, example `appsetings.json`

```jsonc
{
  "Primitive": "Pri",                                 // -> Result: "Pri"
  "Ref-Primitive1": "--{Primitive}--",                // -> Result: "--Pri--"
  "Ref-Primitive2": "{Primitive}.{Ref-Primitive1}",   // -> Result: "Pri.--Pri--"
  "Ref-Primitive-Loop": "--{{Primitive}mitive}--",    // -> Result: "--Pri--"
}
```

```csharp
var value = configuration["Ref-Primitive-Loop"]       // -> Result: "--Pri--"
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