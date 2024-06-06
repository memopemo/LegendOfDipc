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
