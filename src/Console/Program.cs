
using Terui;
using Terui.Elements;

var app = new App();

app
    .AddCommand(
        new TableCommand("table")
        );


await app.RunAsync(args);


class TableCommand(string name) :
    AsyncCommand<DefaultArgs>(name)
{
    public override DefaultArgs Args { get; init; } = new DefaultArgs();
    public override Task ExecuteAsync()
    {
        var table = new Table<string>("Name", "Name");

        table.AddRow("")
            .AddColumn("Test")
            .AddColumn("Test");
        table.AddRow("")
            .AddColumn("Test1")
            .AddColumn("Test2");

        table.Render();
        table.HandleInput();
        return Task.FromResult(0);
    }
}