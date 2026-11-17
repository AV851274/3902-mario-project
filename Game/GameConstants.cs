namespace Mario.Game;

internal static class GameConstants
{
    internal static class World
    {
        public const int TileSize = 48;
        public const int HalfTileSize = TileSize / 2;
        public const int QuarterTileSize = TileSize / 4;
        public const float EntityDespawnY = 1000f;
        public const int OverworldRoomIndex = 0;
        public const int UnderwaterRoomIndex = 1;
    }

    internal static class Entities
    {
        public const int StandardSize = 40;
        public const int TallHeight = 60;
    }

    internal static class Enemies
    {
        public const float StandardWalkSpeed = 60f;
        public const float SlowWalkSpeed = 30f;
        public const float StandardJumpVelocity = -500f;
        public const float KickSpeedMultiplier = 6f;
        public const float KickGraceDuration = 0.25f;
        public const float HammerSpawnOffsetX = 20f;
        public const float HammerSpawnOffsetY = -20f;
        public const float ProjectileHorizontalVelocity = 250f;
        public const float HammerVelocityY = -350f;
        public const float DirectionChangeChancePerFrame = 0.05f;
    }

    internal static class Player
    {
        public const float SmallBodyWidth = 36f;
    }

    internal static class Projectiles
    {
        public const float DefaultBounceSpeed = 400f;
    }

    internal static class Blocks
    {
        public const float BumpHeight = 12f;
        public const float BumpDuration = 0.15f;
    }

    internal static class Screen
    {
        public const int TitleOffset = 80;
        public const int PromptOffset = 40;
    }
}
