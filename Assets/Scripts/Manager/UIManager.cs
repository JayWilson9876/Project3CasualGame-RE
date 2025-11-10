using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Slider progressSlider;
    public TMP_Text dayText;

    void Awake()
    {
        Instance = this;
    }

    public void UpdateProgressBar(float normalizedValue)
    {
        progressSlider.value = Mathf.Clamp01(normalizedValue);
    }

    public void ResetProgressBar(float newMax)
    {
        progressSlider.maxValue = 1f;
        progressSlider.value = 0f;
        dayText.text = $"Day {GameManager.Instance.currentDay}";
    }
}
