using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [Header("Особливості")]
    public bool isImmuneToSlow = false;

    [Header("Movement Settings")]
    public float speed = 2f;
    public float baseSpeed = 2f;
    private float slowTimer = 0f;

    [Header("Path Settings")]
    public Transform[] waypoints;
    public int currentWaypointIndex = 0;

    [Header("Base Damage")]
    public int damageToBase = 1;

    void Start()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            enabled = false;
        }
    }

    void Update()
    {
        if (slowTimer > 0)
    {
        slowTimer -= Time.deltaTime;
        if (slowTimer <= 0)
        {
            speed = baseSpeed;
        }
    }
        MoveAlongPath();
    }

    private void MoveAlongPath()
    {
        Vector3 targetPosition = waypoints[currentWaypointIndex].position;


        transform.position = Vector3.MoveTowards(
            transform.position, 
            targetPosition, 
            speed * Time.deltaTime
        );


        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                ReachBase();
            }
        }
    }

    private void ReachBase()
    {
        
        BaseHealth baseHealth = FindObjectOfType<BaseHealth>();
        
        if (baseHealth != null)
        {
            baseHealth.TakeDamage(damageToBase); 
        }
        
        gameObject.SetActive(false); 
    }
    public void ApplySlow(float slowPercentage, float duration)
    {
        if (isImmuneToSlow)
        {
            return; 
        }
        speed = baseSpeed * slowPercentage;
        slowTimer = duration;
    }
    
    void OnEnable()
    {
        currentWaypointIndex = 0;
        slowTimer = 0f;

        speed = baseSpeed;
    }
}