using Mario.Game.Animation;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Mario.Game.Rendering.Sprites;

public class MarioSpriteFactory
{
    private readonly Texture2D marioTexture;
    private readonly Texture2D marioGifTexture;

    public MarioSpriteFactory(ContentManager content)
    {
        marioTexture = content.Load<Texture2D>("mario");
        marioGifTexture = content.Load<Texture2D>("Mariogif");
    }

    public ISprite CreateSmallMarioSprite()
    {
        return CreateMarioSprite(
            new Rectangle(181, 0, 13, 16), // idle
            [
                new Rectangle(150, 0, 16, 15), // walk
                new Rectangle(121, 0, 16, 16),
                new Rectangle(89, 0, 16, 16)
            ],
            new Rectangle(29, 0, 17, 16), // jump
            new Rectangle(60, 0, 14, 16), // small skid
            death: new Rectangle(390, 16, 15, 14)); //small death pose
    }

    public ISprite CreateBigMarioSprite()
    {
        return CreateMarioSprite(
            new Rectangle(180, 52, 16, 32), //big idle
            [
                new Rectangle(150, 52, 16, 32), //big run frames
                new Rectangle(121, 52, 16, 31),
                new Rectangle(90, 53, 16, 30)
            ],
            new Rectangle(30, 52, 16, 32), //big jump
            new Rectangle(329, 52, 16, 32), //big skid
            new Rectangle(0, 57, 16, 22)); //  big crouch
    }

    public ISprite CreateFireMarioSprite()
    {
        // Same shapes as Big Mario, just the white/red rows of the sheet
        return CreateMarioSprite(
            idle: new Rectangle(180, 122, 16, 32), //fire idle
            run: [
                new Rectangle(152, 122, 16, 32), //fire run frames
                new Rectangle(128, 122, 16, 31),
                new Rectangle(102, 123, 16, 30)
            ],
            jump: new Rectangle(27, 122, 16, 32), //fire jump
            skid: new Rectangle(337, 122, 16, 32), //fire skid
            crouch: new Rectangle(0, 127, 16, 22), // fire crouch
            throwP: new Rectangle(77, 123, 16, 30) // Throwing pose
        );
    }

    private AnimationController CreateMarioSprite(Rectangle idle,
        Rectangle[] run, Rectangle jump, Rectangle skid, Rectangle? crouch = null, Rectangle? throwP = null,
        Rectangle? death = null)
    {
        AnimationController mario = new AnimationController(marioTexture, Vector2.Zero, 2.5f);

        if (death.HasValue)
            mario.AddClip(new Track("Dead", [death.Value], [1f], false));

        mario.AddClip(new Track("Idle", [idle], [1f], true));
        mario.AddClip(new Track("Run", run, [0.15f, 0.15f, 0.15f], true));
        mario.AddClip(new Track("Jump", [jump], [1f], false));
        mario.AddClip(new Track("Skid", [skid], [1f], true));
        if (throwP.HasValue)
            mario.AddClip(new Track("Throw", [throwP.Value], [1f], false));

        // Only Big and Fire Mario can crouch
        if (crouch.HasValue)
        {
            mario.AddClip(new Track("Crouch", [crouch.Value], [1f], true));
        }

        mario.Play("Idle");

        return mario;
    }

    public ISprite CreateSmallSwimmingMarioSprite()
    {
        return CreateSwimmingMarioSprite(
            new Rectangle(6, 7, 12, 16), // stand
            [
                new Rectangle(21, 8, 14, 15), // walk
                new Rectangle(38, 7, 14, 16),
                new Rectangle(57, 7, 14, 16)
            ],
            [
                new Rectangle(124, 6, 14, 16), // float
                new Rectangle(140, 6, 14, 16)
            ],
            [
                new Rectangle(140, 6, 14, 16), // swim stroke
                new Rectangle(156, 6, 14, 16),
                new Rectangle(173, 7, 14, 16),
                new Rectangle(189, 7, 14, 16)
            ]);
    }

    public ISprite CreateBigSwimmingMarioSprite()
    {
        return CreateSwimmingMarioSprite(
            new Rectangle(6, 61, 16, 32), // stand
            [
                new Rectangle(29, 62, 16, 32), // walk
                new Rectangle(52, 63, 16, 30),
                new Rectangle(74, 62, 14, 31)
            ],
            [
                new Rectangle(160, 60, 16, 32), // float
                new Rectangle(179, 61, 16, 32)
            ],
            [
                new Rectangle(179, 61, 16, 32), // swim stroke
                new Rectangle(199, 61, 16, 32),
                new Rectangle(220, 60, 16, 32),
                new Rectangle(241, 59, 16, 32)
            ]);
    }

    public ISprite CreateFireSwimmingMarioSprite()
    {
        return CreateSwimmingMarioSprite(
            new Rectangle(6, 101, 16, 32), // stand
            walk: [
                new Rectangle(48, 101, 16, 32), // walk
                new Rectangle(70, 100, 16, 30),
                new Rectangle(92, 98, 14, 31)
            ],
            floatFrames: [
                new Rectangle(175, 95, 16, 32), // float
                new Rectangle(194, 96, 16, 32)
            ],
            swim: [
                new Rectangle(194, 96, 16, 32), // swim stroke
                new Rectangle(214, 96, 16, 32),
                new Rectangle(235, 95, 16, 32),
                new Rectangle(256, 94, 16, 32)
            ],
            throwP: new Rectangle(173, 95, 20, 32) // throw
        );
    }

    private AnimationController CreateSwimmingMarioSprite(Rectangle stand, Rectangle[] walk, Rectangle[] floatFrames,
        Rectangle[] swim, Rectangle? throwP = null)
    {
        AnimationController mario = new AnimationController(marioGifTexture, Vector2.Zero, 2.5f);

        mario.AddClip(new Track("Stand", [stand], [1f], false));
        mario.AddClip(new Track("Walk", walk, [0.3f, 0.3f, 0.3f], true));
        mario.AddClip(new Track("Float", floatFrames, [0.05f, 0.05f], true));
        mario.AddClip(new Track("Swim", swim, [0.05f, 0.05f, 0.05f, 0.05f], true));

        if (throwP.HasValue)
            mario.AddClip(new Track("Throw", [throwP.Value], [1f], false));

        mario.Play("Stand");

        return mario;
    }

    public ISprite CreateFireballSprite()
    {
        AnimationController fireball = new AnimationController(marioGifTexture, Vector2.Zero, 3f);
        fireball.AddClip(
            new Track(
                "Idle",
                [
                    new Rectangle(7, 140, 8, 8),
                    new Rectangle(21, 140, 8, 8),
                    new Rectangle(35, 140, 8, 8),
                    new Rectangle(49, 139, 8, 8),
                ],
                Enumerable.Repeat(0.12f, 4).ToArray(),
                true)
        );

        fireball.AddClip(
            new Track(
                "Explode",
                [
                    new Rectangle(61, 135, 16, 16)],
                [0.5f],
                false)
        );
        fireball.Play("Idle");
        return fireball;
    }
}
