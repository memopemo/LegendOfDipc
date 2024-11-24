# Options

Options is a prefab for the canvas to be loaded.

Probably will be the only prefab, as most other menus are either only in a type of scene (pipe selection and demon buffs) or are physical game objects instead (shops)

Loads behind iris wipe

must also be able to handle switching the unity event system to itself and deny switching to the parent menu's options.

```c#

LoadSettings()
{
    CreateSettingsScreen();
    PositionSettingsScreen();
    EventSystem.SetSelectedTo(SettingsScreen.firstOption);
}
UnloadSettings()
{
    EventSystem.SetSelectedTo(MainMenu.lastSelected);
    DestroySettingsScreen();
}
OnPushBack()
{
    UnloadSettings();
}
```

In unity:

- Set selections in main and options menu to have custom pathfinding to respective self.
