using Mario.Game.Animation;
using Mario.Game.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace Mario.Game.Rendering.Sprites;

public class EnemySpriteFactory
{
    private const float Scale = 2.5f;

    private readonly Texture2D enemyTexture;

    public EnemySpriteFactory(ContentManager content)
    {
        enemyTexture = content.Load<Texture2D>("enemies");
    }

    public ISprite CreateGoombaSprite()
    {
        AnimationController goomba = new AnimationController(enemyTexture, Vector2.Zero, Scale);

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
        AnimationController turtle = new AnimationController(enemyTexture, Vector2.Zero, Scale);

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

    public ISprite CreateBuzzyBeetleSprite()
    {
        AnimationController buzzyBeetle = new AnimationController(enemyTexture, Vector2.Zero, Scale);

        buzzyBeetle.AddClip(new Track(
            "Walk",
            [
                new Rectangle(0, 34, 16, 16),
                new Rectangle(18, 34, 16, 16)
            ],
            [0.2f, 0.2f],
            true));

        buzzyBeetle.AddClip(new Track(
            "Stomped",
            [new Rectangle(36, 34, 16, 16)],
            [1f],
            false));

        buzzyBeetle.Play("Walk");

        return buzzyBeetle;
    }

    public ISprite CreateSpinySprite()
    {
        AnimationController spiny = new AnimationController(enemyTexture, Vector2.Zero, Scale);

        spiny.AddClip(new Track(
            "Walk",
            [
                new Rectangle(72, 352, 16, 16),
                new Rectangle(90, 352, 16, 16)
            ],
            [0.2f, 0.2f],
            true));

        spiny.Play("Walk");

        return spiny;
    }

    public ISprite CreateParaKoopaSprite()
    {
        AnimationController paraKoopa = new AnimationController(enemyTexture, Vector2.Zero, Scale);

        paraKoopa.AddClip(new Track(
            "Walk",
            [
                new Rectangle(36, 112, 16, 24),
                new Rectangle(54, 112, 16, 24)
            ],
            [0.2f, 0.2f],
            true));

        paraKoopa.Play("Walk");

        return paraKoopa;
    }

     public ISprite CreateLakituSprite()
    {
        AnimationController lakitu = new AnimationController(enemyTexture, Vector2.Zero, Scale);

        lakitu.AddClip(new Track(
            "Idle",
            [
                new Rectangle(54, 138, 16, 24),
            ],
            [0.2f],
            true));

            lakitu.AddClip(new Track(
            "EggThrow",
            [
                new Rectangle(72, 138, 16, 24),
            ],
            [0.2f],
            true));

        lakitu.Play("Idle");

        return lakitu;
    }

    public ISprite CreateGreenCheepSprite()
    {
        AnimationController greenCheep = new AnimationController(enemyTexture, Vector2.Zero, Scale);

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
        AnimationController grayCheep = new AnimationController(enemyTexture, Vector2.Zero, Scale);

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
        AnimationController hammerBro = new AnimationController(enemyTexture, Vector2.Zero, Scale);

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

    public ISprite CreateBowserSprite()
    {
        AnimationController bowser = new AnimationController(enemyTexture, Vector2.Zero, Scale);

        bowser.AddClip(new Track(
            "Idle",
            [
                new Rectangle(0, 208, 32, 32),
                new Rectangle(34, 208, 32, 32),
            ],
            [0.4f, 0.4f],
            true));

        bowser.AddClip(new Track(
            "FireBreath",
            [
                new Rectangle(68, 208, 32, 32),
                new Rectangle(102, 208, 32, 32),
            ],
            [0.4f, 0.4f],
            false));

        bowser.Play("Idle");

        return bowser;
    }

    public ISprite CreateHammerSprite()
    {
        AnimationController hammer = new AnimationController(enemyTexture, Vector2.Zero, Scale);
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

    // public ISprite CreateFirebar()
    // {
    //     AnimationController fire = new AnimationController(enemyTexture, Vector2.Zero, Scale);
    //     fire.AddClip(
    //         new Track(
    //             "Idle",
    //             [
    //                 new Rectangle(98, 154, 8, 8),
    //                 new Rectangle(108, 154, 8, 8),
    //                 new Rectangle(118, 154, 8, 8),
    //                 new Rectangle(128, 154, 8, 8),
    //             ],
    //             Enumerable.Repeat(0.2f, 4).ToArray(),
    //             true)
    //     );

    //     fire.Play("Idle");
    //     return fire;
    // }

    public ISprite CreateEggSprite()
    {
        AnimationController egg = new AnimationController(enemyTexture, Vector2.Zero, Scale);
        egg.AddClip(
            new Track(
                "Idle",
                [
                    new Rectangle(36, 352, 16, 16),
                    new Rectangle(54, 352, 16, 16),
                ],
                [0.2f, 0.2f],
                true)
        );
        return egg;
    }

    public ISprite CreateFireSprite()
    {
        AnimationController fire = new AnimationController(enemyTexture, Vector2.Zero, Scale);
        fire.AddClip(
            new Track(
                "Idle",
                [
                    new Rectangle(102, 242, 24, 8),
                    new Rectangle(102, 252, 24, 8),
                ],
                [0.2f, 0.2f],
                true)
        );
        return fire;
    }
}
