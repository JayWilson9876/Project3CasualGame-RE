using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MonsterManager : MonoBehaviour
{
    [Header("Monster Info")]
    public string monsterName = "Mushroom Monster";

    [Header("Needs")]
    public MonsterNeed[] needs;

    [Header("Behavior")]
    public float checkInterval = 1f; // how often to update
    private float checkTimer;

    [Header("Monster Components")]
    private NavMeshAgent agent;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        checkTimer += Time.deltaTime;
        if (checkTimer >= checkInterval)
        {
            UpdateNeeds();
            checkTimer = 0f;
        }
    }

    void UpdateNeeds()
    {
        foreach (MonsterNeed need in needs)
        {
            need.UpdateNeed(checkInterval);
        }

        // Example: Log which needs are low
        foreach (MonsterNeed need in needs)
        {
            if (need.IsLow())
            {
                Debug.Log($"{monsterName} needs {need.needName}!");
            }
        }
    }

    // Call this when the player gives the monster something
    public void SatisfyNeed(string needName, float amount)
    {
        foreach (MonsterNeed need in needs)
        {
            if (need.needName == needName)
            {
                need.Satisfy(amount);
                Debug.Log($"{monsterName} satisfied {needName}!");
                return;
            }
        }
        Debug.LogWarning($"{monsterName} has no need called {needName}!");
    }

    public float GetNeedValue(string needName)
    {
        foreach (MonsterNeed need in needs)
        {
            if (need.needName == needName)
                return need.currentValue;
        }
        return 0f;
    }

    #region Pickup Monster
    public void OnPickedUp()
    {
        if (agent != null)
            agent.enabled = false;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = false;
        }
    }

    public void OnDropped()
    {
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        if (agent != null)
        {
            StartCoroutine(ReenableAgentNextFrame());
        }
    }

    private IEnumerator ReenableAgentNextFrame()
    {
        yield return null;
        agent.enabled = true;
        agent.Warp(transform.position);
    }
    #endregion Pickup Monster
}
