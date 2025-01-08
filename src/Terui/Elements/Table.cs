using Terui.Drawing;
using Terui.Interaction;
using Terui.Visuals;

namespace Terui.Elements;

public sealed class Table<T> : IInteractable
{
    private readonly HeaderRow Header;
    private readonly List<Row<T>> rows = [];
    private readonly Canvas Canvas = new(CanvasSize.Full, 1);
    private int MaxColumns = 0;
    public Table(
    params string[] columnValues)
    {
        Header = CreateHeader(columnValues);
    }
    private HeaderRow CreateHeader(
        params string[] names)
    {
        var header = new HeaderRow();
        foreach (var name in names)
        {
            header.AddColumn(name);
        }
        MaxColumns = names.Length;
        return header;
    }
    public Row<T> AddRow(T item, Action<T>? action = null)
    {
        var row = new Row<T>(item, rows.Count, action);
        rows.Add(row);
        return row;
    }
    public void Render()
    {
        var columnSegments = Canvas.Width / MaxColumns;
        if (Header.Rerender)
        {
            const int headerPosition = 2;
            var headerColumns = Header.GetColumns();
            for (int i = 0; i < headerColumns.Length; i++)
            {
                var start = (columnSegments * i) + headerPosition;
                Canvas.AddPlacement(headerColumns[i].Text, start, headerPosition);
            }
            Header.Rerender = false;
        }
        foreach (var row in rows)
        {
            PlaceColumns(row);
        }
        Canvas.Render();
    }

    private void PlaceColumns(Row<T> row)
    {
        if (!row.Rerender)
        {
            return;
        }
        var columns = row.GetColumns();
        if (columns.Length > MaxColumns)
        {
            throw new Exception("Row columns cannot exceed header column amount.");
        }
        Canvas.AddPlacement(TableBorder.CellLeft, 0, row.Index + 3);
        var columnSegments = Canvas.Width / MaxColumns;
        ConsoleColor? color = row.IsSelected ? ConsoleColor.DarkCyan : null;
        for (int i = 0; i < columns.Length; i++)
        {
            var start = (columnSegments * i) + 2;
            Canvas.AddPlacement(columns[i].Text, start, row.Index + 3, color);
            Canvas.AddPlacement(TableBorder.CellLeft, start - 1, row.Index + 3);
        }
        Canvas.AddPlacement(TableBorder.CellRight, Canvas.Width, row.Index + 3);
        row.Rerender = false;
    }
    public void Select(ISelectable selectable)
    {
        selectable.IsSelected = true;
        selectable.Rerender = true;
    }
    public void Unselect(ISelectable selectable)
    {
        selectable.IsSelected = false;
        selectable.Rerender = true;
    }

    public void HandleInput()
    {
        var exit = false;
        while (!exit)
        {
            Console.CursorVisible = true;
            var consoleKey = Console.ReadKey(false);
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
            Render();
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
            Select(selected);
        }
        return selected;
    }
    private Row<T> SelectPrevious()
    {
        var selected = GetSelected();
        if (selected.Index == 0)
            return selected;
        Unselect(selected);

        selected = rows[selected.Index - 1];
        Select(selected);
        return selected;
    }
    private Row<T> SelectNext()
    {
        var selected = GetSelected();
        if (rows.Count == selected.Index + 1)
            return selected;
        Unselect(selected);

        selected = rows[selected.Index + 1];
        Select(selected);
        return selected;
    }
}
