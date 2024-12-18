
var table = new Table<string>();

table.AddRow("")
    .AddColumn("Test")
    .AddColumn("Test");
table.AddRow("")
    .AddColumn("Test1")
    .AddColumn("Test2");

table.Render();
table.HandleInput();
public sealed class Table<T>
{
    private readonly List<Row<T>> rows = [];
    public Row<T> AddRow(T item, Action<T>? action = null)
    {
        var row = new Row<T>(item, rows.Count, action);
        rows.Add(row);
        return row;
    }
    public void Render()
    {
        var positions = new CursorPosition[rows.Count];

        var (left, top) = Console.GetCursorPosition();
        while ((top + positions.Length) >= Console.BufferHeight)
        {
            Console.WriteLine();
            top -= 1;
        }
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = new CursorPosition(left, top);
            rows[i].SetPosition(left, top);
            rows[i].RenderColumns();
            top += 1;
        }
        Console.WriteLine();
    }

    public void HandleInput()
    {
        var exit = false;
        while (!exit)
        {
            Console.CursorVisible = true;
            var consoleKey = Console.ReadKey();
            switch (consoleKey.Key)
            {
                case ConsoleKey.DownArrow:
                    SelectNext();
                    break;
                case ConsoleKey.UpArrow:
                    SelectPrevious();
                    break;

                case ConsoleKey.Escape:
                case ConsoleKey.Q:
                    exit = true;
                    break;
            }
        }
    }
    private Row<T> GetSelected()
    {
        if (rows.Count == 0)
            throw new Exception("Cannot get selected from empty table.");

        var selected = rows.Find(r => r.IsSelected);
        if (selected == null)
        {
            selected = rows.First();
            selected.Select();
        }
        return selected;
    }
    private Row<T> SelectPrevious()
    {
        var selected = GetSelected();
        if (selected.Index == 0)
            return selected;
        selected.Unselect();

        selected = rows[selected.Index - 1];
        selected.Select();

        return selected;
    }
    private Row<T> SelectNext()
    {
        var selected = GetSelected();
        if (rows.Count == selected.Index + 1)
            return selected;
        selected.Unselect();

        selected = rows[selected.Index + 1];
        selected.Select();

        return selected;
    }
}
public sealed class Row<T>
{
    public bool IsSelected = false;
    public CursorPosition Position { get; set; }
    public int Index { get; }
    private readonly List<Column> columns = [];
    private IReadOnlyCollection<Column> readonlyColumns = [];
    private readonly T? item;
    private readonly Action<T>? action;
    public Row(
        T item,
        int index,
        Action<T>? action = null)
    {
        Index = index;
        this.item = item;
        this.action = action;
    }
    public Row<T> AddColumn(string Text)
    {
        var column = new Column { Text = Text };

        columns.Add(column);

        return this;
    }
    public void SetPosition(int left, int top)
    {
        Position = new CursorPosition(left, top);
    }
    public IReadOnlyCollection<Column> GetColumns()
    {
        readonlyColumns = columns;
        return readonlyColumns;
    }
    public void RenderColumns()
    {
        Console.SetCursorPosition(Position.Left, Position.Top);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(Position.Left, Position.Top);
        Console.Write(Console.GetCursorPosition().Top);
        foreach (var column in columns)
        {
            Console.Write(column.Text);
        }
    }
    public void Select()
    {
        IsSelected = true;
        var firstColumn = columns.First();
        if (!firstColumn.Text.Contains('>'))
            firstColumn.Text = $">{firstColumn.Text}";

        RenderColumns();

    }
    public void Unselect()
    {
        IsSelected = false;
        var firstColumn = columns.First();

        if (firstColumn.Text.Contains('>'))
            firstColumn.Text = firstColumn.Text[1..];

        RenderColumns();
    }
}
public sealed class Column
{
    public required string Text { get; set; }
}
public record struct CursorPosition(int Left, int Top);