using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Аудіо")]
    public AudioClip towerShootSound;
    [Range(0f, 1f)] public float shootVolume = 1f;
    [Header("Дані вежі")]
    public TowerData towerData;
    [Header("Анімація")]
    public Animator towerAnimator;
    private Transform target;
    private float fireCountdown = 0f;

    void Update()
    {
        UpdateTarget();

        if (target != null)
        {
            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / towerData.fireRate; 
            }
        }

        fireCountdown -= Time.deltaTime;
    }

    void UpdateTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, towerData.range);
        
        Transform bestTarget = null;
        int maxWaypointIndex = -1;
        float shortestDistanceToNextWaypoint = Mathf.Infinity;

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                EnemyMovement enemyMovement = collider.GetComponent<EnemyMovement>();
                
                if (enemyMovement != null)
                {
                    if (enemyMovement.currentWaypointIndex > maxWaypointIndex)
                    {
                        maxWaypointIndex = enemyMovement.currentWaypointIndex;
                        bestTarget = collider.transform;
                        
                        if (enemyMovement.currentWaypointIndex < enemyMovement.waypoints.Length)
                        {
                            shortestDistanceToNextWaypoint = Vector2.Distance(collider.transform.position, enemyMovement.waypoints[enemyMovement.currentWaypointIndex].position);
                        }
                    }
                    else if (enemyMovement.currentWaypointIndex == maxWaypointIndex)
                    {
                        if (enemyMovement.currentWaypointIndex < enemyMovement.waypoints.Length)
                        {
                            float distanceToNext = Vector2.Distance(collider.transform.position, enemyMovement.waypoints[enemyMovement.currentWaypointIndex].position);
                            
                            if (distanceToNext < shortestDistanceToNextWaypoint)
                            {
                                shortestDistanceToNextWaypoint = distanceToNext;
                                bestTarget = collider.transform;
                            }
                        }
                    }
                }
            }
        }

        target = bestTarget;
    }

    void Shoot()
    {
        if (towerAnimator != null)
        {
            towerAnimator.SetTrigger("ShootTrigger");
        }
        if (towerData.projectilePrefab != null)
        {
            GameObject projGO = ObjectPooler.Instance.GetObject(towerData.projectilePrefab, transform.position, Quaternion.identity);
            AudioManager.Instance.PlaySFX(towerShootSound, shootVolume);
            Projectile projectile = projGO.GetComponent<Projectile>();
            if (projectile != null)
            {
                
                projectile.Seek(target, towerData);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (towerData != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, towerData.range);
        }
    }
}