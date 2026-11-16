namespace Mario.Game.Interfaces;

public interface IGameControls
{
    void ResetGame();
    void NextRoom();
    void PreviousRoom();
    void ToggleHitboxes();
}