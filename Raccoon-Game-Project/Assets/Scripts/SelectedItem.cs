/// <summary>
/// Persistent item selection
/// </summary>
public static class SelectedItem
{
    private static int keyItem;
    private static int consumableItem;

    public static int KeyItem { get => keyItem; set => keyItem = value % 18; } //just to be safe.
    public static int ConsumableItem { get => consumableItem; set => consumableItem = value % 24; } //just to be safe.
}
