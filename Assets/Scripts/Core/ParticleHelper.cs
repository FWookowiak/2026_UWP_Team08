using UnityEngine;

public static class ParticleHelper
{
    public static void SpawnExplosion(Vector3 position, Color color, float speed = 10f, int count = 30)
    {
        GameObject go = new GameObject("ExplosionVFX");
        go.transform.position = position;
        ParticleSystem ps = go.AddComponent<ParticleSystem>();
        
        var main = ps.main;
        main.duration = 0.5f;
        main.startLifetime = 0.5f;
        main.startSpeed = speed;
        main.startSize = 0.5f;
        main.startColor = color;
        main.maxParticles = 100;
        main.playOnAwake = false;

        var emission = ps.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[]{ new ParticleSystem.Burst(0f, (short)count) });

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;

        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material = new Material(Shader.Find("Sprites/Default"));

        ps.Play();
        Object.Destroy(go, 1f);
    }
}
