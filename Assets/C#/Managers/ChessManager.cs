using System.Collections.Generic;
using Unity.Sentis;
using UnityEngine;
using UnityEngine.UI;

public class ChessManager : MonoBehaviour
{
    public static ChessManager Instance { get; private set; }
    private int[,] _chessboard = new int[3, 3];
    public int[,] Chessboard => _chessboard;
    private Button[] _chessButton = new Button[9];
    private TMPro.TextMeshProUGUI[] _chessText = new TMPro.TextMeshProUGUI[9];
    private int _chessCount = 0;
    public bool isPlayerFirst = true;
    public int Player { get; private set; } = 1;
    public int Bot { get; private set; } = 2;
    public DropChessAgent dropChessAgent;
    public List<ModelAsset> models;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        switch(UIManager.Instance.DifficultyDropdown.value)
        {
            case 0:
                dropChessAgent.SetModel("My Behavior", models[0]);
                break;
            case 1:
                dropChessAgent.SetModel("My Behavior", models[1]);
                break;
            case 2:
                dropChessAgent.SetModel("My Behavior", models[2]);
                break;
            default:
                dropChessAgent.SetModel("My Behavior", models[0]);
                break;
        }
        switch (UIManager.Instance.IsFirstDropdown.value)
        {
            case 0:
                isPlayerFirst = UnityEngine.Random.Range(0, 2) == 0;
                break;
            case 1:
                isPlayerFirst = true;
                break;
            case 2:
                isPlayerFirst = false;
                break;
            default:
                isPlayerFirst = UnityEngine.Random.Range(0, 2) == 0;
                break;
        }
        Player = isPlayerFirst ? 1 : 2;
        Bot = isPlayerFirst ? 2 : 1;
        for (int i = 0; i < transform.childCount; i++)
        {
            _chessButton[i] = transform.GetChild(i).GetComponent<Button>();
            _chessText[i] = _chessButton[i].GetComponentInChildren<TMPro.TextMeshProUGUI>();
            int x = i / 3;
            int y = i % 3;
            _chessButton[i].onClick.AddListener(() =>
            {
                if (DropChess(x, y, Player))
                {
                    if (CheckWin(Player, Chessboard))
                    {
                        //ResetChessboard();
                    }
                    else
                    {
                        dropChessAgent.canDrop = true;
                        dropChessAgent.RequestDecision();

                        if (CheckWin(Bot, Chessboard))
                        {
                            //ResetChessboard();
                        }
                    }
                }
            });
        }
        ResetChessboard();
        if (!isPlayerFirst)
        {
            DropChess(UnityEngine.Random.Range(0, 3), UnityEngine.Random.Range(0, 3), Bot);
        }
    }

    private void OnDisable()
    {
        if (_chessCount != 0)
            ResetChessboard();
    }

    public static bool CheckWin(int player, int[,] _chessboard)
    {
        for (int i = 0; i < 3; i++)
        {
            if (_chessboard[i, 0] == player && _chessboard[i, 1] == player && _chessboard[i, 2] == player)
            {
                UIManager.Instance.ShowGameOverMenu( player + " wins!");
                return true;
            }
            if (_chessboard[0, i] == player && _chessboard[1, i] == player && _chessboard[2, i] == player)
            {
                UIManager.Instance.ShowGameOverMenu(player + " wins!");
                return true;
            }
        }
        if (_chessboard[0, 0] == player && _chessboard[1, 1] == player && _chessboard[2, 2] == player)
        {
            UIManager.Instance.ShowGameOverMenu( player + " wins!");
            return true;
        }
        if (_chessboard[0, 2] == player && _chessboard[1, 1] == player && _chessboard[2, 0] == player)
        {
            UIManager.Instance.ShowGameOverMenu(player + " wins!");
            return true;
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (_chessboard[i, j] == 0)
                {

                    return false;
                }
            }
        }
        UIManager.Instance.ShowGameOverMenu("Draw!");
        return false;
    }
    public void ResetChessboard()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                _chessboard[i, j] = 0;
                _chessText[i * 3 + j].text = "";
            }
        }
    }

    public bool DropChess(int x, int y, int player)
    {
        Debug.Log(player);
        if (_chessboard[x, y] == 0)
        {
            _chessboard[x, y] = player;
            _chessText[x * 3 + y].text = player == 1 ? "X" : "O";
            _chessText[x * 3 + y].color = player == 1 ? Color.red : Color.blue;
            _chessCount++;
            return true;
        }
        return false;
    }
}
