using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReactionMinigame : MonoBehaviour
{
    public Button reactionButton;
    public TMP_Text promptText;
    public MiniGameStation station;

    private float startTime;
    private bool clickable;

    void OnEnable()
    {
        StartCoroutine(StartSequence());
    }

    IEnumerator StartSequence()
    {
        clickable = false;
        promptText.text = "Wait for it...";

        // Random delay before it's clickable
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        promptText.text = "CLICK!";
        clickable = true;
        startTime = Time.time;

        // Enable button
        reactionButton.onClick.RemoveAllListeners();
        reactionButton.onClick.AddListener(Clicked);
    }

    void Clicked()
    {
        if (!clickable) return;

        float reaction = Time.time - startTime;

        clickable = false;

        float funAmount;
        if (reaction < 0.25f)
        {
            promptText.text = "Amazing! (" + reaction.ToString("F2") + "s)";
            funAmount = 50f;
        }
        else if (reaction < 0.5f)
        {
            promptText.text = "Good! (" + reaction.ToString("F2") + "s)";
            funAmount = 35f;
        }
        else
        {
            promptText.text = "Slow! (" + reaction.ToString("F2") + "s)";
            funAmount = 20f;
        }

        station.EndMinigame(funAmount);

        StartCoroutine(Close());
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
