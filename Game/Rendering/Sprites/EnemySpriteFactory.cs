using Mario.Game.Animation;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Mario.Game.Rendering.Sprites;

public class EnemySpriteFactory
{
    private readonly Texture2D enemyTexture;

    public EnemySpriteFactory(ContentManager content)
    {
        enemyTexture = content.Load<Texture2D>("enemies");
    }

    public ISprite CreateGoombaSprite()
    {
        AnimationController goomba = new AnimationController(enemyTexture, Vector2.Zero, 2f);

        goomba.AddClip(new Track(
            "Walk",
            [
                new Rectangle(0, 16, 16, 16),
                new Rectangle(18, 16, 16, 16)
            ],
            [0.2f, 0.2f],
            true));

        goomba.AddClip(new Track(
            "Stomped",
            [
                new Rectangle(0, 16, 16, 16),
            ],
            [0.2f],
            false));

        goomba.Play("Walk");

        return goomba;
    }

    public ISprite CreateTurtleSprite()
    {
        AnimationController turtle = new AnimationController(enemyTexture, Vector2.Zero, 2f);

        turtle.AddClip(new Track(
            "Walk",
            [
                new Rectangle(0, 112, 16, 24),
                new Rectangle(18, 112, 16, 24)
            ],
            [0.2f, 0.2f],
            true));

        turtle.AddClip(new Track(
            "Stomped",
            [new Rectangle(72, 111, 16, 24)],
            [1f],
            false));

        turtle.Play("Walk");

        return turtle;
    }

    public ISprite CreateGreenCheepSprite()
    {
        AnimationController greenCheep = new AnimationController(enemyTexture, Vector2.Zero, 2f);

        greenCheep.AddClip(new Track(
            "Swim",
            [
                new Rectangle(0, 164, 16, 16),
                new Rectangle(18, 164, 16, 16)
            ],
            [0.2f, 0.2f],
            true));

        greenCheep.AddClip(new Track(
            "Stomped",
            [new Rectangle(0, 164, 16, 16)],
            [1f],
            false));

        greenCheep.Play("Swim");

        return greenCheep;
    }

    public ISprite CreateGrayCheepSprite()
    {
        AnimationController grayCheep = new AnimationController(enemyTexture, Vector2.Zero, 2f);

        grayCheep.AddClip(new Track(
            "Swim",
            [
                new Rectangle(292, 164, 16, 16),
                new Rectangle(310, 164, 16, 16)
            ],
            [0.2f, 0.2f],
            true));

        grayCheep.AddClip(new Track(
            "Stomped",
            [new Rectangle(292, 164, 16, 16)],
            [1f],
            false));

        grayCheep.Play("Swim");

        return grayCheep;
    }

    public ISprite CreateHammerBroSprite()
    {
        AnimationController hammerBro = new AnimationController(enemyTexture, Vector2.Zero, 2f);

        hammerBro.AddClip(new Track(
            "Idle",
            [
                new Rectangle(0, 182, 16, 24),
                new Rectangle(18, 182, 16, 24),
            ],
            [0.2f, 0.2f],
            true));

        hammerBro.AddClip(new Track(
            "Stomped",
            [
                new Rectangle(0, 182, 16, 24),
            ],
            [0.2f],
            false));

        hammerBro.AddClip(new Track(
            "HammerThrow",
            [
                new Rectangle(37, 182, 16, 24),
                new Rectangle(55, 182, 16, 24),
            ],
            [0.2f, 0.2f],
            false));

        hammerBro.Play("Idle");

        return hammerBro;
    }

    public ISprite CreateHammerSprite()
    {
        AnimationController hammer = new AnimationController(enemyTexture, Vector2.Zero, 2f);
        hammer.AddClip(
            new Track(
                "Idle",
                [
                    new Rectangle(1, 78, 16, 16),
                    new Rectangle(18, 78, 16, 16),
                    new Rectangle(36, 78, 16, 16),
                    new Rectangle(54, 78, 16, 16),
                ],
                Enumerable.Repeat(0.12f, 4).ToArray(),
                true)
        );
        return hammer;
    }
}
