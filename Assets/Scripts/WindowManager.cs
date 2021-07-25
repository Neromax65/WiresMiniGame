using Extensions;
using TMPro;
using UnityEngine;

public class WindowManager : Singleton<WindowManager>
{
    [SerializeField] private GameObject defeatWindow;
    [SerializeField] private GameObject scoreboardWindow;
    [SerializeField] private GameObject statsWindow;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_Text scoreboardText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text totalScoreText;


    public override void Awake()
    {
        GameManager.Instance.LevelChanged += UpdateLevelText;
        GameManager.Instance.ScoreChanged += UpdateScoreText;
        GameManager.Instance.TimeLeftChanged += UpdateTimerText;
    }

    public void ShowStatsWindow()
    {
        defeatWindow.SetActive(false);
        scoreboardWindow.SetActive(false);
        statsWindow.SetActive(true);
    }
        
    public void ShowDefeatWindow(int totalScore)
    {
        totalScoreText.text = $"Your score: {totalScore}";
        statsWindow.SetActive(false);
        scoreboardWindow.SetActive(false);
        defeatWindow.SetActive(true);
    }

    public void ShowScoreboardWindow(string leadersText)
    {
        scoreboardText.text = leadersText;
        statsWindow.SetActive(false);
        defeatWindow.SetActive(false);
        scoreboardWindow.SetActive(true);
    }

    public string GetPlayerName()
    {
        return string.IsNullOrEmpty(nameInput.text) ? null : nameInput.text;
    }

    private void UpdateLevelText(int level)
    {
        levelText.text = $"Level: {level}";
    }

    private void UpdateTimerText(float timeLeft)
    {
        timerText.text = $"Time left: {timeLeft:F1}";
    }

    private void UpdateScoreText(int score)
    {
        scoreText.text = $"Score: {score}";
    }
    
}