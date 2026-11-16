using System;
using Mario.Game.Entities.Blocks;
using Mario.Game.Entities.Items;
using Mario.Game.Interfaces;
using Mario.Game.Levels;
using Mario.Game.Rendering.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Mario.Game.State;

internal sealed class RoomManager
{
    private const int PipeExitTileOffset = 3;

    private static readonly string[] RoomFiles =
    {
        "1-1.csv",
        "underwater.csv",
        "testRoom.csv"
    };

    private readonly ContentManager content;
    private Vector2 returnPosition;

    public int CurrentRoom { get; private set; }
    public bool IsUnderwater => CurrentRoom == GameConstants.World.UnderwaterRoomIndex;

    public int NextRoomIndex =>
        (CurrentRoom + 1) % RoomFiles.Length;

    public int PreviousRoomIndex =>
        (CurrentRoom - 1 + RoomFiles.Length) % RoomFiles.Length;

    public RoomManager(ContentManager content)
    {
        this.content = content;
    }

    public (Vector2 spawn, int width, int height) LoadRoom(
        int roomIndex,
        GameEntities entities,
        SpriteFactory spriteFactory,
        IEntitySpawner spawner,
        ItemFactory itemFactory,
        BlockFactory blockFactory)
    {
        if (roomIndex < 0 || roomIndex >= RoomFiles.Length)
            throw new ArgumentOutOfRangeException(nameof(roomIndex));

        CurrentRoom = roomIndex;
        entities.Reset();

        return LevelLoader.LoadLevel(
            content,
            RoomFiles[CurrentRoom],
            entities,
            spriteFactory,
            spawner,
            itemFactory,
            blockFactory
        );
    }

    public (int roomIndex, Vector2? spawn) GetWarpDestination(
        Vector2 playerPosition)
    {
        if (IsUnderwater)
            return (GameConstants.World.OverworldRoomIndex, returnPosition);

        returnPosition = playerPosition
            + new Vector2(GameConstants.World.TileSize * PipeExitTileOffset, 0f);

        return (GameConstants.World.UnderwaterRoomIndex, null);
    }
}
