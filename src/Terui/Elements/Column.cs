using System.Text;
using Terui.Visuals;

namespace Terui.Elements;

public sealed class Column
{
    public required string Text { get; set; }
    public string Build(int padding)
    {
        var sb = new StringBuilder();
        sb.Append(TableBorder.CellLeft);
        sb.Append(' ', padding);
        sb.Append(Text);
        sb.Append(' ', padding);
        sb.Append(TableBorder.CellRight);

        return sb.ToString();
    }
}