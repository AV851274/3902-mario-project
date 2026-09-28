Code Review of Player.cs by Justin Kumar
Code authored by: Aakash Vatti (several people have contributed to this file, but he made the initial base/structure)
Day of Code Review: 9/28/2026
Sprint 2

Minutes spent on this review: 77

Readability:
The file is overall very easily readable. Descriptive variable names are used throughout and the vast majority of functions are very short. As an example, take MoveRight: it first checks if isOnGround is true and velocity.X < 0 (setting this to isSkidding). Then, it increases velocity, and it does so by more if isSkidding is true. Last, it sets facingDirection to Direction.Right. It is very easy to translate this function into English to explain what it's doing:

Mario accelerates in the direction he intends to move. If Mario is moving the opposite direction (left, when MoveRight is called) and touching the ground, he should skid and accelerate more (since friction is working in his favor). He should also face the direction that he is intending to move.

With lots of useful variable names and relatively simple logic kept in each function, conceptual understanding of what each function does is very easily attainable simply by reading the code directly.

There are, however, a couple things that could be done to improve readability further:
1) Put more of the Update code into other methods. Update is the longest function by far right now, and it achieves enough different things (gravity, changing position with velocity, adjusting the isOnGround variable, and updating Mario's sprite) that it can take a bit for a reader to understand all of it. An easy way to do this would be to move the sprite logic to its own function, which would roughly halve the lines occupied by Update().
2) Put more comments in the code. This is far less necessary because the variable names are so straightforward and effective, but comments always help with readability and this file has almost none.


Maintainability/Quality:
As mentioned in the section on readability, this file makes use of many short functions and specific, effective variable names throughout. This helps a lot with being able to maintain code - for example, it was easy for me to modify the movement functions to implement acceleration because just reading the code enabled me to know exactly what it was doing before. And when Aakash implemented skidding afterwards, he was able to very quickly build off what was already there as well.

As a result, even though there's a lot of functions, the code is kept simple enough in each of them to make it relatively easy to modify what's there or build new functionality on top of it.

One note about this file, though, is that it does not implement any swimming logic as that is kept in a separate SwimmingPlayer class. While the functionality of the land player is easy to maintain with the current setup, it can be annoying to implement functionality that applies to both the swimming and walking player, as it has to be copied and pasted into the other file, then modified further if the two classes implement something relevant differently.

Hypothetical change: one change which will at some point not just be hypothetical is implementing player death. I think this could be somewhat challenging, but there are a lot of tools in this file to make it easier. The main issue I foresee is that the Player can't access the Game1 to tell it what to do, but there needs to be a death animation before the game resets. So, the entire death animation has to play using sprite logic added to Player.cs, but this logic doesn't have a way to directly tell the Game1 that the animation finished and the game can be reset. Here is a way this could be resolved:

The Player class could have a function added to return if the player is dead or not, which could be used by either Player or Game1 to tell if the player JUST died, initiating the animation that could be continued with code in Player. At the end of the animation, the Player could change a variable returned by a separate function that tells Game1 to reset the game. This implementation seems somewhat annoying and not ideal. However, I came up with it in a very short amount of time (and there are therefore probably much more efficient solutions), and most of the individual functions themselves still wouldn't be particularly complex. So, I would ultimately say this change would only be moderately challenging to support with the current implementation, as it's easy to come up with an idea that works but more difficult to come up with something that is similarly straightforward when compared to the rest of the code in Player.cs.
