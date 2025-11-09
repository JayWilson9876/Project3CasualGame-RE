using UnityEngine;

[System.Serializable]
public class MonsterNeed
{
    public string needName;
    public float currentValue = 100f;  // starts full
    public float depletionRate = 1f;   // how fast it depletes per second

    public void UpdateNeed(float deltaTime)
    {
        currentValue -= depletionRate * deltaTime;
        currentValue = Mathf.Clamp(currentValue, 0f, 100f);
    }

    public bool IsLow(float threshold = 25f)
    {
        return currentValue <= threshold;
    }

    public void Satisfy(float amount)
    {
        currentValue = Mathf.Clamp(currentValue + amount, 0f, 100f);
    }
}
