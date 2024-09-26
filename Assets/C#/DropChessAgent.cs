using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Unity.MLAgents.Actuators;
public class DropChessAgent : Agent
{
    public bool canDrop = true;
    public int Bot { get; private set; } = 2;
    public override void OnEpisodeBegin()
    {

        Bot = ChessManager.Instance.isPlayerFirst ? 2 : 1;
    }
    public override void CollectObservations(VectorSensor sensor)
    {

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                sensor.AddObservation(ChessManager.Instance.Chessboard[i, j]);
            }
        }
        Bot = ChessManager.Instance.isPlayerFirst ? 2 : 1;
        sensor.AddObservation(Bot);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        if (canDrop)
        {
            int x = actions.DiscreteActions[0] / 3;
            int y = actions.DiscreteActions[0] % 3;
            Bot = ChessManager.Instance.isPlayerFirst ? 2 : 1;
            if (ChessManager.Instance.DropChess(x, y, Bot))
            {
                canDrop = false;
                SetReward(2);
                if (ChessManager.CheckWin(Bot, ChessManager.Instance.Chessboard))
                {
                    if (Bot == 1)
                    {
                        SetReward(10);
                        ChessAgent.V1++;
                    }
                    else
                    {
                        SetReward(-10);
                        ChessAgent.V2++;
                    }
                    EndEpisode();
                }
            }
            else
            {
                Debug.Log("Invalid action");
            }
        }
    }
}
