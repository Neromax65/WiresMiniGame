using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Класс, отвечающий за сохранение и загрузку списка лидеров
/// </summary>
public static class Leaderboard
{
    /// <summary>
    /// Количество лидеров
    /// </summary>
    private const int TopCount = 10;
    
    /// <summary>
    /// Ключ для сохранения имен игроков 
    /// </summary>
    private const string PlayerNamesKey = "Players";
    
    /// <summary>
    /// Ключ для сохранения очков игроков
    /// </summary>
    private const string PlayerScoresKey = "Scores";

    private static Dictionary<string, int> _leaderboardDict;
         
    /// <summary>
    /// Сортировка и сохранение списка лидеров в PlayerPrefs
    /// </summary>
    /// <returns>Отсортированный список лидеров</returns>
    public static IEnumerable<KeyValuePair<string, int>> SaveScores()
    {
        IEnumerable<KeyValuePair<string, int>> sortedLeaderboard = _leaderboardDict.OrderByDescending(kvp => kvp.Value).Take(TopCount);
        string[] playerNames = sortedLeaderboard.Select(kvp => kvp.Key).ToArray();
        string[] playerScores = sortedLeaderboard.Select(kvp => kvp.Value.ToString()).ToArray();
        PlayerPrefs.SetString(PlayerNamesKey, string.Join(",", playerNames));
        PlayerPrefs.SetString(PlayerScoresKey, string.Join(",", playerScores));
        return sortedLeaderboard;
    }

    /// <summary>
    /// Добавить рекорд игрока
    /// </summary>
    /// <param name="playerName">Имя игрока</param>
    /// <param name="score">Количество очков</param>
    public static void AddPlayerRecord(string playerName, int score)
    {
        if (_leaderboardDict == null)
            LoadScores();
        
        if (_leaderboardDict.Keys.Contains(playerName))
        {
            if (score > _leaderboardDict[playerName])
                _leaderboardDict[playerName] = score;
        } else 
            _leaderboardDict.Add(playerName, score);
    } 
    
    /// <summary>
    /// Загрузить список лидеров из PlayerPrefs
    /// </summary>
    public static void LoadScores()
    {
        _leaderboardDict = new Dictionary<string, int>();
        string[] playerNames = PlayerPrefs.GetString(PlayerNamesKey).Split(new []{','}, StringSplitOptions.RemoveEmptyEntries);
        string[] playerScores = PlayerPrefs.GetString(PlayerScoresKey).Split(new []{','}, StringSplitOptions.RemoveEmptyEntries);
        if (playerNames.Length != playerScores.Length)
        {
            Debug.LogError("Player names count doesn`t match player scores count.");
            return;
        }
        for (int i = 0; i < playerNames.Length; i++)
        {
            _leaderboardDict.Add(playerNames[i], int.Parse(playerScores[i]));
        }
    }
}