using UnityEngine;
using System.Collections.Generic;

public class WavePreviewPresenter : MonoBehaviour
{
    [SerializeField] private WavePreviewView wavePreviewView;

    private void Update()
    {
        if (wavePreviewView == null || WaveManager.Instance == null) return;

        int nextIndex = WaveManager.Instance.CurrentRoundIndex;
        WaveData[] allRounds = WaveManager.Instance.allRounds;

        if (nextIndex < allRounds.Length)
        {
            WaveData nextWave = allRounds[nextIndex];
            List<WavePreviewData> previewList = new List<WavePreviewData>();

            foreach (WaveGroup group in nextWave.waveGroup)
            {
                int tier = 1;
                string name = group.enemyPrefab.name;
                if (name.Contains("2")) tier = 2;
                else if (name.Contains("3")) tier = 3;

                previewList.Add(new WavePreviewData
                {
                    enemyName = name,
                    count = group.count,
                    tier = tier
                });
            }

            wavePreviewView.ShowNextWave(previewList.ToArray());
        }
        else
        {
            wavePreviewView.HidePreview();
        }
    }
}
