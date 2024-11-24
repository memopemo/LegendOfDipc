# Progression

Goal: progression should be as non-linear as possible, allowing the player to take the most different paths.

Method: Allow the player to skip dungeons by finding different items.

## Item Labels

An example of a Specialized Action item would be the Power Bombs in Super Metroid.

An example of a General Movement item would be the Gravity Suit in Super Metroid.

TODO: Balance this out so its fun

| Symbol | Location | Function | Mood | Item |
|-|-|-|-|-|
| 1   | Dungeon 1 | Specialized Action | Filler | [Lighter](/obtainables.md#lighter) |
| 2   | Dungeon 2 | Specialized Action | Awesome | [Lifting Gloves](/obtainables.md#lifting-gloves) |
| 3   | Dungeon 3 | Specialized Action | Awesome | [Ramming Helmet](/obtainables.md#ramming-helmet) |
| 4   | Dungeon 4 | Specialized Action | Filler | [Tape Measurer](/obtainables.md#tape-measurer) |
| 5   | Dungeon 5 | Specialized Action | Awesome | [Snorkel](/obtainables.md#snorkel) |
| 6   | Dungeon 6 | Specialized Action | Filler | [Umbrella](/obtainables.md#umbrella) |
| 7   | Dungeon 7 | Specialized Action | Awesome | [Wall Staples](/obtainables.md#wall-staples) |
| A   | Overworld | Specialized Action | Filler | [Saxophone](/obtainables.md#saxophone) |
| B   | Overworld | Specialized Action | Filler | [Flashlight](/obtainables.md#flashlight) |
| C   | Overworld | Specialized Action | Awesome | [3D Glasses](/obtainables.md#3d-glasses) |
| D   | Overworld | Specialized Action | Filler | [Wall Shield](/obtainables.md#wall-shield) |
| E   | Overworld | Specialized Action | Awesome | [Magnet](/obtainables.md#magnet) |
| F   | Overworld | Specialized Action | Awesome | [Ice Boots](/obtainables.md#ice-boots) |
| G   | Overworld | Specialized Action | Filler | [Hover Feather](/obtainables.md#hover-feather) |
| I   | Overworld | General Movement   | Awesome | [Jump Boots](/obtainables.md#jump-boots) |
| II  | Overworld | General Movement   | Filler | [Climbing Gloves](/obtainables.md#climbing-gloves) |
| III | Overworld | General Movement   | Filler | [Flippers](/obtainables.md#flippers) |
| IV  | Overworld | General Movement   | Awesome | [Super Jump Boots](/obtainables.md#super-jump-boots) |

Items not needed for progression

|Item|
|-|
|[RPG-Boy](/obtainables.md#rpg-game)|
|[Magic Wand](/obtainables.md#magic-rod)|
|[Super Magic Wand](/obtainables.md#super-magic-rod)|
|[Drone](/obtainables.md#drone)|
|[Bomb Drone](/obtainables.md#bomb-drone)|
|[Enemy Cloak](/obtainables.md#enemy-cloak) |

## Progression Table (player side progression)

| When Get Item | Opens Acces to Items |
|-|-|
| 1   | 2, A   |
| 2   | 3, F   |
| 3   | 4, G   |
| 4   | 5, I   |
| 5   | 6, II  |
| 6   | 7, III |
| 7   | A, IV  |
| A   | B, 3   |
| B   | C, 4   |
| C   | D, 5   |
| D   | E, 6   |
| E   | F, 7   |
| F   | G, A   |
| G   | I, B   |
| I   | II, C  |
| II  | III, D |
| III | IV, E  |
| IV  | 3, F   |

## Lock Table (developer side)

| Item | Can Be Accessed by Using |
|-|-|
| 1   | Nothing   |
| 2   | 1   |
| 3   | 2, A, IV   |
| 4   | 3, B   |
| 5   | 4, C  |
| 6   | 5, D |
| 7   | 6, E  |
| A   | 7, F, 1   |
| B   | A, G   |
| C   | B, I   |
| D   | C, II   |
| E   | D, III   |
| F   | E, IV   |
| G   | F, 3   |
| I   | G, 4  |
| II  | I, 5 |
| III | II, 6  |
| IV  | III, 7 |

Essentially, A-IV is the bypasser of 3-7

## Shortest Path To Each Item

|Item |Length| | Longest Item | Length | Reason For Item |
|-----|---   |-|--------------|--------|-----------------|
| 1   | 0    | | IV           | 7      | Awesome         |
| 2   | 1    | | III          | 6      | Filler          |
| 3   | 2    | | 7            | 6      | Awesome         |
| 4   | 3    | | II           | 5      | Filler          |
| 5   | 4    | | E            | 5      | Awesome         |
| 6   | 5    | | 6            | 5      | Filler          |
| 7   | 6    | | I            | 4      | Awesome         |
| A   | 1    | | D            | 4      | Filler          |
| B   | 2    | | 5            | 4      | Awesome         |
| C   | 3    | | G            | 3      | Filler          |
| D   | 4    | | C            | 3      | Awesome         |
| E   | 5    | | 4            | 3      | Filler          |
| F   | 2    | | F            | 2      | Awesome         |
| G   | 3    | | B            | 2      | Filler          |
| I   | 4    | | 3            | 2      | Awesome         |
| II  | 5    | | A            | 1      | Filler          |
| III | 6    | | 2            | 1      | Awesome         |
| IV  | 7    | | 1            | 0      | Filler          |
