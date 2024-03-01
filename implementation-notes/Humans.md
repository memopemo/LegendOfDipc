# Humans
- Kick the player when near or walk away
    - After 3 attempted provokings by the player, start angrily running towards player and kicking them
- Walk to random points of interest in city
    - Wont change target until completion
- 16 variants


# Cars
- Insta-kill the player if they are moving fast enough.
- Follow paths to and from places on a given day
    - Mostly park somewhere, spawn a human (exiting the car), and then when the human comes back, drive back home.
    - Two Places: Inside Garage, Parked somewhere
-  
# Houses
- Have set times where the player is not allowed in.
    - Also called "human's at home"

## When the time of day reaches when the human is supposed to enter the home:
If player is outside the house:
- Spawn a car (same car everytime)
- make the car target the garage door
- drive up
- Open garage (player is blocked from entering at this point)
- Drive in garage
- Close garage
- Destroy Car Cutscene Object
- After a minute, house lights up

If player is inside the house:
- Play a far-away garage opening sound
    - To let player know their time is almost up
- After 1 minute, play a door opening sound
- If player is in a state where they dont have control, wait for that lack of control to end (stop control in the meantime anyway)
- Player and game world stops immediately
- Play a cutscene where the player is freaked out and then scurries off to the right
    - Doesnt matter if theres a wall im not gonna solve that
    - Draw player ontop of everything
