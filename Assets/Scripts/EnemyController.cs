using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class EnemyController : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float fieldOfViewAngle = 360f;
    public float sightRange = 50f;

    private Transform player;
    [SerializeField]
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    // [SerializeField]
    // private Animator animator;

    public bool playerInSight;
    private bool targetMode = false;
    private float footstepFrequency = 0.6f;
    public float timeRemaining = 10f;
    public int enemyState = 1;
    private Vector3 lastKnownPlayerPosition;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastKnownPlayerPosition = transform.position;
    }

    void Update()
    {
        CheckPlayerInSight();
        SetEnemyState();
        Footsteps();
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
                if (hit.collider.CompareTag("Player")) {
                    enemyState = 2;
                }
                else {
                    if (enemyState == 2) {
                        timeRemaining = 10f;
                        targetMode = true;
                        enemyState = 3;

                    }
                }
            }
        }
    }

    private void SetEnemyState()
    {
        switch (enemyState)
        {
            case 1: // Patrol
                playerInSight = false;
                navMeshAgent.speed = patrolSpeed;
                navMeshAgent.SetDestination(player.position);
                break;
            case 2: // Chase
                playerInSight = true;
                navMeshAgent.speed = chaseSpeed;
                navMeshAgent.SetDestination(player.position);
                break;
            case 3: // Target
                playerInSight = false;
                navMeshAgent.speed = chaseSpeed;
                targetTimer();
                navMeshAgent.SetDestination(player.position);
                break;
        }
    }

    private void targetTimer() {
        if (!targetMode) return;
            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
            }
            else {
                enemyState = 1;                
                targetMode = false;
            }
    }
    

    public string FootstepRunningPath = "event:/FootstepsRunning";
    public string FootstepWalkingPath = "event:/FootstepsWalking";
    private float time;
    public float currentSpeed;

    private void Footsteps() {
        if (Time.time > time) {
            time = Time.time + footstepFrequency;
            currentSpeed = new Vector3(navMeshAgent.velocity.x, 0, navMeshAgent.velocity.z).magnitude;
            if (currentSpeed > 1f) {
                // animator.SetBool("isRunning", true);
            }
            else {
                // animator.SetBool("isRunning", false);
            }
        }
    }
}