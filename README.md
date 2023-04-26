# EasyMapper

EasyMapper is a lightweight and easy-to-use library for mapping properties between objects in C#. It provides a simple reflection-based extension method called `HydrateFrom`, as well as a fluent interface through the `PropertyMapper` class, for more advanced scenarios.

## Features

- Simple reflection-based property mapping
- Basic type conversion using `Convert.ChangeType`
- Customizable property mapping with actions
- Support for ignoring specific properties
- Fluent interface for more complex mapping scenarios

## Usage

### HydrateFrom Extension Method

```csharp
// Basic usage
target.HydrateFrom(source);

// With custom after-map action
target.HydrateFrom(source, (tgt, src) => { /* Custom mapping logic */ });

// Ignoring specific properties
target.HydrateFrom(source, null, new[] { "Property1", "Property2" });
```

### PropertyMapper Fluent Interface

```csharp
// Basic usage
var target = PropertyMapper<Source, Target>.Create(source, () => new Target()).Map();

// With custom after-map action
var target = PropertyMapper<Source, Target>.Create(source, () => new Target())
    .WithCustomAfterMap((tgt, src) => { /* Custom mapping logic */ })
    .Map();

// Ignoring specific properties
var target = PropertyMapper<Source, Target>.Create(source, () => new Target())
    .IgnoreProperties("Property1", "Property2")
    .Map();
```