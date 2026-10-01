using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Game2D.Animation;
using Game2D.Interfaces;

public class SpriteFactory
{
    private Texture2D marioTexture;
    private Texture2D marioGifTexture;
    private Texture2D itemTexture;
    private Texture2D blockTexture;
    private Texture2D enemyTexture;
    private Texture2D tileSet;
    // private Texture2D fireballTexture;

    public void LoadTextures(ContentManager content)
    {
        marioTexture = content.Load<Texture2D>("mario");
        marioGifTexture = content.Load<Texture2D>("Mariogif");
        itemTexture = content.Load<Texture2D>("itemsAndBlocks");
        blockTexture = content.Load<Texture2D>("itemsAndBlocks");
        enemyTexture = content.Load<Texture2D>("enemies");
        tileSet = content.Load<Texture2D>("tileset");
        // fireballTexture = content.Load<Texture2D>("orange_fireball");
    }

    public List<ISprite> CreateItemSprites()
    {
        return new List<ISprite> {
            new StaticSprite(itemTexture, new Rectangle(0, 8, 16, 16)), // mushroom
            new StaticSprite(itemTexture, new Rectangle(32, 8, 16, 16)), // fire flower
            CreateCoinSprite(),
            CreateStarSprite(),
        };
    }

    private ISprite CreateCoinSprite()
    {
        AnimationController coin = new AnimationController(itemTexture, Vector2.Zero, 3f);

        coin.AddClip(new Track(
            "Spin",
            [
                new Rectangle(180, 36, 8, 15),
                new Rectangle(190, 36, 8, 15),
                new Rectangle(200, 36, 8, 15),
                new Rectangle(210, 36, 8, 15)
            ],
            [0.15f, 0.15f, 0.15f, 0.15f],
            true));
        coin.Play("Spin");

        return coin;
    }

    private ISprite CreateStarSprite()
    {
        AnimationController star = new AnimationController(itemTexture, Vector2.Zero, 3f);

        star.AddClip(new Track(
            "Spin",
            [
                new Rectangle(142, 8, 16, 16),
                new Rectangle(160, 8, 16, 16),
                new Rectangle(142, 8, 16, 16),
                new Rectangle(160, 8, 16, 16),
            ],
            [0.15f, 0.15f, 0.15f, 0.15f],
            true));
        star.Play("Spin");

        return star;
    }

    public List<ISprite> CreateBlockSprites()
    {
        return new List<ISprite> {
            new StaticSprite(tileSet, new Rectangle(17, 16, 16, 16)), // brick
            CreateQuestionBlockSprites(), // Question block animated
            new StaticSprite(blockTexture, new Rectangle(180, 116, 16, 16)), // blue brick
            new StaticSprite(blockTexture, new Rectangle(180, 332, 16, 16)), // grey block
            new StaticSprite(tileSet, new Rectangle(0, 16, 16, 16)), // Ground block
            new StaticSprite(tileSet, new Rectangle(349, 78, 16, 16)), // :Hit: question block
        };
    }

    public ISprite CreateQuestionBlockSprites()
    {
        AnimationController qBlock = new AnimationController(tileSet, Vector2.Zero, 3f);

        qBlock.AddClip(new Track(
            "Spin",
            [
                new Rectangle(298, 78, 16, 16),
                new Rectangle(315, 78, 16, 16),
                new Rectangle(332, 78, 16, 16),
            ],
            [0.15f, 0.15f, 0.15f],
            true));
        qBlock.Play("Spin");

        return qBlock;
    }

    //PLAYER SPRITES
    public ISprite CreateSmallMarioSprite()

    {
        return CreateMarioSprite(
            new Rectangle(180, 0, 16, 22),  // idle
            [
                new Rectangle(149, 0, 15, 16),
                new Rectangle(120, 0, 13, 16),
                new Rectangle(88, 0, 17, 16)
            ],
            new Rectangle(26, 0, 20, 16),  // jump
            new Rectangle(60, 0, 14, 16));   // small skid
    }

    public ISprite CreateBigMarioSprite()
    {
        return CreateMarioSprite(
            new Rectangle(180, 50, 16, 34), //big idle
            [
                new Rectangle(150, 50, 16, 32),  //big run frames
                new Rectangle(120, 50, 16, 32),
                new Rectangle(90, 50, 16, 32)
            ],
            new Rectangle(28, 50, 20, 32),  //big jump
            new Rectangle(325, 50, 20, 32),   //big skid
            new Rectangle(0, 47, 16, 34));    // CHECK: big crouch
    }

