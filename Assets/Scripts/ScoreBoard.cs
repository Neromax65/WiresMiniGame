using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ScoreBoard
{
    private const int TopCount = 10;
    private const string PlayerNamesKey = "Players";
    private const string PlayerScoresKey = "Scores";

    private static Dictionary<string, int> _scoreBoardDict;
        
    public static IEnumerable<KeyValuePair<string, int>> SaveScores()
    {
        IEnumerable<KeyValuePair<string, int>> sortedScoreBoard = _scoreBoardDict.OrderByDescending(kvp => kvp.Value).Take(TopCount);
        string[] playerNames = sortedScoreBoard.Select(kvp => kvp.Key).ToArray();
        string[] playerScores = sortedScoreBoard.Select(kvp => kvp.Value.ToString()).ToArray();
        PlayerPrefs.SetString(PlayerNamesKey, string.Join(",", playerNames));
        PlayerPrefs.SetString(PlayerScoresKey, string.Join(",", playerScores));
        return sortedScoreBoard;
    }

    public static void AddPlayerRecord(string playerName, int score)
    {
        if (_scoreBoardDict == null)
            LoadScores();
        
        if (_scoreBoardDict.Keys.Contains(playerName))
        {
            if (score > _scoreBoardDict[playerName])
                _scoreBoardDict[playerName] = score;
        } else 
            _scoreBoardDict.Add(playerName, score);
    } 
        
    public static void LoadScores()
    {
        _scoreBoardDict = new Dictionary<string, int>();
        string[] playerNames = PlayerPrefs.GetString(PlayerNamesKey).Split(new []{','}, StringSplitOptions.RemoveEmptyEntries);
        string[] playerScores = PlayerPrefs.GetString(PlayerScoresKey).Split(new []{','}, StringSplitOptions.RemoveEmptyEntries);
        if (playerNames.Length != playerScores.Length)
        {
            Debug.LogError("Player names count doesn`t match player scores count.");
            return;
        }
        for (int i = 0; i < playerNames.Length; i++)
        {
            _scoreBoardDict.Add(playerNames[i], int.Parse(playerScores[i]));
        }
    }
}