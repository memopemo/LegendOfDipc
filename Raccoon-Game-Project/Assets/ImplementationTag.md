# Implementation Mental Tag Team Notes

Add to the top of the list for the most recent entry.

## Date: from IDK when to November 20th

Holy shit i forgot to write in this. Sorry me, but i didnt make any information about the game since July 23rd i guess! :(

- Added Cutscene Basis and a janky opening cutscene
- Added start of Warp Pipes and selections
- **Added Title Screen!!!**
- Reworked how the game saves info between scenes to be more of a "type" of transition than figuring out which transition was made.
- This also means that Room objects now do the heavy lifting of placing things where they are supposed to be when the scene loads, like
  - "if its a exit, where do we put the player?"
  - "if we are reloading our save file, animate him jumping out"
- Added snorkeling scenes underwater! You can dive down into a sub area of any level and swim, and surface back to the main area in a different place!
- Made a new UI look
- Added scene sections for the overworld, so now towns are loaded ontop of the overworld but are in a different scene (and also deals with things leaving the area)
- Added NES font and custom "Big" pixel font.
- Added death screen!
- Added prefabs for UI stuff
- Made shaders for 3D Glasses and wavy underwater stuff!
- Made pallete changing shader!
- Reworked death screens to not be buggy
- Added Game Definitions for defining extremely top-level constants like pixels per unit
- Added Chests!~
- Added Chest contents, opening animations, player-revealing item animations, and sounds for all of them
- Added Loading a game from save file
- Pitable objects recognize being heightless
- Added shiny collectable objects like toilet paper, keys, heart containers, keys, and pendants!
- Player can accept being in a state already prior to awake
- Pushable/pullable objects now have events for starting, updating, and ending a push/pull.
- Added Secrets!
- Pretty much EVERY ITEM except the Super Magic Wand is in a working state.
  - Note the word "WORKING". These arent like in a finished state AT ALL yet.
  - Anims still need to be made
  - edge cases still need to be fixed
  - Transitions and timing still need to be adjusted/added
- Added foundation for buffs! They work!
- removed golden hearts bc i have no use for them
- Added bare bones shield and boomerang functionality (no anims)
- Added Day-Night cycle to SampleScene (which im thinking is gonna be the overworld now)
- Added Player house barebones stuff
  - items frames to reequip old previously gotten items
  - demon just lurkin (where the buffs will be selected)
  - furniture layout thingy (not started at all)
- Added my own programatic cutscene creator (for simple stuff like secrets and focusing the camera)
- Added Game End Door
- Updated some objects to not use light (light is triangle intensive!!!)
- Added color pallete animator

## Date: July 19th through 23rd

I was mostly inspired when Jack told me I should really work on this more.

- Added Shootable Bombs
- Added Hookshot
- Added Hookshot block
- Added some player animations for both
- Added NoiseMaker, a per-object sound library
- Utilized Unity's Built-In Audio Mixer
- Added lever sound effects
- Added step button sound effects
- Added default enemy sound effects
- Added player grass step sound effect
- Added UI sound effects
- Added SimpleAnimator2D and its data class, which allows non-directioned objects to use animation.
- Added transition for inventory screen (fly up, then fly down)

## Date: May 6th through ???

- Added Bombs
- Uhh i forgot
- Tried to work on paths (man fuck this)

## Date: May 3rd?? through May 6th

- Added Money drops
- Added Save and refill bins
- Added Poison and Stun Clouds
- Added Ability of Using Items
- Added Food Items
- Added using CSV files for parameters
- Added turning off player rendering
- Renamed "Damage Flash" to just "Flasher" for more usability across objects.
- Removed "Common" Library and split into multiple classes.

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
