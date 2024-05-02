# Implementation Mental Tag Team Notes

Add to the top of the list for the most recent entry.

## Date: April 29th - May 1, 24

oh god uhh

- made turning
- added stopping running
- added collectable hearts and money
- added freezing
- removed old player script
- changed destroying to disabling
- added a cleanuper? does it work?
- Broke up "libraries"
- Made consts for animations.
- Broke up Readme into multiple docs.

## Date: March 8, 24

- Added jumping off sides of ledges to lower areas
- Made normal dungeon heights 2 blocks tall instead of 1.
- Formatted Z-layers so things look good
-

## Date: March 7, 24

- Edited dungeon tilemap.
- Added Layers to Dungeons.

## Date: March 3 - 6, 24

More things:

- Enemy Health Bar
- Demon Key Object
- Error room
- Organization of assets
- Organization of scene objects
- Money Counter
- Money in save file

TODO: Fix Dungeon Tileset in Unity

## Date: March 2, 24

This is sort of where I leave off my current status of implementation so I can start from where I left off.

Last thing I was doing was implementing damage and health from both the player and damageable entities like enemies.

Also knockback. It is currently working.

Specifically in PlayerStateManager.cs where I was implementing player knockback.

Other stuff I did today:

- Added more enemy configuration/customization
- Made enemies and the player flash when taking damage (white for enemies and red for player)
- Replaced the normal srender pipeline with a scriptable render pipeline so that I can have bloom and other effects :)
