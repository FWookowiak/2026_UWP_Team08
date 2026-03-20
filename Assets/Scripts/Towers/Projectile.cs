using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;

    public float speed = 30f;
    public float damage = 10f;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    private void HitTarget()
    {
        EnemyBase e = target.GetComponent<EnemyBase>();
        if (e != null)
        {
            e.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
