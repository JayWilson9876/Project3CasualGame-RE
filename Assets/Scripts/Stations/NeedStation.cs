using UnityEngine;

public class NeedStation : MonoBehaviour
{
    [Header("Station Settings")]
    public string needType = "Thirst";  // which need it fulfills
    public float satisfyRate = 10f;     // how fast it restores per second
    public float range = 2f;            // how close the monster needs to be

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range);
        foreach (Collider hit in hits)
        {
            MonsterManager monster = hit.GetComponent<MonsterManager>();
            if (monster != null)
            {
                monster.SatisfyNeed(needType, satisfyRate * Time.deltaTime);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
