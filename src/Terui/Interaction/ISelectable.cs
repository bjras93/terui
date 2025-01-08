namespace Terui.Interaction;

public interface ISelectable
{
    public bool IsSelected { get; set; }
    public bool Rerender { get; set; }
}