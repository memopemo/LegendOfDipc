# RPG Mechanics

The [RPG-Boy](obtainables.md#rpg-game) allows the player to enter a simplified, turn-based-combat with a compatible enemy.

## Pros and Cons of use

### Pros

- The player can get more money, providing an easier way of mid-to-late-game money grinding.
- The player can obtain cheap healing items in a pinch.
- Provides an opportunity for less mechanically-skilled players to take on tougher enemies.

### Cons

- Takes longer to defeat enemies than just slashing them.
- Cannot challenge some enemies like mini-bosses and main-bosses.
- Enemies on the overworld can instead become a pack that must be dealt with.

## Challenging an enemy

1. Player uses item
2. A line is made in the direction the player is facing
3. The first compatible enemy found in the path is selected for challenging
    - If no enemy is found, challenging is cancelled.
4. The player enters an RPG encounter with the challenged enemy.

### Graphics

1. Player points and shoots a line of energy.
2. A Canvas graphic of the player and enemy challenged (if any) is drawn
    - if no enemy is found, the player makes a surprised/dissapointed face that there is
    no enemy to face.
3. Graphic grows vertically to cover the screen
4. Battle screen is loaded and faded in.

## Battle System

### Player Actions

#### Attack

Attacks the enemy with current sword

#### Key Item

Uses key item against enemy (effects vary)

#### Consumable Item

Uses consumable item against enemy (either heal player or bomb enemy for damage)

#### Defend

Uses Shield To Defend for the turn.

#### Run

Attempts a run from battle.

### Enemy Actions

#### Enemy Attack

Attacks player.

#### Special Attack

Attacks player with a unique attack.

### Statuses

#### Paralyzed

Has a chance of not being able to attack for the given turn.

#### Poisoned

Takes damage over time for a brief period.

#### On Fire

Take damage over time for a brief period. hurts more but lasts shorter.

## Stats

### Player

- Health: Determined by Heart Containers
- Speed: Determined by Number of Bosses Defeated
- Attack: Determined by Number of Bosses Defeated
- Defense: Determined by Number of Bosses Defeated

### Enemies

Enemies stats are determined by the number of bosses defeated, but each enemy has a unique set of modifiers that add/subtract their base stats.

Current Dungeon Enemies follow the players stats, but enemies encountered in previous *dungeons* keep the level they were completed in.

Same concept for [Enemy Scaling](enemies.md#enemy-scaling)

This helps give a feeling that the player is progressing at a perceptable pace; When they enter previously beaten areas, they can easily destroy all the enemies in it.

## Player Item Use

Here are the effects of battle for when the player uses a key/consumable item.

|Item            |Description                                                     |
|----------      |------------------------------------                            |
|Lighter         |50% chance of setting enemy on fire                             |
|Wall Shield     |Defense +3                                                      |
|Ram Helmet      |Attack +3                                                       |
|Wand            |[Magic Attack](#player-magic-attacks-enemy)                     |
|Super Wand      |[Magic Attack](#player-magic-attacks-enemy) With x2 base damage |
|Tape Measurer   |Steals Item Held By Enemy                                       |
|Ice Boots       |Attack Enemy with +5 base damage                                |
|Bomb            |Attacks Enemy with +7 base damage                               |
|Bottle Rocket   |Attacks Enemy with +8 base damage                               |
|Roman Candle    |Attacks Enemy 3x with +7 base damage                            |
|Molotov Cocktail|Attack Enemy with +5 base damage                                |
|Stun Bomb       |Stun Status Set On Enemy                                        |
|Poison Bomb     |Poison Status Set On Enemy                                      |
|Any Food        |Heals by regular food health given x2                           |
|Flashlight      |dunno|
|3D Glasses      |dunno|
|Drone           |dunno|
|Magnet          |dunno|
|Umbrella        |dunno|
|Bomb Drone      |dunno|
|Hover Feather   |dunno|
|RPG-boy         |for the love of god dont make this a recursive joke|
|Wall Staples    |dunno|

## Calculations

### Player Attacks Enemy

    Damage = Base.damage + Player.attack - Enemy.defense

### Enemy Attacks Player

    Damage = Enemy.attack + Enemy.modifier - (Player.defense - Armor.protection)

### Player Magic Attacks Enemy
