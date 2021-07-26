using System.Linq;
using Helpers;
using TMPro;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Класс, управляющий окнами и UI-элементами игры
    /// </summary>
    public class WindowManager : Singleton<WindowManager>
    {
        [SerializeField] private GameObject defeatWindow;
        [SerializeField] private GameObject leaderboardWindow;
        [SerializeField] private GameObject statsWindow;
        [SerializeField] private TMP_InputField nameInput;
        [SerializeField] private TMP_Text leaderboardText;
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

        /// <summary>
        /// Показать окно со статистикой уровня (номер уровня, таймер, очки)
        /// </summary>
        public void ShowStatsWindow()
        {
            defeatWindow.SetActive(false);
            leaderboardWindow.SetActive(false);
            statsWindow.SetActive(true);
        }
        
        /// <summary>
        /// Показать окно проигрыша
        /// </summary>
        /// <param name="totalScore">Очки игрока на момент проигрыша</param>
        public void ShowDefeatWindow(int totalScore)
        {
            totalScoreText.text = $"Your score: {totalScore}";
            statsWindow.SetActive(false);
            leaderboardWindow.SetActive(false);
            defeatWindow.SetActive(true);
        }

        /// <summary>
        /// Показать список лидеров
        /// </summary>
        /// <param name="score">Очки игрока</param>
        public void ShowLeaderboardWindow(int score)
        {
            string playerName = GetPlayerName();
            if (playerName == null) return;
        
            Leaderboard.LoadScores();
            Leaderboard.AddPlayerRecord(playerName, score);
            var leaders = Leaderboard.SaveScores().ToArray();
            string leadersText = "";
            for (int i = 0; i < leaders.Length; i++)
            {
                var leader = leaders[i];
                if (leader.Key == playerName)
                    leadersText += $"<b><u>{i+1}. {leader.Key}: {leader.Value}</b></u>\n";
                else
                    leadersText += $"{i + 1}. {leader.Key}: {leader.Value}\n";
            }
            leaderboardText.text = leadersText;
            statsWindow.SetActive(false);
            defeatWindow.SetActive(false);
            leaderboardWindow.SetActive(true);
        }

        /// <summary>
        /// Получить имя игрока из поля для ввода
        /// </summary>
        /// <returns></returns>
        private string GetPlayerName()
        {
            return string.IsNullOrEmpty(nameInput.text) ? null : nameInput.text;
        }

        /// <summary>
        /// Обновить текст уровня
        /// </summary>
        /// <param name="level">Уровень</param>
        private void UpdateLevelText(int level)
        {
            levelText.text = $"Level: {level}";
        }

        /// <summary>
        /// Обновить текст таймера
        /// </summary>
        /// <param name="timeLeft">Времени осталось</param>
        private void UpdateTimerText(float timeLeft)
        {
            timerText.text = $"Time left: {timeLeft:F1}";
        }

        /// <summary>
        /// Обновить текст очков
        /// </summary>
        /// <param name="score">Очки</param>
        private void UpdateScoreText(int score)
        {
            scoreText.text = $"Score: {score}";
        }
    
    }
}