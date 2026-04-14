public interface IWavePreviewView
{
    void ShowNextWave(WavePreviewData[] groups);
    void HidePreview();
}

[System.Serializable]
public class WavePreviewData
{
    public string enemyName;
    public int count;
    public int tier; // 1, 2, 3
}
