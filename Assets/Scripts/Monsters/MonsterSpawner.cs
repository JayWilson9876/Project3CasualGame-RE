using UnityEngine;
using System.Collections.Generic;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Monster Pool")]
    public MonsterData[] allMonsters; // assign all monster data here

    [Header("Spawn Points")]
    public Transform[] spawnPoints;

    private List<GameObject> activeMonsters = new List<GameObject>();

    void Start()
    {
        GameManager.Instance.OnNewDayStarted.AddListener(OnNewDay);
    }

    void OnNewDay(int day)
    {
        Debug.Log("MonsterSpawner heard OnNewDay: " + day);

        int monstersToHave = day; // 1 monster on day 1, 2 on day 2, etc.
        int monstersToAdd = monstersToHave - activeMonsters.Count;

        for (int i = 0; i < monstersToAdd; i++)
        {
            SpawnNewMonster();
        }
    }

    void SpawnNewMonster()
    {
        if (activeMonsters.Count >= allMonsters.Length) return;

        // pick a random monster type not already active
        List<MonsterData> available = new List<MonsterData>(allMonsters);
        foreach (var m in activeMonsters)
        {
            MonsterManager n = m.GetComponent<MonsterManager>();
            if (n != null)
                available.RemoveAll(x => x.monsterName == n.monsterName);
        }

        if (available.Count == 0) return;

        MonsterData randomMonster = available[Random.Range(0, available.Count)];
        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject monsterObj = Instantiate(randomMonster.monsterPrefab, spawn.position, spawn.rotation);
        activeMonsters.Add(monsterObj);

        MonsterManager needs = monsterObj.GetComponent<MonsterManager>();
        if (needs != null)
        {
            needs.monsterName = randomMonster.monsterName;
            needs.needs = new MonsterNeed[]
            {
                new MonsterNeed { needName = "Hunger", depletionRate = randomMonster.hungerRate },
                new MonsterNeed { needName = "Thirst", depletionRate = randomMonster.thirstRate },
                // new MonsterNeed { needName = "Fun", depletionRate = randomMonster.funRate },
                new MonsterNeed { needName = "Hygiene", depletionRate = randomMonster.hygieneRate }
            };
        }
    }
}
