using System.Threading.Tasks;

namespace Terui;

public sealed class Command(
    string name) : AsyncCommand<CommandArgs>(name)
{
    public override CommandArgs Args { get; init; } = new CommandArgs();
    public override Task ExecuteAsync()
    {
        return Task.FromResult(0);
    }
}
public sealed class CommandArgs : IArgs
{

}