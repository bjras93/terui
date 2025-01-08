using System.Text;
using Terui.Drawing;
using Terui.Visuals;

namespace Terui.Elements;

public sealed class HeaderRow : IRow, IRenderable
{
    public bool Rerender { get; set; } = true;
    public bool IsSelected = false;
    public Position Position { get; set; }
    public int Index { get; } = 0;
    private readonly List<Column> columns = [];
    private Column[] readonlyColumns = [];
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
    public Column[] GetColumns()
    {
        readonlyColumns = [.. columns];
        return readonlyColumns;
    }
    public string Build(
        int cellPadding)
    {
        var sb = new StringBuilder();
        var columnTexts = columns.Select(c => c.Build(cellPadding));
        var columnLength = columnTexts.Sum(c => c.Length);
        sb.Append(TableBorder.HeaderTopLeft);
        sb.Append(TableBorder.HeaderTopSeparator, columnLength - 2);
        sb.Append(TableBorder.HeaderTopRight);
        sb.AppendLine();
        foreach (var text in columnTexts)
        {
            sb.Append(text);
        }
        sb.AppendLine();
        sb.Append(TableBorder.HeaderBottomLeft);
        sb.Append(TableBorder.HeaderBottom, columnLength - 2);
        sb.Append(TableBorder.HeaderBottomRight);

        return sb.ToString();
    }
}
