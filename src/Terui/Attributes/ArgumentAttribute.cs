using System;

namespace Terui.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class ArgumentAttribute(
    int position,
    string name,
    bool required = false) : Attribute
{
    private readonly string Name = name;
    private readonly int Position = position;
    private readonly bool Required = required;
    public string GetName() => Name;
    public int GetPosition() => Position;
    public bool IsRequired() => Required;
}