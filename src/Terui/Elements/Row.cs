using System.Text;
using Terui.Drawing;
using Terui.Interaction;
using Terui.Visuals;

namespace Terui.Elements;

public sealed class Row<T>(
    T item,
    int index,
    Action<T>? action = null) : IRow, IRenderable, ISelectable
{
    public bool IsSelected { get; set; } = false;
    public bool Rerender { get; set; } = true;
    public Position Position { get; set; }
    public int Index { get; } = index;
    private readonly List<Column> columns = [];
    private Column[] readonlyColumns = [];
    private readonly T? item = item;
    private readonly Action<T>? action = action;
    public IRow AddColumn(string Text)
    {
        var column = new Column { Text = Text };

        columns.Add(column);

        return this;
    }
    public void SetPosition(int left, int top)
    {
        Position = new Position(left, top);
    }
    public void RunAction()
    {
        if (action == null || item == null)
        {
            return;
        }
        action.Invoke(item);
    }
    public Column[] GetColumns()
    {
        readonlyColumns = [.. columns];
        return readonlyColumns;
    }
    public string BuildTop(
        int length)
    {
        var sb = new StringBuilder();

        sb.Append(TableBorder.HeaderTopLeft);
        sb.Append(TableBorder.HeaderTop, length - 2);
        sb.Append(TableBorder.HeaderTopLeft);
        return sb.ToString();
    }
    public string BuildBottom(
        int length)
    {
        var sb = new StringBuilder();

        sb.Append(TableBorder.HeaderTopLeft);
        sb.Append(TableBorder.HeaderTop, length - 2);
        sb.Append(TableBorder.HeaderTopLeft);
        return sb.ToString();
    }
}
