namespace Terui.Tests;

public sealed class CreateApp
{
    [Fact]
    public async Task Setup()
    {
        var app = new App();

        app.AddCommand(new TestCommand("test"));
        using var outputWriter = new StringWriter();
        Console.SetOut(outputWriter);
        await app.RunAsync(["test"]);
        string output = outputWriter.ToString();

        Assert.Equal("test", output);
    }
}
internal sealed class TestCommand(string name) : AsyncCommand<IArgs>(name)
{
    public override IArgs Args { get; init; } = new DefaultArgs();

    public override Task ExecuteAsync()
    {
        Console.Write("test");
        return Task.FromResult(0);
    }
}