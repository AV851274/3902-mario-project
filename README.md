CURRENT SPRINT: #3

## Controls

| Input                    | Action                            |
| ------------------------ | --------------------------------- |
| A / D or Left / Right    | Move Mario                        |
| W or Up                  | Jump (swim stroke while swimming) |
| S or Down                | Crouch (Big / Fire Mario only)    |
| Space + direction        | Dash                              |
| F                        | Shoot fireball (Fire Mario only)  |
| E                        | Damage Mario                      |
| 1 / 2 / 3                | Small / Big / Fire Mario          |
| 4 / 5 / 6                | Small / Big / Fire swimming Mario |
| Left click / Right click | Previous room / Next room         |
| R                        | Reset to the first room           |
| Q                        | Quit                              |

## Current features

- Rooms loaded from CSV files in `Content/levels/` (testRoom, level1, room2, 1-1), switched with the mouse
- A test room containing every object type
- Small, Big, and Fire Mario, plus swimming versions of each
- Mario movement with acceleration, skidding, crouching, dashing, and jumping
- Enemies: Goomba, Koopa (shell can be kicked), Para-Koopa, Buzzy Beetle, Spiny, Hammer Bro (throws hammers),
  Cheep Cheeps (green and gray), Lakitu (follows Mario and throws eggs that hatch into Spinies),
  Bowser (jumps, throws hammers, and breathes fire), Fire Bars, Podoboos (big and normal), and Bullet Bills fired from launchers
- Enemies have gravity, land on blocks, turn around at walls, and can be stomped (except Spiny, Cheep Cheeps, Bowser, Fire Bars, and Podoboos)
- Taking damage shrinks Mario, with a short invincibility flash; Small Mario plays a death animation and the room restarts
- Falling into a pit restarts the room
- Fireballs bounce along blocks and defeat enemies; enemy projectiles (hammers, Bowser's fire, Lakitu's eggs, Bullet Bills) damage Mario
- ? blocks release their item (coin, mushroom/fire flower, or star), which rises out of the block
- Big / Fire Mario break bricks; Small Mario bumps them
- Items: mushroom, fire flower, coin, and star; mushrooms and stars move and bounce off walls
- Blocks: ground, brick, ? block, hard block, pipes, trees, bridges, and cloud blocks that disappear after being stood on
- Background scenery: clouds, hills, bushes, and castles
- Camera follows Mario horizontally; taller rooms zoom out to fit the window

## Known bugs

- A kicked Koopa shell does not knock out other enemies yet.

## Deviations from the original game

- The star can be collected but does not give invincibility yet.
- Items from ? blocks rise out of the block and can't be collected until they are fully out. Coins stay on top of the block until Mario touches them instead of being collected automatically.
- Power-up ? blocks release a mushroom for Small Mario and a fire flower if Mario is already big (same as the original).
- Enemies keep moving during Mario's death animation; in the original, the game freezes.
- Switching rooms or dying reloads the room, so Mario restarts as Small Mario.
- Taller rooms zoom out to fit the window instead of scrolling vertically.
- Mario taking damage freeze frames are replaced with invincibility frames instead.

## Project organization

Runtime source is grouped under `Game/` by responsibility:

- `Entities/` contains players, enemies, items, projectiles, blocks, and background objects.
- `Interfaces/` contains shared contracts.
- `Physics/` contains the physics engine, collision handling, and the `Box` bounds type.
- `Animation/`, `Controllers/`, and `Commands/` contain animation and input handling.
- `Rendering/` contains the camera and the sprite factories (Mario, enemy, item, block, and background).
- `Levels/` and `State/` contain level loading and the collection of game entities.
- `MarioGame.cs` and `Program.cs` are the game and its entry point.

Content assets and project files remain at the repository root because the MonoGame content pipeline and build configuration use those paths. `Code-Reviews/` remains separate from runtime code.

## Physics

Every moving object implements `IPhysicsBody`. Each frame, `PhysicsEngine.Step`:

1. Applies each body's acceleration and gravity, then moves it.
2. Resolves collisions with terrain (solid blocks, and top-only platforms like bridges and clouds).
3. Resolves projectile collisions (fireballs with enemies, hammers with Mario).
4. Resolves player interactions (picking up items, stomping or being hurt by enemies).

`CollisionManager` works out which side two objects touched on and calls each object's `OnCollision`, so each object decides how to react (for example, an enemy turns around at a wall, a block releases its item).

## Credit for Other Mario Sprites:

- This sprite:
  https://www.mariouniverse.com/wp-content/img/sprites/nes/smb/mario-2.gif (From this website: https://www.mariouniverse.com/sprites-nes-smb/)
