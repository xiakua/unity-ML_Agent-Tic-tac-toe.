using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
public class ChessAgent : Agent
{
    public static int V1 = 0;
    public static int V2 = 0;
    public static int D = 0;
    public int[,] Chessboard { get; private set; } = new int[3, 3];
    public int Player { get; private set; } = 1;
    public int count = 0;
    public override void OnEpisodeBegin()
    {
        //Debug.Log("Episode Begin");
        Chessboard = new int[3, 3];
        count = 1;
        Player = 2;
        Chessboard[Random.Range(0, 3), Random.Range(0, 3)] = 1;

    }
    public override void CollectObservations(VectorSensor sensor)
    {
        for (int i = 0; i < Chessboard.GetLength(0); i++)
        {
            for (int j = 0; j < Chessboard.GetLength(1); j++)
            {
                sensor.AddObservation(Chessboard[i, j]);
            }
        }
        sensor.AddObservation(Player);

    }
    public override void OnActionReceived(ActionBuffers actions)
    {
       // Debug.Log($"Bot {Bot}'s turn");
       //Debug.Log(count);
       // Debug.Log(actions.DiscreteActions.Length);
       // Debug.Log("---------------------------");
        int x = actions.DiscreteActions[0] / 3;
        int y = actions.DiscreteActions[0] % 3;
        Player = count % 2 + 1;
        if (DropChess(x, y, Player))
        {
            count++;
            SetReward(10);
            if (CheckWin(Player, Chessboard))
            {
                SetReward(20);
                //Debug.Log("Bot " + Bot + " wins!");
                if (Player == 1)
                {
                    V1++;
                }
                else
                {
                    V2++;
                }
                EndEpisode();
            }
            else if (count == 9)
            {
                //Debug.Log("Draw!");
                D++;
                SetReward(50);
                EndEpisode();
            }
        }
        else
        {
            SetReward(-20);
        }

    }

    public bool DropChess(int x, int y, int player)
    {
        if (Chessboard[x, y] == 0)
        {
            Chessboard[x, y] = player;
            return true;
        }
        return false;
    }
    public static bool CheckWin(int player, int[,] _chessboard)
    {
        for (int i = 0; i < 3; i++)
        {
            if (_chessboard[i, 0] == player && _chessboard[i, 1] == player && _chessboard[i, 2] == player)
            {
                return true;
            }
            if (_chessboard[0, i] == player && _chessboard[1, i] == player && _chessboard[2, i] == player)
            {
                return true;
            }
        }
        if (_chessboard[0, 0] == player && _chessboard[1, 1] == player && _chessboard[2, 2] == player)
        {
            return true;
        }
        if (_chessboard[0, 2] == player && _chessboard[1, 1] == player && _chessboard[2, 0] == player)
        {
            return true;
        }
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (_chessboard[i, j] != 0)
                {
                    return false;
                }
            }
        }
        return false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("V1: " + V1 + " V2: " + V2 + " D: " + D);
        }
    }
}
