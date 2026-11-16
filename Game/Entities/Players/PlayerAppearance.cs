using System.Collections.Generic;
using Mario.Game.Animation;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mario.Game.Entities.Players;

public sealed class PlayerAppearance
{
    private const float ThrowAnimationTime = 0.3f;
    private const float InvincibilityFlashFrequency = 10f;
    private const int InvincibilityFlashPhases = 2;
    private const float BigAppearanceOffsetY = -40f;
    private sealed record Appearance(ISprite Sprite, Vector2 Offset);

    private readonly Dictionary<(MarioPower Power, bool Swimming), Appearance> appearances;
    private Appearance current;
    private bool swimming;
    private float timeSinceThrow = float.PositiveInfinity;

    public PlayerAppearance(ISpriteFactory sprites)
    {
        appearances = new()
        {
            [(MarioPower.Small, false)] = new(sprites.Create("mario", "small"), Vector2.Zero),
            [(MarioPower.Big, false)] = new(sprites.Create("mario", "big"),
                new Vector2(0f, BigAppearanceOffsetY)),
            [(MarioPower.Fire, false)] = new(sprites.Create("mario", "fire"),
                new Vector2(0f, BigAppearanceOffsetY)),
            [(MarioPower.Small, true)] = new(sprites.Create("mario", "small.swimming"), Vector2.Zero),
            [(MarioPower.Big, true)] = new(sprites.Create("mario", "big.swimming"),
                new Vector2(0f, BigAppearanceOffsetY)),
            [(MarioPower.Fire, true)] = new(sprites.Create("mario", "fire.swimming"),
                new Vector2(0f, BigAppearanceOffsetY)),
        };
        current = appearances[(MarioPower.Small, false)];
    }

    public void SetForm(MarioPower power, bool swimming)
    {
        current = appearances[(power, swimming)];
        this.swimming = swimming;
        timeSinceThrow = float.PositiveInfinity;
    }

    public void ShowDeath() => current = appearances[(MarioPower.Small, false)];

    public void PlayThrow() => timeSinceThrow = 0f;

    public void Update(GameTime gameTime) => timeSinceThrow += (float)gameTime.ElapsedGameTime.TotalSeconds;

    public void UpdateAnimation(GameTime gameTime, PlayerBodyBase body, bool canCrouch, bool isDying)
    {
        string animation = isDying ? "Dead" : SelectAnimation(body, canCrouch);
        current.Sprite.Play(animation);
        current.Sprite.UpdateAnimation(gameTime);
    }

    private string SelectAnimation(PlayerBodyBase body, bool canCrouch)
    {
        if (body is SwimmingPlayerBody swimmingBody)
        {
            if (timeSinceThrow < ThrowAnimationTime)
                return "Throw";
            if (swimmingBody.IsSwimming)
                return "Swim";
            if (!body.IsOnGround)
                return "Float";
            return body.Velocity.X != 0f ? "Walk" : "Stand";
        }

        if (!body.IsOnGround)
            return "Jump";
        if (timeSinceThrow < ThrowAnimationTime)
            return "Throw";
        if (body.IsCrouching && canCrouch)
            return "Crouch";
        if (body.IsSkidding)
            return "Skid";
        return body.Velocity.X != 0f ? "Run" : "Idle";
    }

    public void Draw(SpriteBatch spriteBatch, PlayerBodyBase body, Direction facingDirection, float invincibleTimer)
{
    // Flash while invincible: skip drawing every other tenth of a second.
    if (invincibleTimer > 0f &&
        (int)(invincibleTimer * InvincibilityFlashFrequency) % InvincibilityFlashPhases == 0)
        return;

    bool flip = swimming ? facingDirection == Direction.Left : facingDirection == Direction.Right;
    SpriteEffects effects = flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

    if (current.Sprite is AnimationController animation)
    {
        Rectangle destination = new Rectangle(
            (int)body.Bounds.X,
            (int)body.Bounds.Y,
            (int)body.Bounds.Width,
            (int)body.Bounds.Height);

        animation.Draw(spriteBatch, destination, effects: effects);
        return;
    }

    Vector2 drawPosition = body.Position + current.Offset;
    current.Sprite.Draw(spriteBatch, drawPosition, effects: effects);
}
}
