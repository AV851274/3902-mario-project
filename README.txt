CURRENT SPRINT: #2

Current features (With Controls):

| Key | Action |
|-----|--------|
| W/A/D | Move Mario |
| S | Crouch (Only big mario) |
| 1 / 2 / 3 | Small / Big / Fire Mario |
| 4 / 5 / 6 | Small / Big / Fire swimming Mario |
| F | Shoot fireball (Fire Mario only) |
| E | Damage Mario |
| O / P | Cycle enemies |
| K | "Stomp" enemies |
| T / Y | Cycle blocks |
| U / I | Cycle items |
| Q / R | Quit / Reset |
Animations for all sprites that require such
Multiple blocks that can be cycled
Multiple Items that can be cycled
All mario types
Three enemies that behave accordingly
Gravity and Water physics
Proper mario movement

Our current "bug" is the sizing of enemies according to mario. As some are a little too small.

Credit for Other Mario Sprites:
This sprite:
https://www.mariouniverse.com/wp-content/img/sprites/nes/smb/mario-2.gif
From this website:
https://www.mariouniverse.com/sprites-nes-smb/
Using this to remove the background:
https://ezgif.com/remove-background/ezgif-2e049edabdd9f560.gif.html
Using this to find the pixels for each sprite:
http://www.spritecow.com/


## ✅ Roslyn check

- Build succeeded
- No compiler errors were reported
- The project in `Monogame.csproj` is currently compiling cleanly through Roslyn

Code Review Generalized: 0 ERRORS 109 analyzer warnings
After running a code check, there were many minor errors within the code. Nothing that broke the code but more
stylistic decisions (CA1805). Like setting booleans = false when you can just create them and they auto set false. We cleaned up many
of these small issues, with some warnings being too vague and unnecesary of a change.

While many of these issues are small, the team is dedicated to lower these warnings as the weeks go on.


The team used AI agents to help handle bug fixing and to help wrap our heads around the tasks at hand. Claude Code played a role
in reviewing code errors and cleaning up the project as well. Making sure to not let it go wild as the team needed to understand
and have readability stay strong in the project.

SPRINT 2 REVIEW:

Sam - Overall Sprint 2 was a bit of an eye opener for the team. I think that we started slow and rocky, without much of a plan in mind
which really hurt the performance at first. But slowly the team gathered and understood what needed to be done. Which vastly improved
production of Sprint 2. Tasked were split relatively evenly with teammates offering to take on work with little pushing.

There were many commits to this sprint (84 to be exact), not all were reviewed by teammates as they were very minor 1-10 line changes but
the major changes were asked upon to be reviewed to ensure the code is clean and proper. This is reflected in the code review not for each
pull request but for the overall file and class that resides.

CODE REVIEW LIVES WITHIN "CODE REVIEW" FOLDER