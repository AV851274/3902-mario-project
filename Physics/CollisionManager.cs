using Mario.Interfaces.Player;
using Mario.Interfaces;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Mario.Physics;

public static class CollisionManager
{
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