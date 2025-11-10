using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [Header("Monster Info")]
    public string monsterName = "Mushroom Monster";

    [Header("Needs")]
    public MonsterNeed[] needs;

    [Header("Behavior")]
    public float checkInterval = 1f; // how often to update
    private float checkTimer;

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
}
