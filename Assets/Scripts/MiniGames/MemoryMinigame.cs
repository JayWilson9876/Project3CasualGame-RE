using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemoryMinigame : MonoBehaviour
{
    public Button[] cards;                    // Assign 4–6 buttons
    public TMP_Text resultText;

    public MiniGameStation station;           // Assigned at runtime

    private int[] cardValues;                 // 1,1,2,2,3,3
    private bool[] matched;
    private int firstIndex = -1;
    private bool inputLocked = false;

    void Start()
    {
        ResetBoard();
    }

    public void ResetBoard()
    {
        resultText.text = "";
        firstIndex = -1;
        inputLocked = false;

        int pairCount = cards.Length / 2;
        cardValues = new int[cards.Length];
        matched = new bool[cards.Length];

        // Create pairs
        List<int> vals = new List<int>();
        for (int i = 1; i <= pairCount; i++)
        {
            vals.Add(i);
            vals.Add(i);
        }

        // Shuffle
        for (int i = 0; i < vals.Count; i++)
        {
            int r = Random.Range(0, vals.Count);
            int temp = vals[i];
            vals[i] = vals[r];
            vals[r] = temp;
        }

        // Assign values
        vals.CopyTo(cardValues);

        // Setup buttons
        for (int i = 0; i < cards.Length; i++)
        {
            int index = i;
            cards[i].onClick.RemoveAllListeners();
            cards[i].onClick.AddListener(() => FlipCard(index));
            cards[i].GetComponentInChildren<TMP_Text>().text = "?";
        }
    }

    void FlipCard(int index)
    {
        if (inputLocked || matched[index]) return;

        cards[index].GetComponentInChildren<TMP_Text>().text = cardValues[index].ToString();

        // First card selected
        if (firstIndex == -1)
        {
            firstIndex = index;
        }
        else
        {
            // Second card selected
            StartCoroutine(CheckMatch(index));
        }
    }

    IEnumerator CheckMatch(int secondIndex)
    {
        inputLocked = true;

        yield return new WaitForSeconds(0.7f);

        if (cardValues[firstIndex] == cardValues[secondIndex])
        {
            matched[firstIndex] = true;
            matched[secondIndex] = true;

            // Check win
            if (AllMatched())
            {
                resultText.text = "You Win!";
                station.EndMinigame(50f);
                yield return new WaitForSeconds(1f);
                gameObject.SetActive(false);
            }
        }
        else
        {
            // Flip back
            cards[firstIndex].GetComponentInChildren<TextMeshProUGUI>().text = "?";
            cards[secondIndex].GetComponentInChildren<TextMeshProUGUI>().text = "?";
        }

        firstIndex = -1;
        inputLocked = false;
    }

    bool AllMatched()
    {
        foreach (bool m in matched)
            if (!m) return false;
        return true;
    }
}
