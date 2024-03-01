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
