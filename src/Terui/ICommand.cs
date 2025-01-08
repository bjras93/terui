using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Terui;

public interface ICommand
{
    string Name { get; init; }
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicProperties)]
    Type ArgsType { get; init; }
    ICommand? GetSubCommand(string name);
    Task ExecuteAsync();
    void SetArg(
        string propertyName,
        object? value);
}