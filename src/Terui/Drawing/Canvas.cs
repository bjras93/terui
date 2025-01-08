namespace Terui.Drawing;

public sealed class Canvas
{
    public enum Edges
    {
        Top,
        Bottom,
        Right,
        Left
    }
    private static int MaxWidth => Console.BufferWidth;
    private static int MaxHeight => Console.BufferHeight;
    private bool Initial = true;
    public Canvas(
        CanvasSize size,
        int padding = 1
    )
    {
        if (size == CanvasSize.Full)
        {
            Width = MaxWidth;
            Height = MaxHeight;
        }
        Size = size;
        Padding = padding;
    }
    private readonly CanvasSize Size;
    public readonly int Padding;
    public readonly int Width;
    public readonly int Height;
    public readonly static Position Cursor = GetCursorPosition();
    private readonly List<Placement> Placements = [];
    private static Position GetCursorPosition()
    {
        var (Left, Top) = Console.GetCursorPosition();
        return new Position(Left, Top);
    }
    private int GetEdge(Edges edge)
    => edge switch
    {
        Edges.Top => 0 + Padding,
        Edges.Bottom => Height - Padding,
        Edges.Right => Width - Padding,
        Edges.Left => 0 + Padding,
    };
    internal void RepeatPlacementX(
        char input,
        int left,
        Edges edge,
        ConsoleColor? color = null)
    {
        RepeatPlacementX(input, left, GetEdge(edge), color);
    }
    internal void RepeatPlacementX(
        char input,
        int left,
        int top,
        ConsoleColor? color = null)
    {
        while (Width - Padding - left > 0)
        {
            AddPlacement(input, left, top, color);
            ++left;
        }
    }
    internal void RepeatPlacementY(
        char input,
        int top,
        Edges edge,
        ConsoleColor? color = null)
    {
        RepeatPlacementY(input, top, GetEdge(edge), color);
    }
    internal void RepeatPlacementY(
        char input,
        int left,
        int top,
        ConsoleColor? color = null)
    {
        while (Height - Padding - top > 0)
        {
            AddPlacement(input, left, top, color);
            ++top;
        }
    }
    internal void AddPlacement(
        string input,
        int left,
        int top,
        ConsoleColor? color = null)
    {
        left = AdjustBoundary(left, Padding, Width);
        top = AdjustBoundary(top, Padding, Height);
        var split = input.ToCharArray();
        for (int i = 0; i < split.Length; i++)
        {
            ClearPlacements(left, top);
            Placements.Add(new Placement(split[i], left, top, color));
            ++left;
        }
    }
    internal void AddPlacement(
        char input,
        int left,
        int top,
        ConsoleColor? color = null)
    {
        ClearPlacements(left, top);
        left = AdjustBoundary(left, Padding, Width);
        top = AdjustBoundary(top, Padding, Height);
        Placements.Add(new Placement(input, left, top, color));
    }
    private void ClearPlacements(int left, int top)
    {
        Placements.RemoveAll(p => p.Key == Placement.PlacementKey(left, top));
    }
    private static int AdjustBoundary(
        int position,
        int padding,
        int limit)
    {
        return position == 0 || position == limit ? position + padding : position;
    }
    public void Render()
    {
        if (Initial)
        {
            for (int i = 0; i < Height; i++)
            {
                Console.WriteLine();
            }
            Initial = false;
        }
        foreach (var placement in Placements)
        {
            if (!placement.Rerender)
            {
                continue;
            }
            var (left, top) = placement.Position;
            Console.SetCursorPosition(left, top);
            if (placement.Color.HasValue)
                Console.ForegroundColor = placement.Color.Value;

            Console.Write(placement.Character);

            Console.ResetColor();
            placement.Rerender = false;
        }
    }
    public void Clear()
    {
        for (int i = 0; i < Height; i++)
        {
            Console.WriteLine();
        }
    }
}
