using Mario.Game.Interfaces;
using Mario.Game.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Blocks;

public abstract class BlockBase : IBlock
{
    protected ISprite Sprite { get; set; }
    protected virtual Vector2 DrawPosition => Position;

    public Vector2 Position { get; }
    public bool Active => true;
    public BlockType Type { get; }
    public virtual Box Bounds => new(Position.X, Position.Y,
        GameConstants.World.TileSize, GameConstants.World.TileSize);
    public virtual ICollider.BlockingModes BlockingMode => ICollider.BlockingModes.Solid;

    protected BlockBase(ISprite sprite, BlockType type, Vector2 position)
    {
        Sprite = sprite;
        Type = type;
        Position = position;
    }

    public virtual void Hit(bool big) { }
    public virtual void OnCollision(CollisionEvent collision) { }

    public virtual void Update(GameTime gameTime) =>
        Sprite.UpdateAnimation(gameTime);

    public virtual void Draw(SpriteBatch spriteBatch) =>
        Sprite.Draw(spriteBatch, DrawPosition);
}
