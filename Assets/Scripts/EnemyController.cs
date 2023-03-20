using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float fieldOfViewAngle = 360f;
    public float sightRange = 50f;

    private Transform player;
    [SerializeField]
    private NavMeshAgent navMeshAgent;
    private Vector3 lastKnownPlayerPosition;
    private bool playerInSight;
    private bool chasingMode;
    private int enemyState;
    public float timeRemaining = 5f;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastKnownPlayerPosition = transform.position;
    }

    void Update()
    {
        CheckPlayerInSight();
        SetEnemyState();
    }

    private void CheckPlayerInSight()
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
                else {
                    if (enemyState == 2) {
                        
                    }
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

                timeRemaining = 5f;
                targetTimer();
                navMeshAgent.SetDestination(lastKnownPlayerPosition);

                break;

            case 3: // Target
                playerInSight = false;
                navMeshAgent.speed = chaseSpeed;
                
                chasingMode = true;


                navMeshAgent.SetDestination(player.position);

                break;
        }
    }

    private bool targetMode;

    private void targetTimer() {
        if (targetMode)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                //execute code here
                targetMode = false;
            }
        }

    }
}