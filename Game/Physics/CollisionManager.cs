using Mario.Interfaces.Player;
using Mario.Interfaces;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Mario.Physics;

public static class CollisionManager
{
    private const float StompBounceSpeed = 350f;
    private const int StompTolerance = 10;

    public static void ResolveEnemyBlockCollisions(IEnemy enemy, List<IBlock> blocks, Rectangle previousBounds)
    {
        // Landing first, so an enemy resting on the ground isn't mistaken for hitting a wall
        ResolveEnemyLanding(enemy, blocks, previousBounds);
        ResolveEnemyWalls(enemy, blocks, previousBounds);
    }

    private static void ResolveEnemyLanding(IEnemy enemy, List<IBlock> blocks, Rectangle previousBounds)
    {
        foreach (IBlock block in blocks)
        {
            Rectangle enemyBounds = enemy.Bounds;
            Rectangle blockBounds = block.Bounds;

            bool horizontalOverlap =
                enemyBounds.Right > blockBounds.Left &&
                enemyBounds.Left < blockBounds.Right;

            bool landing =
                horizontalOverlap &&
                previousBounds.Bottom <= blockBounds.Top &&
                enemyBounds.Bottom >= blockBounds.Top;

            if (landing)
            {
                enemy.LandOn(blockBounds.Top);
                return;
            }
        }
    }

    private static void ResolveEnemyWalls(IEnemy enemy, List<IBlock> blocks, Rectangle previousBounds)
    {
        foreach (IBlock block in blocks)
        {
            Rectangle enemyBounds = enemy.Bounds;
            Rectangle blockBounds = block.Bounds;

            bool verticalOverlap =
                enemyBounds.Bottom > blockBounds.Top &&
                enemyBounds.Top < blockBounds.Bottom;

            bool hitLeftSide =
                verticalOverlap &&
                previousBounds.Right <= blockBounds.Left &&
                enemyBounds.Right > blockBounds.Left;

            bool hitRightSide =
                verticalOverlap &&
                previousBounds.Left >= blockBounds.Right &&
                enemyBounds.Left < blockBounds.Right;

            if (hitLeftSide)
            {
                enemy.BounceOffWall(blockBounds.Left - enemyBounds.Width);
                return;
            }

            if (hitRightSide)
            {
                enemy.BounceOffWall(blockBounds.Right);
                return;
            }
        }
    }

    public static void ResolvePlayerEnemyCollisions(IPlayer player, List<IEnemy> enemies, Rectangle previousPlayerBounds)
    {
        foreach (IEnemy enemy in enemies)
        {
            if (enemy.IsDefeated || !player.Bounds.Intersects(enemy.Bounds))
            {
                continue;
            }

            // Falling, and Mario's feet were above the enemy's head last frame
            bool landedOnTop =
                player.Velocity.Y > 0f &&
                previousPlayerBounds.Bottom <= enemy.Bounds.Top + StompTolerance;

            if (landedOnTop)
            {
                enemy.Stomp();
                player.Velocity = new Vector2(player.Velocity.X, -StompBounceSpeed);
            }
            else if (enemy.CanHurtPlayer)
            {
                player.TakeDamage();
            }
        }
    }

    public static void ResolvePlayerBlockCollisions(IPlayer player, List<IBlock> blocks, Rectangle previousBounds)
    {
        foreach (IBlock block in blocks)
        {
            Rectangle playerBounds = player.Bounds;
            Rectangle blockBounds = block.Bounds;

            bool horizontalOverlap =
                playerBounds.Right > blockBounds.Left &&
                playerBounds.Left < blockBounds.Right;

            bool landing =
                horizontalOverlap &&
                previousBounds.Bottom <= blockBounds.Top &&
                playerBounds.Bottom >= blockBounds.Top &&
                player.Velocity.Y >= 0f;

            if (landing)
            {
                float newY = blockBounds.Top - playerBounds.Height;
                if (playerBounds.Height > 48)
                {
                    newY += 40f;
                }

                player.SetPosition(new Vector2(player.Position.X, newY));
                player.Velocity = new Vector2(player.Velocity.X, 0f);
                player.IsOnGround = true;

                break;
            }
        }
    }

