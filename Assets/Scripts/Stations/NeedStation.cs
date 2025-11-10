using UnityEngine;
using UnityEngine.UIElements;

public class NeedStation : MonoBehaviour
{
    [Header("Station Settings")]
    public string needType = "Thirst";  // which need it fulfills
    public float satisfyRate = 10f;     // how fast it restores per second
    public float range = 2f;            // how close the monster needs to be

    [Header("Progress Reward")]
    public float progressReward = 0.5f;
    public float rewardCooldown = 2f;

    private float lastRewardTime;

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range);
        foreach (Collider hit in hits)
        {
            MonsterManager monster = hit.GetComponent<MonsterManager>();
            if (monster != null)
            {
                // Try to satisfy the need (this will naturally fill gradually)
                float beforeValue = monster.GetNeedValue(needType);
                monster.SatisfyNeed(needType, satisfyRate * Time.deltaTime);
                float afterValue = monster.GetNeedValue(needType);

                // Check if we actually filled the need (value increased)
                bool needWasFilled = afterValue > beforeValue;

                // Only reward if cooldown elapsed AND need increased
                if (needWasFilled && Time.time - lastRewardTime >= rewardCooldown)
                {
                    GameManager.Instance.AddProgress(progressReward);
                    lastRewardTime = Time.time;
                    Debug.Log($"Gave progress +{progressReward} for helping {monster.monsterName}'s {needType}");
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
