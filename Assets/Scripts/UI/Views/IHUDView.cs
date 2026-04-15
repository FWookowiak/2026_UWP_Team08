public interface IHUDView
{
    void UpdateMoney(int amount);
    void UpdateLives(int amount);
    void UpdateWaveCounter(int current, int total);
    void UpdateGameState(string state);
}
