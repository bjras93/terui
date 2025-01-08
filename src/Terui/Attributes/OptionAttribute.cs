

using System;

namespace Terui.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class OptionAttribute(
    string name,
    string? alias = "") : Attribute
{
    private readonly string Name = name;
    private readonly string? Alias = alias;
    public string GetName() => Name;
    public string? GetAlias() => Alias;
}