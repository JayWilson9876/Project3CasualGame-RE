using UnityEngine;
using UnityEngine.UI;

public class MonsterNeedUI : MonoBehaviour
{
    public MonsterManager monster; // reference to monster
    public string needToDisplay = "Thirst";
    public Slider needSlider;
    public Camera mainCamera;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        if (monster == null || needSlider == null) return;

        // Find the need we’re tracking
        foreach (var need in monster.needs)
        {
            if (need.needName == needToDisplay)
            {
                needSlider.value = need.currentValue / 100f; // normalized
                break;
            }
        }

        // Always face the camera
        transform.LookAt(transform.position + mainCamera.transform.forward);
    }
}
