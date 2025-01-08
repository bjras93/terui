using Terui.Interaction;

namespace Terui;

public interface IInteractable
{
    void HandleInput();
    void Select(ISelectable selectable);
    void Unselect(ISelectable selectable);
}