using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentDay = 1;
    public int maxDays = 5;
    public UnityEvent<int> OnNewDayStarted; // UI or spawner can subscribe

    [Header("Progress System")]
    public float currentProgress = 0f;
    public float progressToNextDay = 5f; // how many "tasks" or actions fill a day

    private bool dayInProgress = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SoundManager.instance.Play("MenuMusic");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1")
        {
            StartCoroutine(StartDayAfterSceneLoad());
            SoundManager.instance.Stop("MenuMusic");
            SoundManager.instance.Play("LevelMusic");
        }
    }

    public void AddProgress(float amount)
    {
        if (!dayInProgress) return;

        currentProgress += amount;
        UIManager.Instance?.UpdateProgressBar(currentProgress / progressToNextDay);

        if (currentProgress >= progressToNextDay)
        {
            EndDay();
        }
    }

    void EndDay()
    {
        dayInProgress = false;
        currentDay++;
        currentProgress = 0f;
        progressToNextDay *= 2f;

        if (currentDay > maxDays)
        {
            Debug.Log("All days complete!");
            return;
        }

        progressToNextDay += 2f; // increase required progress per day
        UIManager.Instance?.ResetProgressBar(progressToNextDay);
        StartNewDay();
    }

    void StartNewDay()
    {
        dayInProgress = true;
        Debug.Log($"Starting Day {currentDay}");
        OnNewDayStarted?.Invoke(currentDay);
    }

    private IEnumerator StartDayAfterSceneLoad()
    {
        yield return null;
        StartNewDay();
    }
}
