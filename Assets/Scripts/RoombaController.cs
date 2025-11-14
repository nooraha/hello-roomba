using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.AI;

public class RoombaController : MonoBehaviour
{
    Transform player;
    NavMeshAgent agent;
    Transform patrolPointsParent;
    List<Transform> patrolPoints = new List<Transform>();

    float chaseRange = 20f;
    float attackRange = 1f;
    float baseSpeedMultiplier = 1f;
    float baseAngularSpeed = 180f;
    float chaseSpeed = 2f;
    float roamSpeed = 1f;
    float playerLostWhileChasingTimer;
    float roombaAttentionSpan = 6f;
    float roombaSightDistance = 200f;

    bool attacking = false;
    bool chasing = false;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        patrolPointsParent = GameObject.FindGameObjectWithTag("PatrolPoints").transform;
    }
    void Start()
    {
        foreach (Transform p in patrolPointsParent)
        {
            patrolPoints.Add(p);
        }
    }

    void Update()
    {
        // if bool attacking then doAttackState
        // if bool chasing then doChaseState
        // if neither of these then roam i guess, doRoamState
        if (attacking)
        {
            AttackPlayer();
        }
        else if (chasing)
        {
            DoChaseState();
        }
        else
        {
            DoRoamState();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Knockable"))
        {
            StunRoombaForSeconds(2f);
        }
    }

    float DistanceToPlayer()
    {
        return Vector3.Distance(player.transform.position, transform.position);
    }

    void EnterChaseState()
    {
        agent.speed = baseSpeedMultiplier * chaseSpeed;
        agent.angularSpeed = baseAngularSpeed * 2;
        playerLostWhileChasingTimer = 0f;
        chasing = true;

        Debug.Log("Roomba entered chase state");
    }

    void DoChaseState()
    {
        // pathfind to player (or last pos player has been seen in)
        // move towards target pos
        agent.SetDestination(player.position);

        // if the player is within attack range, enter attack state
        if (DistanceToPlayer() < attackRange)
        {
            EnterAttackState();
        }

        // check if player is within sight
        bool playerInSight = false;
        Vector3 toPlayer = player.position - Vector3.up * 0.5f - transform.position;

        Vector3 origin1 = transform.position + Vector3.up * 2.5f;
        Vector3 origin2 = transform.position + transform.forward * 3.5f;

        Debug.DrawRay(origin1, toPlayer.normalized * roombaSightDistance, Color.red);
        Debug.DrawRay(origin2, toPlayer.normalized * roombaSightDistance, Color.red);

        if (Physics.Raycast(origin1, toPlayer, out RaycastHit hit1, roombaSightDistance))
        {
            if (hit1.collider != null && hit1.collider.CompareTag("Player"))
            {
                playerInSight = true;
            }
        }
        if (Physics.Raycast(origin2, toPlayer, out RaycastHit hit2, roombaSightDistance))
        {
            if (hit2.collider != null && hit2.collider.CompareTag("Player"))
            {
                playerInSight = true;
            }
        }

        // if player is not within sight, update timer
        if (playerInSight)
        {
            playerLostWhileChasingTimer = 0f;
            Debug.Log("Player is in sight");
        }
        else
        {
            playerLostWhileChasingTimer += Time.deltaTime;
            Debug.Log("Player is not in sight");
        }

        // if timer is more than set val, enter roaming state
        if (playerLostWhileChasingTimer >= roombaAttentionSpan)
        {
            Debug.Log("Player not seen in a while, roomba lost interest");
            EnterRoamState();
        }
    }

    void EnterAttackState()
    {
        // game over for cat idk what happens here yet
        // run over the cat i guess
        attacking = true;

        Debug.Log("Roomba entered attack state");
    }

    void AttackPlayer()
    {
        // angry vroooommmm
    }

    void EnterRoamState()
    {
        agent.speed = baseSpeedMultiplier * roamSpeed;
        agent.angularSpeed = baseAngularSpeed;
        chasing = false;
        attacking = false;

        Debug.Log("Roomba entered roaming state");
    }

    void DoRoamState()
    {
        // roam around randomly, pathfind to random locations ?
        // maybe check specific locations where food spawns (make food spawn within set areas, but randomly and in a random area)
        agent.SetDestination(patrolPoints[Random.Range(0, patrolPoints.Count)].position);

        // check if player is within chase range, if so enterChaseState
        if (DistanceToPlayer() < chaseRange)
        {
            EnterChaseState();
        }

    }

    public void StunRoombaForSeconds(float seconds)
    {
        // enterStunState
        // after seconds, exitStunState, go back to chasing/roaming? 
        EnterStunState();

        Debug.Log("Stunned roomba for " + seconds);

        Invoke("ExitStunState", seconds);

        
    }

    void EnterStunState()
    {
        // stop attacking but keep chasing/roaming state whichever roomba was in (to go back to it afterwards)
        // set speed and angularspeed to 0
        // my guy is stunned
        agent.speed = 0f;
        agent.angularSpeed = 0f;

        Debug.Log("Roomba entered stun state");
    }
    
    void ExitStunState()
    {
        EnterChaseState();
    }


}
