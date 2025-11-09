using UnityEngine;

[CreateAssetMenu(fileName = "NewMonsterData", menuName = "Monster Daycare/Monster Data")]
public class MonsterData : ScriptableObject
{
    public string monsterName;
    public GameObject monsterPrefab;

    [Header("Need Rates (depletion per second)")]
    public float hungerRate = 1f;
    public float thirstRate = 1f;
    // public float funRate = 1f;
    public float hygieneRate = 1f;

    [Header("Visuals")]
    public Color monsterColor = Color.white;
    public Sprite portrait; // optional, for UI menus
}
