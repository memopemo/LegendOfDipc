# Enemies

Enemies are killable objects that exist for a few things:

1. Provide challenges for the player to overcome
2. Provide resources like [Money](obtainables.md/#money), [Health](obtainables.md/#health), and rarely [Consumables](obtainables.md/#consumable-items) and [Key Items](obtainables.md/#key-items)
3. Add personality, variety, and surprise to the game's world.

Can be found in:

- Overworld
- Dungeons
- Houses

Overworld enemies spawn less frequently and in non-civilized locations (not human towns)
More enemies spawn at night

- pre-set enemies spawn during day
- random enemies spawn at night

Dungeon Enemies spawn in rooms in packs

The hurting and death of an enemy should be satisfying.

- Satisfying death sound
- Satisfying visual flair (big poof)
- Flashy effect when hit
- Study Zelda 1's hit and death effects.
  - Instead of disappearing with a "poof" they disapear with a bang, like popping a balloon.

## Dungeon Enemy Packs

These are invisible objects that keep track of enemies in a single room.

Enemies must be contained in the room, they cannot leave.

Certain things can happen when one is cleared of all enemies:

- Open a [Kill-All-Enemies Door](obstacles.md#clear-all-enemies-doors)
- Spawn loot in a preset area
  - Can be chests, keys, hearts, anything.
- Place/Remove blocking objects
  - More versatile dungeon design
- Toggle or activate certain features of objects
  - Like pushable blocks

## Properties of Each Enemy

### Enemy Health

Allows it to be killed when health reaches 0.

### Loot Group

What it drops frequently.

This is so players can get an inkling of what enemies drop. For example when a player is like "im hurting for money" they can kill enemies that drop more money.

For player's convenience, all loot groups must be within easy reach.

#### Loot Groups

- Money/Health (default)
- Money (drops big money)
- Health (drops big health)
- Explodey (Drops Bombs)
- Stinky (drops stink/poision bombs)
- Trashy (drops trash food)
- Big Health (more likely to drop Fairies)

### (Optional) Required Drops

Loot that will always drop upon the enemy's death.

Examples: dungeon keys, bombs, etc

### Enemy Scaling

Because [Dungeons](areas.md#dungeons) can be beaten in any order, we have to make sure enemies are scaled according to the players progress.

Enemies scale their health by how many [Bosses](bosses.md) the player has defeated. (or keys? or pendants? or heart containers? im not sure what it should scale by)

Enemies also do not scale and keep the level if they were found at in previous dungeons that have been beaten already.

- For example, the Xth dungeon the player has defeated will have enemies at level X.

This allows the player to percieve enemies as incredibly easy if they backtrack and go through it again.

The health scaling is linearly. +(CONST * Level) health

## Overworld Enemy List

### Deemlit

![img](imgs/enemies/en_1.gif)

Health:

Loot Group:

Action:

- Moves around randomly.

Purpose:

- essentially plays the role of the goomba
- First enemy the player encounters
- Punching bag
- underling servant for other enemies
- modular, can create other types of enemies by modifying it

### Grass Mound

![img](imgs/enemies/en_3.png)

Health:

Loot Group:

Action:

- on spawn, grow out from ground and grow mouth
- Follows player

Purpose:

- other standard enemy
- sorta tough but manageable

Location:

- [Forests](/design-doc/overworld.md/#forest)
- anywhere theres natural green grass essentially.

### Cursed Grass Mound

![img](imgs/enemies/en_7.png)

Health:

Loot Group:

Action:

- similar to [Grass Mound](#grass-mound)
- __on spawn, wait until player gets near and then grow__
- Follows player

Purpose:

- other standard enemy
- sorta tough but manageable

Location:

- [Cursed Forest](/design-doc/overworld.md/#cursed-woods)

## Dungeon Enemy List

### Slime

![img](imgs/enemies/en_5.gif)

Health:

Loot Group:

Action:

- Moves around randomly

Purpose:

- Common enemy

Physical Description:

- A colored slime with a pair of eyes on it

---

### Skeleton

![img](imgs/enemies/en_2.gif)

Health:

Loot Group:

Action:

- Walks around
- Sometimes walks towards player
- Avoids Walls

Purpose:

- Common enemy

Physical Description:

- A simple animated and short skeleton, about the same height as the player.
- Weilds a single dagger.

---

### Weak-looking Magic Enemy

Health:

Loot Group:

Action

- Runs around (4 directions)
- Occasionally charges, then shoots a single magic projectile.

Purpose

- Common Enemy

Physical Description

- A little mage

---

### Lava Enemy

Health:

Loot Group: Bombs

Action

- Struts around (4d)
- if there are no fire puddles, shoot out a lava bubble that creates a fire puddle
- fire puddles are circular areas that set the player on fire.

Physical Description

- A walking and clumsy looking volcano with a face.

---

### Bat

![img](imgs/enemies/en_4.gif)

Health:

Loot Group:

Action

- "Flies" around (4d)
- Can either be above or at ground level (Switches between them periodically)
- Can only be hit if the player is also at the same level (airborne or ground level)
- Can be used for jumping gaps maybe?

Physical Description

- A red bat with fangs and a devilish smile

---

### Hoppy Boi

Heath:

Loot Group:

Action

- Jumps around (4d)

Physical Description

---

### Wall Shooter

![img](imgs/enemies/en_6.gif)

Heath:

Loot Group:

Action

- shoots a projectile from a wall, then jumps off and spins around until it hits another wall.

Physical Description

- spider-like thing with a giant eye
- eye glows
- blinks twice before shooting
- blinks and crouches before jumping

---

### Shield Dog

![img](imgs/enemies/en_10.gif)

Heath:

Loot Group:

Action

- walks around
- stabs if near player

Physical Description

- pink dog
- holding shield and strapping shield to tail
- knife

---

### Snake Enemy

![img](imgs/enemies/en_9.png)

Heath:

Loot Group:

Action

- moves 1 block towards player periodically in a grid-fashion
- stabs if near player

Physical Description

- snake

---

### 3-Way Shielder

![img](imgs/enemies/en_8.gif)

Heath:

Loot Group:

Action

- walks around

Physical Description

- guy with a shield covering his front and sides, but not his butt.
- his big, juicy butt.
