public class SelectionSystem
{
    public ISelectable CurrentSelected { get; private set; }

    public void Select(ISelectable target)
    {
        if (CurrentSelected == target)
            return;

        CurrentSelected?.Deselect();

        CurrentSelected = target;

        CurrentSelected.Select();
    }

    public void Clear()
    {
        CurrentSelected?.Deselect();

        CurrentSelected = null;
    }
}
