using UnityEngine;

public class MiniGameStation : MonoBehaviour
{
    [Header("Station Settings")]
    public float range = 2f;

    [Header("Minigame Panels")]
    public GameObject ticTacToePanel;
    public TicTacToeMinigame ticTacToe;

    public GameObject memoryPanel;
    public MemoryMinigame memory;

    public GameObject reactionPanel;
    public ReactionMinigame reaction;

    private MonsterManager monster;
    private bool isBusy = false;

    public PlayerLook playerLookScript;
    public PlayerController playerControllerScript;

    void Update()
    {
        if (isBusy) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, range);
        foreach (var hit in hits)
        {
            MonsterManager m = hit.GetComponent<MonsterManager>();
            if (m != null)
            {
                monster = m;

                if (Input.GetKeyDown(KeyCode.F))
                    StartRandomMinigame();

                break;
            }
        }
    }

    void StartRandomMinigame()
    {
        if (monster == null) return;
        isBusy = true;

        // Disable monster movement while in game
        monster.OnPickedUp();

        // Choose a random minigame
        int choice = Random.Range(0, 3);

        switch (choice)
        {
            case 0:
                StartTicTacToe();
                break;

            case 1:
                StartMemoryGame();
                break;

            case 2:
                StartReactionGame();
                break;
        }
    }

    void StartTicTacToe()
    {
        UnlockCursor();
        ticTacToePanel.SetActive(true);
        ticTacToe.station = this;
        ticTacToe.ResetBoard();
    }

    void StartMemoryGame()
    {
        UnlockCursor();
        memoryPanel.SetActive(true);
        memory.station = this;
        memory.ResetBoard();
    }

    void StartReactionGame()
    {
        UnlockCursor();
        reactionPanel.SetActive(true);
        reaction.station = this;
        // Reaction game auto-resets on enable
    }

    // Called by any minigame when it ends
    public void EndMinigame(float funRestored)
    {
        if (monster != null)
        {
            monster.SatisfyNeed("Fun", funRestored);
            GameManager.Instance.AddProgress(0.1f);
            monster.OnDropped();
        }

        monster = null;
        isBusy = false;

        LockCursor();
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerLookScript != null && playerControllerScript != null)
        {
            playerLookScript.enabled = false;
            playerControllerScript.enabled = false;
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerLookScript != null && playerControllerScript != null)
        {
            playerLookScript.enabled = true;
            playerControllerScript.enabled = true;
        }
            
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
