using Terui.Drawing;

namespace Terui.Elements;

public interface IRow
{
    Position Position { get; set; }
    int Index { get; }
    void SetPosition(int left, int top);
    public IRow AddColumn(string Text);
    Column[] GetColumns();
}