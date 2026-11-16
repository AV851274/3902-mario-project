CURRENT SPRINT: #4

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
| M                        | Mute / unmute background music    |
| R                        | Reset to the first room           |
| Q                        | Quit                                  |

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
- The flagpole at the end of 1-1 catches Mario, slides him down to the ground, and shows the course-clear screen
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
- `Rendering/` contains sprites, the camera, HUD, and `WorldRenderer`, which draws the world, water background, and optional hitboxes.
- `Levels/` loads CSV maps. `State/GameWorld.cs` owns gameplay, room transitions, and world-level events, and delegates drawing to its `WorldRenderer`. `MarioGame` creates the renderer and shared stats, then injects them into the world.
- `State/GameEntities.cs` owns entity collections and a queue committed after physics each frame. New entities begin updating on the next frame; resetting a room also clears queued entities.
- `State/GameScreen/` contains the screen manager and Start, Playing, Paused, and GameOver states. Playing and Paused share one world; pausing draws it without updating it.
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

Analyzers are enabled in `Monogame.csproj` (`AnalysisMode = All`). Latest run (11/16/2026, .NET SDK 10.0.113): 0 errors, 250 warnings (238 analyzer warnings and 12 C# nullability warnings).

These warnings are not currently considered critical because the review found no confirmed gameplay failure or security vulnerability caused by them. Most concern API design, naming, or redundant code, and `Random` is used only for enemy behavior. The reviewed nullability warnings have existing guards, intentional unused values, or initialization paths, and normal shutdown already disposes the flagged resources.

| Rule | Count | Explanation and impact |
| ---- | ----- | ---------------------- |
| CA1002 | 6 | Avoid exposing `List<T>` in public APIs. `GameEntities` exposes lists, so callers must follow the collection update rules. |
| CA1003 | 3 | Use standard event-handler types. Our `Action` events work for simple in-game notifications. |
| CA1024 | 2 | Use properties for getter-like methods. FireBar keeps methods to make their calculations and allocations clearer. |
| CA1034 | 1 | Avoid public nested types. Our nested `CollisionResult` affects API organization, not collision behavior. |
| CA1051 | 6 | Avoid public or protected instance fields. Our `protected` fields simplify access but tie subclasses to base-class details. |
| CA1052 | 1 | Make static-only classes static. `LevelLoader` could be static without changing level loading. |
| CA1062 | 55 | Check reference parameters for null. Our methods rely on controlled callers; non-nullable annotations do not add runtime checks. |
| CA1305 | 3 | Specify a culture when formatting values. Our HUD uses the user's locale, so display conventions may vary. |
| CA1515 | 123 | Make application-only types internal. Our public types do not need to be used by other assemblies. |
| CA1716 | 4 | Avoid other .NET languages' keywords in API names. Our `Stop` and `Exit` names work normally in C#. |
| CA1724 | 1 | Avoid type names that clash with namespaces. `Player` overlaps with a namespace name and can make references confusing. |
| CA1805 | 9 | Avoid redundant default initializers. Our explicit `false` or `0` values repeat defaults C# already provides. |
| CA1819 | 1 | Avoid properties that return arrays. Callers can modify `Frames` elements even though the property uses `init`. |
| CA1822 | 2 | Make members static when they use no instance state. Our two sound-manager methods fit the current instance-based API. |
| CA1823 | 2 | Remove unused fields. `Spiny` has two unused constants that can be removed without changing behavior. |
| CA1859 | 7 | Use concrete types where they can improve performance. We retain interfaces for abstraction; any performance benefit should be measured. |
| CA2213 | 3 | Release disposable fields through `Dispose`. MonoGame disposes `graphics`; `UnloadContent` disposes `spriteBatch` and `world`, but paths that skip unloading need attention. |
| CA5394 | 9 | Avoid non-cryptographic random generators. Our `Random` calls only control enemy behavior, so cryptographic security is unnecessary. |
| CS8600 | 1 | A possibly null value is assigned to a non-nullable variable. Failed clip lookups return before the result is used. |
| CS8601 | 1 | A possibly null reference is assigned to a non-nullable target. Failed clip lookups return before assigning `currentClip`. |
| CS8618 | 7 | A non-nullable member may be uninitialized after construction. Existing guards, safe event calls, and `Reset()` handle the reviewed cases. |
| CS8625 | 2 | Null is assigned or passed to a non-nullable reference type. Our null clips and unused launcher sprite are intentional. |
| CS8629 | 1 | A nullable value may be read while null. `IsEmerging` checks `HasValue` before `.Value`, but the compiler cannot follow that property. |


```
          LINES OF CODE (1 █ ≈ 10 code lines)
Player.cs               ███████████████████████████ 272
GameWorld.cs            █████████████████           172
Item.cs                 ███████████████             150
PhysicsEngine.cs        ██████████████              137
GameEntities.cs         ███████████                 113
AnimationController.cs  ███████████                 112
Lakitu.cs               ███████████                 110
Fireball.cs             ███████████                 110
HammerBro.cs            ███████████                 109
BlockTileHandler.cs     ███████████                 109
EnemyTileHandler.cs     ██████████                  101
PlayerBodyBase.cs       ██████████                  100
CollisionManager.cs     ██████████                   99
Bowser.cs               ██████████                   97
CheepCheep.cs           █████████                    93
```

This chart shows the 15 largest C# files under `Game/`, measured on 11/16/2026. The 127 source files contain 5,086 code lines in total.

During Sprint 4, we split several large files by responsibility. `MarioGame.cs` now delegates world logic, rendering, and screen management to `GameWorld`, `WorldRenderer`, and `GameScreenManager`. `LevelLoader.cs` delegates CSV parsing and tile placement to a separate reader and specialized handlers, and the former `Block.cs` was divided into a shared base, a factory, and individual block types. We also replaced the large sprite factories with a shared `SpriteFactory` driven by JSON definitions. In future sprint, we will analyze `Player.cs` (272 code lines), `GameWorld.cs` (172) and try to refactor them where responsibilities can be separated clearly.

The team used AI agents to help handle bug fixing, boilerplate, simple refactor, and to help wrap our heads around the tasks at hand. AI agents also played a role in reviewing code and cleaning up the project as well. Making sure to not let it go wild as the team needed to understand
and have readability stay strong in the project.

## SPRINT 3 REVIEW:

Sam - Sprint 3 was a pretty good progressive sprint for the team. A lot was done especially with map work and collisions. The game is really starting to actually feel like a game instead of a cheap one off knock off. I think introducing a TODO list for the team helped quite a bit, this text file lives in the 'TODO/' folder.

Lots of commits to get the project good and ready for the end of sprint 3. Only missing a small small amount of features to be in the game. But I think the team was able to get a fair amount of features in for the sprint.

## SPRINT 2 REVIEW:

Sam - Overall Sprint 2 was a bit of an eye opener for the team. I think that we started slow and rocky, without much of a plan in mind which really hurt the performance at first. But slowly the team gathered and understood what needed to be done. Which vastly improved production of Sprint 2. Tasked were split relatively evenly with teammates offering to take on work with little pushing.

There were many commits to this sprint (84 to be exact), not all were reviewed by teammates as they were very minor 1-10 line changes but the major changes were asked upon to be reviewed to ensure the code is clean and proper. This is reflected in the code review not for each pull request but for the overall file and class that resides.

Code reviews live in the `Code-Reviews/` folder.
