# Dungeons

## Dungeon Rooms

Contain:

- What exits there are
- Black mask to hide current room when the player is not inside of it.
- Enemies within room
- Bounds of room

## Other Rules

- Enemies must not leave the bounds of their respective room.
- Camera bounds is the bounds of the room.
  - does a wipe movement when changing rooms.
- Objects are culled and do not move when player isnt in room.

## Getting all enemies in dungeon room

- do an operation that gets all enemies that are inside of the bounds.

## Dungeon Name Formatting

- Scene Name: "Dungeon[Dungeon Number]_Floor[Floor]"
- EG: "Dungeon1_1F"

## Pit Falling

There are 2 kinds of pits

1. Hurt Pits
    - These only hurt the player and set them back after falling in.
    - Pitch Black
2. Throught Floor Pits
    - These send the player to the next level downwards.
    - Has a parallax tiny ground texture behind it

Through Floor Pits in dungeons are handled in the following:

1. Get last digit of scene name. This is the floor number.
2. Subtract by 1.
3. Jump to scene with decremented floor number.
    - Keeps the same position when the player fell down into the next screen.
4. Place player at area with same positon, entering in a falling state way up high until he lands. (or possibly falls down another pit.)

## Layers and Collision

4 Floors
3 Walls + "ceiling" and "pit"

Unity: Use 4 Layers of collision:

- Bottom
- Middle
- Top
- Ceiling

---

***THE CEILING LAYER IS FOR SECRETS ONLY!!!!***

- For specific implementation we need to define these levels:
  - Default. Nothing can get past this.
  - Ceiling
  - Top
  - Middle
  - Bottom

Each one of these allow objects to *ALSO* go underneath eachother.

- *Note*: because there can only be 2 walls (WITHOUT SECRETS): Upper and Lower, the super jump boots will be able to bypass alot of 2-block high walls.

- We might need to instead have a sort of blocking system for falling down ledges later on in the game to prevent backtracking. An idea could be a ledge wall that poofs into existence.

- All enemies, specific game objects, and the player can only be on one of four layers.

### New Component: Level

- Keeps track of level, automatically sets the unity collision layer for the game object to it too.

- The Heightable component is not responsible for keeping track of the current "level" of the object. The Heightable component only keeps track of the finer grain height's within a level.
  - An example would be a flying bat on level 2 that you cant hit.

---

- Tilemap collision prevents objects on its layer from exiting its bounds.
  - EG the ceiling layer has a tilemap collider that defines collision.

- The bottom layer has pits, so it "visually" looks like a wall, even though it is on the bottom layer still.

- Same goes for the ceiling. You cannot get over the ceiling.

```txt
** Graph of collision for unity layers** 
|
|
|-Default
|-Ceiling (maybe combine these?)
----------
         |-top
----------------
               |-middle
------------------------
                       |-bottom
-----------------------------------------------------------
```
