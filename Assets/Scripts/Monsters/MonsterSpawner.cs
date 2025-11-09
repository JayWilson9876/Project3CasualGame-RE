using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Monster Pool")]
    public MonsterData[] allMonsters; // assign all monster data here
    public int monstersPerDay = 4;

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    void Start()
    {
        SpawnMonstersForDay();
    }

    void SpawnMonstersForDay()
    {
        if (allMonsters.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("Missing monster data or spawn points!");
            return;
        }

        // Shuffle or pick random monsters
        MonsterData[] chosenMonsters = PickRandomMonsters(monstersPerDay);

        for (int i = 0; i < chosenMonsters.Length; i++)
        {
            MonsterData data = chosenMonsters[i];
            Transform spawn = spawnPoints[i % spawnPoints.Length];

            GameObject monsterObj = Instantiate(data.monsterPrefab, spawn.position, spawn.rotation);

            // Configure needs dynamically
            MonsterManager needs = monsterObj.GetComponent<MonsterManager>();
            if (needs != null)
            {
                needs.monsterName = data.monsterName;
                needs.needs = new MonsterNeed[]
                {
                    new MonsterNeed { needName = "Hunger", depletionRate = data.hungerRate },
                    new MonsterNeed { needName = "Thirst", depletionRate = data.thirstRate },
                    // new MonsterNeed { needName = "Fun", depletionRate = data.funRate },
                    new MonsterNeed { needName = "Hygiene", depletionRate = data.hygieneRate }
                };
            }

            // Optional: color or visuals
            Renderer r = monsterObj.GetComponent<Renderer>();
            if (r != null) r.material.color = data.monsterColor;
        }
    }

    MonsterData[] PickRandomMonsters(int count)
    {
        MonsterData[] result = new MonsterData[Mathf.Min(count, allMonsters.Length)];
        System.Collections.Generic.List<MonsterData> pool = new System.Collections.Generic.List<MonsterData>(allMonsters);

        for (int i = 0; i < result.Length; i++)
        {
            int index = Random.Range(0, pool.Count);
            result[i] = pool[index];
            pool.RemoveAt(index);
        }
        return result;
    }
}
