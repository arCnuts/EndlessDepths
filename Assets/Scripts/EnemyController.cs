using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float fieldOfViewAngle = 180f;
    public float sightRange = 10f;

    private Transform player;
    [SerializeField]
    private NavMeshAgent navMeshAgent;
    private Vector3 lastKnownPlayerPosition;
    private bool playerInSight;
    private int enemyState;
    public float timeRemaining;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastKnownPlayerPosition = transform.position;
        timeRemaining = 3;
    }

    void Update()
    {
        CheckPlayerSight();
        SetEnemyState();

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
    }

    private void CheckPlayerSight()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angleToPlayer = Vector3.Angle(directionToPlayer, transform.forward);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (angleToPlayer < fieldOfViewAngle * 0.5f && distanceToPlayer < sightRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, sightRange))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    enemyState = 2;
                    lastKnownPlayerPosition = player.position;
                    return;
                }
            }
        }
        enemyState = 1;

    }
    private void SetEnemyState()
    {
        switch (enemyState)
        {
            case 1: // Patrol
                playerInSight = false;
                navMeshAgent.speed = patrolSpeed;

                break;

            case 2: // Chase
                playerInSight = true;
                navMeshAgent.speed = chaseSpeed;

                navMeshAgent.SetDestination(lastKnownPlayerPosition);
                break;

            case 3: // Find
                playerInSight = false;
                navMeshAgent.speed = chaseSpeed;

                if (timeRemaining > 0) {
                    timeRemaining -= Time.deltaTime;
                }
                else {
                    enemyState = 1;
                }

                navMeshAgent.SetDestination(player.position);

                break;
        }
    }
}