- Using this to remove the background:
  https://ezgif.com/remove-background/ezgif-2e049edabdd9f560.gif.html
- Using this to find the pixels for each sprite:
  http://www.spritecow.com/

## Code analysis (Roslyn .NET analyzers)

Analyzers are enabled in `Monogame.csproj` (`AnalysisMode = All`). Latest run (10/26/2026): 0 errors, 189 warnings.

None affect how the game runs; we reviewed each type and left them as-is for these reasons:

| Rules                          | Count | Why it's minor                                                                                                                          |
| ------------------------------ | ----- | --------------------------------------------------------------------------------------------------------------------------------------- |
| CA1515, CA1062, CA1002         | 139   | Library rules (make types internal, null-check parameters, don't expose `List<T>`). Our game is standalone and only calls its own code. |
| CA1805, CA1024, CA1052, CA1823 | 16    | Style only (explicit default values, Get-method vs property, non-static `LevelLoader`, two unused constants in `Spiny`).                |
| CA5394                         | 10    | `Random` only drives enemy behavior, not security.                                                                                      |
| CA1003                         | 10    | Our events are internal; `Action` is simpler than `EventHandler<T>`.                                                                    |
| CA1859, CA1051                 | 8     | Intentional design: we depend on interfaces, and base classes share `protected` fields with subclasses.                                 |
| CA1716, CA1724, CA1034, CA2213 | 6     | Naming/cleanup notes (`Stop` is a VB keyword, `Player` name overlap, nested `CollisionResult`, MonoGame disposes graphics itself).      |

```
~Task remaining (Y)
36 |█
   |█ ▓
30 |  █ ▓
   |      ▓   █
24 |        ▓    █
   |          ▓     █
18 |            ▓      █
   |              ▓       █
12 |                ▓       █
   |                  ▓
 6 |                    ▓     █
   |                      ▓
 0 |________________________▓_█___
    10/6      10/13    10/19    10/26
    █ actual   ▓ ideal
```

Going off a downward linear ideal line compared to our code commits. We lagged behind a little near the end of the sprint. But pulled in and finished strong to get what needed to be done in time. Ideally the team will reflect on this and have a bit more of an early push or a linear workflow through sprint 4.

```
                        LINES OF CODE:
EnemySpriteFactory.cs   ███████████████████████████████ 307
MarioGame.cs            ██████████████████████████ 264
Player.cs               ██████████████████████████ 259
LevelLoader.cs          ██████████████████████████ 259
MarioSpriteFactory.cs   ██████████████████ 181
Item.cs                 ███████████████ 147
PhysicsEngine.cs        ██████████████ 144
Block.cs                █████████████ 126
CollisionManager.cs     ████████████ 116
Fireball.cs             ███████████ 110
Lakitu.cs               ███████████ 108
HammerBro.cs            ██████████ 104
CheepCheep.cs           ██████████ 98
AnimationController.cs  ██████████ 97
Bowser.cs               █████████ 90
```

This histogram notes the 15 largerst files in the project. The main desire with this graph it to reflect on what can be refactored to be
closer to the goal of having all files be <150 lines of code. The top four files go high above this goal, the team plans on condensing these files as the sprints go to allow for the files to be the best they can be. MarioGame.cs is the most important since the main game file should be as short and sweet as possible.

The team used AI agents to help handle bug fixing and to help wrap our heads around the tasks at hand. Claude Code played a role in reviewing code errors and cleaning up the project as well. Making sure to not let it go wild as the team needed to understand
and have readability stay strong in the project.

## SPRINT 3 REVIEW:

Sam - Sprint 3 was a pretty good progressive sprint for the team. A lot was done especially with map work and collisions. The game is really starting to actually feel like a game instead of a cheap one off knock off. I think introducing a TODO list for the team helped quite a bit, this text file lives in the 'TODO/' folder.

Lots of commits to get the project good and ready for the end of sprint 3. Only missing a small small amount of features to be in the game. But I think the team was able to get a fair amount of features in for the sprint.

## SPRINT 2 REVIEW:

Sam - Overall Sprint 2 was a bit of an eye opener for the team. I think that we started slow and rocky, without much of a plan in mind which really hurt the performance at first. But slowly the team gathered and understood what needed to be done. Which vastly improved production of Sprint 2. Tasked were split relatively evenly with teammates offering to take on work with little pushing.

There were many commits to this sprint (84 to be exact), not all were reviewed by teammates as they were very minor 1-10 line changes but the major changes were asked upon to be reviewed to ensure the code is clean and proper. This is reflected in the code review not for each pull request but for the overall file and class that resides.

Code reviews live in the `Code-Reviews/` folder.
