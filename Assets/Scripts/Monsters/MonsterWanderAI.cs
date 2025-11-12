using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class MonsterWanderAI : MonoBehaviour
{
    [Header("Wander Settings")]
    public float wanderRadius = 5f;     // how far from origin it can wander
    public float wanderInterval = 3f;   // how often to pick a new destination
    public float idleTime = 1f;         // pause time between moves

    private NavMeshAgent agent;
    private Vector3 origin;
    private float wanderTimer;
    private bool isIdle;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        origin = transform.position;
        wanderTimer = wanderInterval * Random.Range(0.5f, 1.5f); // slight randomness
    }

    void Update()
    {
        if (isIdle) return;

        wanderTimer += Time.deltaTime;

        if (wanderTimer >= wanderInterval && agent.enabled)
        {
            Vector3 newPos = RandomNavSphere(origin, wanderRadius);
            if (newPos != Vector3.zero)
            {
                agent.SetDestination(newPos);
                wanderTimer = 0f;
                StartCoroutine(IdleAfterMove());
            }
        }
    }

    IEnumerator IdleAfterMove()
    {
        isIdle = true;
        yield return new WaitForSeconds(idleTime);
        isIdle = false;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randomDirection = Random.insideUnitSphere * dist;
        randomDirection += origin;
        NavMeshHit navHit;

        if (NavMesh.SamplePosition(randomDirection, out navHit, dist, NavMesh.AllAreas))
            return navHit.position;
        else
            return Vector3.zero;
    }
}
