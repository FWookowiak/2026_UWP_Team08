using UnityEngine;

public static class ProceduralAudio
{
    public static AudioClip GenerateShootSound()
    {
        return GenerateSound(44100 / 8, (time, duration) =>
        {
            float envelope = 1f - (time / duration);
            return Mathf.Sin(2f * Mathf.PI * 440f * time * (1f - 0.5f * (time / duration))) * envelope * 0.5f;
        });
    }

    public static AudioClip GenerateHitSound()
    {
        return GenerateSound(44100 / 10, (time, duration) =>
        {
            float envelope = 1f - (time / duration);
            return (Random.value * 2f - 1f) * envelope * 0.3f; 
        });
    }

    public static AudioClip GenerateExplosionSound()
    {
        return GenerateSound(44100 / 2, (time, duration) =>
        {
            float envelope = Mathf.Pow(1f - (time / duration), 3f);
            return (Random.value * 2f - 1f) * envelope * 0.8f;
        });
    }

    public static AudioClip GenerateBuildSound()
    {
        return GenerateSound(44100 / 4, (time, duration) =>
        {
            float envelope = Mathf.Sin(Mathf.PI * (time / duration));
            return Mathf.Sin(2f * Mathf.PI * 660f * time) * envelope * 0.4f;
        });
    }

    public static AudioClip GenerateSellSound()
    {
        return GenerateSound(44100 / 4, (time, duration) =>
        {
            float envelope = Mathf.Sin(Mathf.PI * (time / duration));
            return Mathf.Sin(2f * Mathf.PI * 220f * time) * envelope * 0.4f;
        });
    }

    private static AudioClip GenerateSound(int sampleCount, System.Func<float, float, float> waveform)
    {
        AudioClip clip = AudioClip.Create("ProceduralSound", sampleCount, 1, 44100, false);
        float[] samples = new float[sampleCount];
        float duration = (float)sampleCount / 44100f;
        for (int i = 0; i < sampleCount; i++)
        {
            float time = i / 44100f;
            samples[i] = waveform(time, duration);
        }
        clip.SetData(samples, 0);
        return clip;
    }
}