        public static void ResolveItemWallCollisions(IItem item, List<IBlock> blocks, Rectangle previousBounds)
    {
        foreach (IBlock block in blocks)
        {
            Rectangle itemBounds = item.Bounds;
            Rectangle blockBounds = block.Bounds;

            bool verticalOverlap =
                itemBounds.Bottom > blockBounds.Top &&
                itemBounds.Top < blockBounds.Bottom;

            bool hitLeftSide =
                verticalOverlap &&
                previousBounds.Right <= blockBounds.Left &&
                itemBounds.Right > blockBounds.Left;

            bool hitRightSide =
                verticalOverlap &&
                previousBounds.Left >= blockBounds.Right &&
                itemBounds.Left < blockBounds.Right;

            if (hitLeftSide)
            {
                item.BounceOffWall(blockBounds.Left - itemBounds.Width);
                break;
            }

            if (hitRightSide)
            {
                item.BounceOffWall(blockBounds.Right);
                break;
            }
        }
    }

    public static void ResolvePlayerItemCollisions(IPlayer player, List<IItem> items)
    {
        foreach (IItem item in items)
        {
            if (!item.IsCollected && player.Bounds.Intersects(item.Bounds))
            {
                item.Collect(player);
            }
        }
    }
    public static void ResolvePlayerWallCollisions(IPlayer player, List<IBlock> blocks, Rectangle previousBounds)
    {
        foreach (IBlock block in blocks)
        {
            Rectangle playerBounds = player.Bounds;
            Rectangle blockBounds = block.Bounds;

            bool verticalOverlap =
                playerBounds.Bottom > blockBounds.Top &&
                playerBounds.Top < blockBounds.Bottom;

            bool movingRight =
                verticalOverlap &&
                previousBounds.Right <= blockBounds.Left &&
                playerBounds.Right >= blockBounds.Left &&
                player.Velocity.X > 0f;

            bool movingLeft =
                verticalOverlap &&
                previousBounds.Left >= blockBounds.Right &&
                playerBounds.Left <= blockBounds.Right &&
                player.Velocity.X < 0f;

            if (movingRight || movingLeft)
            {
                if (movingRight)
                {
                    float newX = blockBounds.Left - playerBounds.Width;

                    player.SetPosition(new Vector2(newX, player.Position.Y));
                    player.Velocity = new Vector2(0f, player.Velocity.Y);
                }
                else if (movingLeft)
                {
                    float newX = blockBounds.Right;

                    player.SetPosition(new Vector2(newX, player.Position.Y));
                    player.Velocity = new Vector2(0f, player.Velocity.Y);
                }

                break;
            }
        }
    }
    public static void ResolvePlayerCeilingCollisions(IPlayer player, List<IBlock> blocks, Rectangle previousBounds)
    {
        foreach (IBlock block in blocks)
        {
            Rectangle playerBounds = player.Bounds;
            Rectangle blockBounds = block.Bounds;

            bool horizontalOverlap =
                playerBounds.Right > blockBounds.Left &&
                playerBounds.Left < blockBounds.Right;

            bool hittingCeiling =
                horizontalOverlap &&
                previousBounds.Top >= blockBounds.Bottom &&
                playerBounds.Top <= blockBounds.Bottom &&
                player.Velocity.Y < 0f;

            if (hittingCeiling)
            {
                float newY = blockBounds.Bottom;
                block.Hit();

                if (playerBounds.Height > 48)
                {
                    newY += 40f;
                }

                player.SetPosition(new Vector2(player.Position.X, newY));
                player.Velocity = new Vector2(player.Velocity.X, 0f);

                break;
            }
        }
    }
}