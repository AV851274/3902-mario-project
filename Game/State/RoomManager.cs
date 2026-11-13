using System;
using Mario.Game.Interfaces;
using Mario.Game.Levels;
using Mario.Game.Rendering.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Mario.Game.State;

internal sealed class RoomManager
{
    private const int OverworldRoom = 0;
    private const int UnderwaterRoom = 1;
    private const float PipeExitOffset = 144f;

    private static readonly string[] RoomFiles =
    {
        "1-1.csv",
        "underwater.csv",
        "testRoom.csv"
    };

    private readonly ContentManager content;
    private Vector2 returnPosition;

    public int CurrentRoom { get; private set; }
    public bool IsUnderwater => CurrentRoom == UnderwaterRoom;

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
        IEntitySpawner spawner)
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
            spawner
        );
    }

    public (int roomIndex, Vector2? spawn) GetWarpDestination(
        Vector2 playerPosition)
    {
        if (IsUnderwater)
            return (OverworldRoom, returnPosition);

        returnPosition = playerPosition
            + new Vector2(PipeExitOffset, 0f);

        return (UnderwaterRoom, null);
    }
}