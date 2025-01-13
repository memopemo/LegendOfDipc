# Implementation Notes

## Explosive Bomb Items

### M80

1. thrown in direction of player
2. is airborne and rolls a little after landing
3. wait after a while
4. instantiates Explosion

### Shooter

1. attaches to the player
2. sparkles for a bit
3. after some time shoots one rocket
4. rocket Travels straight and explodes on collission with wall or object (not player)

### Roman Candle

1. attaches to the player
2. sparkles for a bit
3. after some time shoots one rocket
4. after some more time, another one
5. after more time, another one

### Molotov Cocktail

1. thrown in the direction of player
2. on land, spawn one fire area
3. after some more time, spawn 4 more around that one fire area in a + shape
4. after some more time, spawn 8 more around that
5. after some more time, spawn 12 more around that

## Game Dev Progression

We need to make the SampleScene fully featured before we start making more levels.
Now, this isnt the full story, but we need to get to the point either:

1. is able to have features easily built in and does not need old code to be edited to make things work.
2. is just fully done and does not need any more updates.

This also doesnt apply to one-off/relegated things like the buff demon or pipes.
Just interactions between objects.

I should also make a list of every interaction between objects and describe what is expected to happen.
Even interactions that may not occur. It'd be hard, but we should consider like spamming UI opens and closes too or events that are unlikely to happen but could still happen.

Probably not, but eh.

I think its also good that we are splitting gamedev from level design so that as we add more features it doesnt muddle up the level design.
What I think is just our guiding principle is reducing changes to old code as much as possible.
