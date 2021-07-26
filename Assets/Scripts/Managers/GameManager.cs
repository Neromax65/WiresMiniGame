using System;
using System.Collections;
using Helpers;
using Models;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Класс, отвечающий за логику игрового процесса
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
    
        /// <summary>
        /// Событие изменения уровня
        /// </summary>
        public event Action<int> LevelChanged;
        
        /// <summary>
        /// Событие изменения очков
        /// </summary>
        public event Action<int> ScoreChanged;
        
        /// <summary>
        /// Событие изменения времени таймера
        /// </summary>
        public event Action<float> TimeLeftChanged;
    
        private int _level;
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
        private int _score;
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
        
        private DifficultySettings _difficultySettings;
    
        private Coroutine _timerCoroutine;

        public override void Awake()
        {
            base.Awake();
            _difficultySettings = DifficultySettingsReader.Read();
        }

        public void Start()
        {
            Level = 0;
            Score = 0;
            WindowManager.Instance.ShowStatsWindow();
            NextLevel();
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Backslash))
            {
                PlayerPrefs.DeleteAll();
                Debug.Log("PlayerPrefs cleared");
            }
#endif
        }

        /// <summary>
        /// Переход к следующему уровню
        /// </summary>
        void NextLevel()
        {
            ++Level;
            int wireCount = _difficultySettings.InitialWires + Mathf.FloorToInt( _difficultySettings.WiresPerLevel * (Level-1));
            WireManager.Instance.SpawnWires(wireCount);
            float timeAmount = _difficultySettings.InitialTime + _difficultySettings.TimePerLevel * Level;
            StartTimer(timeAmount);
        }


        /// <summary>
        /// Запуск таймера уровня
        /// </summary>
        /// <param name="time">Изначальное время таймера</param>
        private void StartTimer(float time)
        {
            if (_timerCoroutine != null)
                StopCoroutine(_timerCoroutine);
            _timerCoroutine = StartCoroutine(TimerCoroutine(time));
        }


        /// <summary>
        /// Корутина таймера
        /// </summary>
        /// <param name="time">Изначальное время таймера</param>
        /// <returns></returns>
        private IEnumerator TimerCoroutine(float time)
        {
            while ((time -= Time.deltaTime) > 0)
            {
                var eventHandler = TimeLeftChanged;
                eventHandler?.Invoke(time);
                if (WireManager.Instance.AllWiresConnected)
                {
                    Score += Mathf.RoundToInt(time) * Level;
                    NextLevel();
                    yield break;
                }
                yield return null;
            }
            WindowManager.Instance.ShowDefeatWindow(Score);
        }

        /// <summary>
        /// Сохранение рекорда игрока
        /// </summary>
        public void SaveRecord()
        {
            WindowManager.Instance.ShowLeaderboardWindow(Score);
        }

        /// <summary>
        /// Выход из игры
        /// </summary>
        public void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    
    
    }
}
