﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class ObstaclesLeaderboard : MonoBehaviour
{
    public GameObject leaderboardCanvas;
    public GameObject[] leaderboardEntries;

    public static ObstaclesLeaderboard instance;

    private void Awake()
    {
        instance = this;
    }

    public void OnEndGame()
    {
        leaderboardCanvas.SetActive(true);
        DisplayLeaderboard();
    }

    public void DisplayLeaderboard()
    {
        GetLeaderboardRequest getLeaderboardRequest = new GetLeaderboardRequest
        {
            StatisticName = "LeastObstaclesHit",
            MaxResultsCount = 10
        };

        PlayFabClientAPI.GetLeaderboard(getLeaderboardRequest,
            result => UpdateLeaderboardUI(result.Leaderboard),
            error => Debug.Log(error.ErrorMessage)
        );
    }

    void UpdateLeaderboardUI(List<PlayerLeaderboardEntry> leaderboard)
    {
        Debug.Log("updating ui");
        Debug.Log("leaderboard.count is " + leaderboard.Count);
        for (int x = 0; x < leaderboardEntries.Length; x++)
        {
            leaderboardEntries[x].SetActive(x < leaderboard.Count);
            if (x >= leaderboard.Count) continue;

            leaderboardEntries[x].transform.Find("Playername").GetComponent<TextMeshProUGUI>().text = (leaderboard[x].Position + 1) + ". " + leaderboard[x].DisplayName;
            leaderboardEntries[x].transform.Find("Scoretext").GetComponent<TextMeshProUGUI>().text = (leaderboard[x].StatValue).ToString();
        }
    }

    public void SetLeaderboardEntry(int obstaclesHit)
    {
        Debug.Log("newscore is " + obstaclesHit);
        ExecuteCloudScriptRequest request = new ExecuteCloudScriptRequest
        {
            FunctionName = "UpdateObstaclesLeaderboard",
            FunctionParameter = new {score = obstaclesHit }
        };

        PlayFabClientAPI.ExecuteCloudScript(request,
            result =>
            {
                Debug.Log(result.Error);
                Debug.Log(result.FunctionName);
                Debug.Log(result.FunctionResult);
                Debug.Log(result.FunctionResultTooLarge);
                DisplayLeaderboard();
            },
            error => Debug.Log(error.ErrorMessage)
        );
    }
}
