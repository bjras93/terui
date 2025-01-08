namespace Terui.Drawing;

internal sealed class Placement : IRenderable
{
    public Placement(
        char character,
        int left,
        int top,
        ConsoleColor? color = null)
    {
        Character = character;
        Position = new Position(left, top);
        Color = color;
    }
    public Placement(
        char character,
        Position position,
        ConsoleColor? color = null)
    {
        Character = character;
        Position = position;
        Color = color;
    }
    public bool Empty { get => Character == ' '; }
    public char Character { get; }
    public Position Position { get; }
    public ConsoleColor? Color { get; }
    public int Key { get => PlacementKey(Position.Left, Position.Top); }
    public bool Rerender { get; set; } = true;

    public static int PlacementKey(
        int left,
        int top
    )
    {
        return $"{left}{top}".GetHashCode();
    }
}