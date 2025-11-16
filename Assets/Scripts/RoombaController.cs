using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class RoombaController : MonoBehaviour
{
    public static UnityEvent attackedPlayer = new UnityEvent();
    public static UnityEvent startedChasingPlayer = new UnityEvent();
    public static UnityEvent stoppedChasingPlayer = new UnityEvent();

    public static UnityEvent playerEnteredCloseRange = new UnityEvent();
    public static UnityEvent playerLeftCloseRange = new UnityEvent();
    Transform player;
    NavMeshAgent agent;
    Transform patrolPointsParent;
    List<Transform> patrolPoints = new List<Transform>();

    float chaseRange = 10f;
    float closeRange = 6f;
    float attackRange = 4f;
    float baseSpeedMultiplier = 1f;
    float baseAngularSpeed = 180f;
    float chaseSpeed = 2f;
    float roamSpeed = 1f;
    float attackSpeed = 4f;
    float playerLostWhileChasingTimer;
    float roombaAttentionSpan = 6f;
    float roombaSightDistance = 10f;
    bool chasing = false;
    bool inCloseRange = false;
    bool attacking = false;

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
        if (chasing)
        {
            DoChaseState();
        }
        else
        {
            DoRoamState();
        }

        if(attacking)
        {
            CheckIfGameOver();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Knockable"))
        {
            StunRoombaForSeconds(2f);
        }
    }

    float DistanceToPlayer()
    {
        return Vector3.Distance(player.transform.position, transform.position);
    }

    void CheckIfGameOver()
    {
        if (DistanceToPlayer() < 1f)
        {
            GameStateManager.Instance.LoseGame();
        }
    }

    void EnterChaseState()
    {
        agent.speed = baseSpeedMultiplier * chaseSpeed;
        agent.angularSpeed = baseAngularSpeed * chaseSpeed;
        playerLostWhileChasingTimer = 0f;
        chasing = true;

        Debug.Log("Roomba entered chase state");
        startedChasingPlayer.Invoke();
    }

    void DoChaseState()
    {
        // pathfind to player (or last pos player has been seen in)
        // move towards target pos
        agent.SetDestination(player.position);

        // if the player is within attack range, enter attack state
        if (!inCloseRange && DistanceToPlayer() < closeRange)
        {
            playerEnteredCloseRange.Invoke();
            inCloseRange = true;
        }
        if (inCloseRange && DistanceToPlayer() > closeRange)
        {
            playerLeftCloseRange.Invoke();
            inCloseRange = false;
        }

        if (DistanceToPlayer() < attackRange)
        {
            stoppedChasingPlayer.Invoke();
            AttackPlayer();
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
        }
        else
        {
            playerLostWhileChasingTimer += Time.deltaTime;
        }

        // if timer is more than set val, enter roaming state
        if (playerLostWhileChasingTimer >= roombaAttentionSpan)
        {
            Debug.Log("Player not seen in a while, roomba lost interest");
            stoppedChasingPlayer.Invoke();
            EnterRoamState();
        }
    }


    void AttackPlayer()
    {
        agent.speed = baseSpeedMultiplier * attackSpeed;
        agent.angularSpeed = baseAngularSpeed * attackSpeed;
        // angry vroooommmm
        // make player screen go to red or something lmao
        attackedPlayer.Invoke();
        // player.GetComponent<PlayerMovement>().StopMovement();
        // GameStateManager.Instance.Invoke("LoseGame", 2f);
        attacking = true;
    }

    void EnterRoamState()
    {
        agent.speed = baseSpeedMultiplier * roamSpeed;
        agent.angularSpeed = baseAngularSpeed;
        chasing = false;

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
