# Save File

The save file is located in AppData.

## Format

GAME TITLE Checked along with number of lines in file to determine if the file is valid. (Could do a checksum?)

data1

data2

data3

data3.1

data3.2

data3.3

...

Some values only span one line, while lists of data can span multiple lines.

Valid Types:

- Number: any number

- Float: any decimal number

- Boolean: nothing (new line character) or space character.

- Special: Numbers that hold meaning (like an ID)

  - "Nothing" value is an empty line (only the new line char)

It holds information about:

## Time Played

Type: Number

- Ticks Once per second while player has control.
- Formated as [hour]:[min]:[sec]

## In Game Time

Type: Number

- Current [In-Game Time](day-night-system.md)

## Secrets Found

Type: Boolean List

- Every secret is assigned a boolean.

## Save Point Location

Type: Number

- Each [Save Point](overworld.md#save-point)

## Obtained Key Items

Type: Boolean List

- Each [Key Item](obtainables.md#key-items) is assigned a boolean.

## Unclogged Drain Pipes

Type: Boolean List

- Each [Drain Pipe](overworld.md#drain-pipes) is assigned a boolean. (False = Clogged, True = Unclogged)

## Saved Inventory

This holds all information about what the player currently has in their inventory, including what in each slot and how much.

### Type of Consumable Item In Inventory

Type 1: Special List

- Each Inventory Slot has an ID that shows what [Consumable Item](obtainables.md#consumable-items) is in that slot.

### Amount of Consumable Item In Inventory

Type 2: Number List

- Each Inventory Slot has the amount that is in that slot.

## Saved Equipment

### Current Sword

Type 1: Special

- Current Sword ID

### Current Shield

Type 2: Special

- Current Sheild ID

### Current Armor

Type 3: Armor

- Current Armor ID

## Configuration of House's Items

Type: Special List

- A list of the ID of each square tile for their collectables.
- If any values are not valid (eg 1 half of a couch, or inside of the summon circle), then do not show.

## Obtained Collectable items for House

Type: Boolean List

- Every Collectable is assigned a collected flag

## Obtained Swords

Type: Boolean List

- Every Sword is assigned a boolean.

## Obtained Shields

Type: Boolean List

- Every Shield is assigned a boolean.

## Obtained Armor

Type: Boolean List

- Every Armor is assigned a boolean.

## Obtained Boomerangs

Type: Boolean List

- Every Boomerang is assigned a boolean.

## Obtained Demon Pendants

Type: Boolean List

- Every [Pendant](obtainables.md#demon-pendants) is assigned a boolean.

## Obtained Demon Keys

Type: Boolean List

- Every [Demon Key](obtainables.md#demon-keys) is assigned a boolean.

## Dungeon Information

These are for each dungeon. (0-F)

### Boss Defeated

Type: Bool

- Is the [Boss](bosses.md) defeated?

### Mini-Boss Defeated

Type: Bool

- Is the [Mini-Boss](minibosess.md) defeated?

### Boss-Key Obtained

Type: Bool

- Is the [Boss Key](obtainables.md#boss-key) collected?

### Skele-Key Obtained

Type: Bool

- Is the [Skeleton Key](obtainables.md#skeleton-key) obtained?

### Keys Obtained

Type: Boolean List

- Every [Key](obtainables.md#regular-key) is assigned a boolean.

### Chests Opened

Type: Boolean List

- Every [Chest](treasure-containers.md) chest is assigned a boolean.
