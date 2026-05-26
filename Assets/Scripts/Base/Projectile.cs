using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Налаштування польоту")]
    public float speed = 7f;
    public float aoeRadius = 1f;

    private Transform target;
    private TowerData data;

    public void Seek(Transform _target, TowerData _data)
    {
        target = _target;
        data = _data;
    }

    void Update()
    {
        if (target == null)
        {
            gameObject.SetActive(false);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
    }

    void HitTarget()
    {

        if (data.attackType == AttackType.Single)
        {
            Damage(target);
        }
        else if (data.attackType == AttackType.AoE)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, aoeRadius);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    Damage(collider.transform);
                }
            }
        }
        else if (data.attackType == AttackType.Slow)
        {
            Damage(target);
            
            EnemyMovement movement = target.GetComponent<EnemyMovement>();
            if (movement != null)
            {
                movement.ApplySlow(0.5f, 2f);
            }
        }

        gameObject.SetActive(false);
    }

    void Damage(Transform enemyTransform)
    {
        EnemyHealth e = enemyTransform.GetComponent<EnemyHealth>();
        if (e != null)
        {
            e.TakeDamage(data.damage);
        }
    }
}