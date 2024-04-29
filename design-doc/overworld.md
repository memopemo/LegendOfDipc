# Overworld

## Common Overworld Game Mechanic Objects

These are objects that are commonly used throughout the Overworld for the benefit and convenience of the player.

### Save Point

A Recycling Bin that can be interacted with. When interacted with, controls are disabled (including inventory and pausing), the player character jumps into the recycling bin, and a prompt pops up that asks the player if they want to save. Yes, save. No, do not save.
After either option, the player character pops out of the recycle bin and control is handed back to the player.

### Health Refill Point

A Trash Bin that can be interacted with. When interacted with, controls are disabled (including inventory and pausing), the player character jumps into the recycling bin, health is fully restored, and the player character pops out of the trash bin and control is handed back to the player.

### Campfire

Allows the player to skip to various parts of the day. Found in animal villages, and various parts of the Overworld.

Must be set on fire to be activated and able to be interacted with. Once the player interacts with it while active, the player sits down and a prompt appears that asks the player when to wake up.

| Name     | Time | [IGT](day-night-system.md#in-game-time) |
|----------|------|----------------------|
| Midnight | 12am | 0s                   |
| Sunrise  | 6am  | 360s                 |
| Noon     | 12pm | 720s                 |
| Sunset   | 6pm  | 1080s                |

Once selected, the screen fades to black, the time is set to the correct time (in in game time values), and the campfire is deactivated.

- Skipping through the day does not affect time played.

The player character wakes up, gets up, and control is returned to the player.

## Overworld Regions

### Forest

### Neighborhood

### Cursed Woods

### City

## Overworld Points of Interest

### Animal Villages

These are refugees for the player to rest at. No enemies can approach the villages.

#### Shops

#### Campfires

See [Campfires](#campfire)

#### Animals

#### Animal Houses

### Player's House

#### Equipment Display

A wall shows the player all of the equipment they have collected but are not equipped.

- By interacting with one of these, it shows a prompt to switch current equipment with the new equipment (Default: yes) and then shows an animation of the player taking the equipment and placing their old equipment on the wall.
  - NOTE: this effect is only temporary while the room is still loaded. Once the player exits and re-enters, the wall will be re-sorted.
  - How this can be done is: have the equipment just change its image to the old equipment when the player stuffs the new one away.

#### Summoning Circle

The more [Demon Pendants](obtainables.md#demon-pendants) the player collects, the more access they have to [Demon Buffs](demon-buffs.md).

- The circle appears in the house after collecting 1 pendant. (Maybe some other way?? A special item?)
- When interacted with, a gigantic buff demon sticking his head from hell through the portal appears and allows the player to choose buffs.

### Drain Pipes

These allow the player to quickly traverse to other Drain Pipes throughout the world.

- By default, these are clogged up and can be interacted by the player, showing the player character pulling on the sticks, dead grass and junk clogging the drain pipe and unclogging it, allowing the player to enter them.

- When entering, a [Drain Pipe Selection Screen](ui.md#warp-screen) shows up, and allows the player to select one.

- The player can only traverse through unclogged drain pipes.
  - Once unclogged (doesnt need to be entered), the player can use the drain pipe from anywhere.

- After either choosing a new drain pipe to enter to, or backing out, it shows an animation of the player leaving the drain pipe they have selected (or the same one they entered.)

## Overworld Obstacles

### Ledges

These are things the player can jump down, and sometimes jump back up (if boots have enough height.)

![Alt text](imgs/ledges.png)

### Saxophone Regions

These are hidden regions that respond to when used with the [Saxophone](obtainables.md#saxophone)

![Alt text](imgs/sax_region.png)

### Bombable Walls

These are hidden holes filled with rock that are destructable with bombs.

![Alt text](imgs/bombable_wall.png)

### Burnable Bushes

These are bushes that can be burned to find hidden entrances.

### Grass

While all grass is cuttable, some cuttable grass may contain hidden entrances.

### Pits

These are pits the player can fall into and take damage. They respawn at the place before they fell in.

![Alt text](imgs/pit.png)

## Underwater

### Underwater Caves

### Seaweed
