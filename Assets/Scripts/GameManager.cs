using System;
using System.Collections;
using System.IO;
using System.Linq;
using Extensions;
using Models;
using Newtonsoft.Json;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private const string ConfigName = "DifficultySettings.json";
    
    private WindowManager _windowManager => WindowManager.Instance;
    private WireSpawner _wireSpawner => WireSpawner.Instance;
    
    private DifficultySettings _difficultySettings;
    
    private int _level = 0;

    private int Level
    {
        get => _level;
        set
        {
            _level = value;
            var eventHandler = LevelChanged;
            eventHandler?.Invoke(_level);
        }
    }
    private int _score = 0;

    private int Score
    {
        get => _score;
        set
        {
            _score = value;
            var eventHandler = ScoreChanged;
            eventHandler?.Invoke(_score);
        }
    }

    public event Action<int> LevelChanged;
    public event Action<int> ScoreChanged;
    public event Action<float> TimeLeftChanged;

    void ReadDifficultySettings()
    {
        var path = Path.Combine(Application.streamingAssetsPath, ConfigName);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Could not find {ConfigName} at {path}. Creating a new one with defaults.");
            _difficultySettings = new DifficultySettings();
        }
        else
        {
            var jsonString = File.ReadAllText(path);
            _difficultySettings = JsonConvert.DeserializeObject<DifficultySettings>(jsonString);
        }
    }


    public override void Awake()
    {
        base.Awake();
        ReadDifficultySettings();
    }

    public void Start()
    {
        Level = 0;
        Score = 0;
        _windowManager.ShowStatsWindow();
        NextLevel();
    }

    private void Update()
    {
        // TODO: Testing
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("PlayerPrefs cleared");
        }
    }

    void NextLevel()
    {
        ++Level;
        foreach (var currentWire in _wireSpawner.CurrentWires)
            Destroy(currentWire.gameObject);
        _wireSpawner.CurrentWires.Clear();
        int wireCount = _difficultySettings.InitialWires + Mathf.FloorToInt( _difficultySettings.WiresPerLevel * (Level-1));
        _wireSpawner.SpawnWires(wireCount);
        float timeAmount = _difficultySettings.InitialTime + _difficultySettings.TimePerLevel * Level;
        StartTimer(timeAmount);
    }

    private Coroutine _timerCoroutine;

    private void StartTimer(float time)
    {
        if (_timerCoroutine != null)
            StopCoroutine(_timerCoroutine);
        _timerCoroutine = StartCoroutine(TimerCoroutine(time));
    }


    IEnumerator TimerCoroutine(float time)
    {
        while ((time -= Time.deltaTime) > 0)
        {
            var eventHandler = TimeLeftChanged;
            eventHandler?.Invoke(time);
            if (_wireSpawner.CurrentWires.TrueForAll(w => w.Connected))
            {
                Score += Mathf.RoundToInt(time) * Level;
                NextLevel();
                yield break;
            }
            yield return null;
        }
        _windowManager.ShowDefeatWindow(Score);
    }

    public void SaveRecord()
    {
        string playerName = _windowManager.GetPlayerName();
        if (string.IsNullOrEmpty(playerName))
            return;
        
        ScoreBoard.LoadScores();
        ScoreBoard.AddPlayerRecord(playerName, Score);
        var leaders = ScoreBoard.SaveScores().ToArray();
        string leadersText = "";
        for (int i = 0; i < leaders.Length; i++)
        {
            var leader = leaders[i];
            if (leader.Key == playerName)
                leadersText += $"<b><u>{i+1}. {leader.Key}: {leader.Value}</b></u>\n";
            else
                leadersText += $"{i + 1}. {leader.Key}: {leader.Value}\n";
        }
        _windowManager.ShowScoreboardWindow(leadersText);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    
    
}