    public ISprite CreateFireMarioSprite()
    {
        // Same shapes as Big Mario, just the white/red rows of the sheet
        return CreateMarioSprite(
            new Rectangle(180, 120, 16, 32),  //fire idle
            [
                new Rectangle(150, 120, 20, 32),  //fire run frames
                new Rectangle(125, 120, 20, 32),
                new Rectangle(100, 120, 20, 32)
            ],
            new Rectangle(25, 120, 20, 32),  //fire jump
            new Rectangle(335, 120, 20, 32),   //fire skid
            new Rectangle(0, 117, 16, 34));    // CHECK: fire crouch
    }

    private ISprite CreateMarioSprite(Rectangle idle, Rectangle[] run, Rectangle jump, Rectangle skid, Rectangle? crouch = null)
    {
        AnimationController mario = new AnimationController(marioTexture, Vector2.Zero, 2.5f);

        mario.AddClip(new Track("Idle", [idle], [1f], true));
        mario.AddClip(new Track("Run", run, [0.15f, 0.15f, 0.15f], true));
        mario.AddClip(new Track("Jump", [jump], [1f], false));
        mario.AddClip(new Track("Skid", [skid], [1f], true));

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
            new Rectangle(6, 7, 12, 16),                     // stand
            [
                new Rectangle(21, 8, 13, 15),                // walk
                new Rectangle(38, 7, 15, 16),
                new Rectangle(57, 7, 11, 16)
            ],
            [
                new Rectangle(124, 6, 13, 15),               // float
                new Rectangle(140, 6, 13, 15)
            ],
            [
                new Rectangle(140, 6, 13, 15),               // swim stroke
                new Rectangle(156, 6, 13, 15),
                new Rectangle(173, 6, 13, 15),
                new Rectangle(189, 6, 13, 15)
            ]);
    }

    public ISprite CreateBigSwimmingMarioSprite()
    {
        return CreateSwimmingMarioSprite(
            new Rectangle(6, 61, 16, 32),                     // stand
            [
                new Rectangle(29, 62, 16, 32),                 // walk
                new Rectangle(52, 63, 16, 30),
                new Rectangle(74, 62, 14, 31)
            ],
            [
                new Rectangle(162, 61, 14, 30),                 // float
                new Rectangle(179, 62, 16, 29)
            ],
            [
                new Rectangle(179, 62, 16, 29),                 // swim stroke
                new Rectangle(201, 62, 14, 30),
                new Rectangle(220, 61, 16, 29),
                new Rectangle(241, 60, 16, 30)
            ]);
    }

    public ISprite CreateFireSwimmingMarioSprite()
    {
        return CreateSwimmingMarioSprite(
            new Rectangle(6, 101, 16, 32),                     // stand
            [
                new Rectangle(48, 101, 16, 32),                 // walk
                new Rectangle(70, 100, 16, 30),
                new Rectangle(92, 98, 14, 31)
            ],
            [
                new Rectangle(177, 96, 14, 30),                 // float
                new Rectangle(194, 97, 16, 29)
            ],
            [
                new Rectangle(194, 97, 16, 29),                 // swim stroke
                new Rectangle(216, 97, 14, 30),
                new Rectangle(235, 96, 16, 29),
                new Rectangle(256, 95, 16, 30)
            ]);
    }

    private ISprite CreateSwimmingMarioSprite(Rectangle stand, Rectangle[] walk, Rectangle[] floatFrames, Rectangle[] swim)
    {
        AnimationController mario = new AnimationController(marioGifTexture, Vector2.Zero, 2.5f);

        mario.AddClip(new Track("Stand", [stand], [1f], false));
        mario.AddClip(new Track("Walk", walk, [0.3f, 0.3f, 0.3f], true));
        mario.AddClip(new Track("Float", floatFrames, [0.05f, 0.05f], true));
        mario.AddClip(new Track("Swim", swim, [0.05f, 0.05f, 0.05f, 0.05f], true));
        mario.Play("Stand");

        return mario;
    }

    //ENEMY SPRITES

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
        // goomba.AddClip(new Track(
        //     "Stomped",
        //     [new Rectangle(32, 0, 16, 8)],
        //     [1f],
        //     false));
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
        // turtle.AddClip(new Track(
        //     "Stomped",
        //     [new Rectangle(32, 0, 16, 8)],
        //     [1f],
        //     false));
        turtle.Play("Walk");

        return turtle;
    }

    public ISprite CreateFireBallSprite()
    {
        AnimationController fireball = new AnimationController(marioGifTexture, Vector2.Zero, 4f);
        fireball.AddClip(
            new Track(
                "Idle",
                [
                    new Rectangle(3, 135, 16, 16),
                    new Rectangle(17, 135, 16, 16),
                    new Rectangle(33, 135, 16, 16),
                    new Rectangle(46, 135, 16, 16),
                ],
                Enumerable.Repeat(0.12f, 4).ToArray(),
                true)
        );
        return fireball;
    }
}