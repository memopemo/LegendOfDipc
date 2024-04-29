# Day/Night System

Secrets, entering houses, and events can occur based on the time of day.

The time updates if the player is in the overworld, otherwise, it stands still.

- Day Length:
  | In Game (IG)   | Real Life  |
  |----------  |----------- |
  | 1 Minute   | 1 Second   |
  | 60 Minutes | 60 Seconds |
  | 1 Hour     | 1 Minute   |
  | 24 Hours   | 24 Minutes |
  | 1 Day      | 24 Minutes |
  | 1 Day      | 1440 Seconds |

## In Game Time

This is how In Game Time (IGT) is kept track of and stored in game.

- Time is incremented every second, and when it reaches 1440, it resets back to 0.
- IG Minute = (IGT) % 60
- IG Hour (24) = floor( (IGT) / 24 )
- IG Hour (12) = floor( (IGT) / 12 ) and if IG Hour (24) > 12, PM, else AM.

## Time Ranges

Periods for the ability to do things is not at an instantaneous second in time.

The smallest possible period is 15 minutes IGT (15 seconds)

We can represent these with two numbers in increments of 15 minutes IGT.

- Min = 0  = IGT 0-15
- Max = 95 = IGT 1425-1440

### Wrapping

If the beginning time is greater than the end time, a wrap-around happens.
when this occurs, check if the current time is between the beginning time and 95, OR 0 and the end time. If so, then it is valid.

### Time Toggles

We can also have objects toggle a flag in themselves when the time reaches a certain value, and toggle it back off when it reaches the next value. They must, however, set if they are toggled on or off with a function that checks if the value is within range with [Wrapping](#wrapping)
