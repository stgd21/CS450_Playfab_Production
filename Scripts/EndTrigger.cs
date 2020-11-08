using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject.Find("Player").GetComponent<PlayerController>().End();
        Leaderboard.instance.OnEndGame();
        CollectiblesLeaderboard.instance.OnEndGame();
        ObstaclesLeaderboard.instance.OnEndGame();
    }
}
