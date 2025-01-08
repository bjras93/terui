using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Terui;

public abstract class AsyncCommand<
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors |
 DynamicallyAccessedMemberTypes.PublicProperties)] TArgs>(
    string name)
: ICommand
where TArgs : IArgs
{
    private Dictionary<string, object>? SubCommands { get; set; }
    public string Name { get; init; } = name;
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicProperties)]
    public Type ArgsType { get; init; } = typeof(TArgs);
    public abstract TArgs Args { get; init; }
    public abstract Task ExecuteAsync();
    public AsyncCommand<TArgs> AddSubCommand<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors |
    DynamicallyAccessedMemberTypes.PublicProperties)] TSubArgs>(AsyncCommand<TSubArgs> command)
    where TSubArgs : IArgs
    {
        SubCommands ??= [];
        SubCommands.Add(command.Name, command);
        return this;
    }
    public ICommand? GetSubCommand(string name)
    {
        if (SubCommands?.TryGetValue(name, out var cmd) != true)
            return default;

        return (ICommand?)cmd;
    }
    public void SetArg(
        string propertyName,
        object? value)
    {
        var property = ArgsType.GetProperty(propertyName);
        if (property == null)
            return;

        property.SetValue(Args, value);
    }
}