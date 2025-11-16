using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToeMinigame : MonoBehaviour
{
    public Button[] cells;           // assign 9 buttons in inspector
    public TMP_Text resultText;
    public MiniGameStation station;  // assigned by station when opening

    private int[] board = new int[9]; // 0 = empty, 1 = player, 2 = AI
    private bool playerTurn = true;
    private bool gameOver = false;

    void Start()
    {
        ResetBoard();
    }

    public void ResetBoard()
    {
        board = new int[9];
        resultText.text = "";
        gameOver = false;
        playerTurn = true;

        for (int i = 0; i < cells.Length; i++)
        {
            int index = i;
            cells[i].GetComponentInChildren<TMP_Text>().text = "";
            cells[i].onClick.RemoveAllListeners();
            cells[i].onClick.AddListener(() => PlayerMove(index));
        }
    }

    void PlayerMove(int index)
    {
        if (gameOver || board[index] != 0) return;

        board[index] = 1;
        cells[index].GetComponentInChildren<TMP_Text>().text = "X";

        if (CheckWin(1))
        {
            GameOver(true);
            return;
        }

        if (IsBoardFull())
        {
            GameOver(false);
            return;
        }

        Invoke(nameof(AIMove), 0.4f);
    }

    void AIMove()
    {
        if (gameOver) return;

        // simple AI: pick first empty cell
        for (int i = 0; i < 9; i++)
        {
            if (board[i] == 0)
            {
                board[i] = 2;
                cells[i].GetComponentInChildren<TMP_Text>().text = "O";
                break;
            }
        }

        if (CheckWin(2))
        {
            GameOver(false);
            return;
        }

        if (IsBoardFull())
        {
            GameOver(false);
        }
    }

    void GameOver(bool playerWon)
    {
        gameOver = true;

        if (playerWon)
        {
            resultText.text = "You Win!";
            station.EndMinigame(40f);  // give Fun + progress
        }
        else
        {
            resultText.text = "Tie!";
            station.EndMinigame(20f);
        }

        // delay closing the board
        Invoke(nameof(ClosePanel), 1.2f);
    }

    void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    bool IsBoardFull()
    {
        foreach (int c in board)
            if (c == 0) return false;
        return true;
    }

    // Win condition
    bool CheckWin(int val)
    {
        int[,] lines = new int[,] {
            {0,1,2},{3,4,5},{6,7,8}, // rows
            {0,3,6},{1,4,7},{2,5,8}, // columns
            {0,4,8},{2,4,6}          // diagonals
        };

        for (int i = 0; i < lines.GetLength(0); i++)
        {
            if (board[lines[i, 0]] == val &&
                board[lines[i, 1]] == val &&
                board[lines[i, 2]] == val)
                return true;
        }

        return false;
    }
}
