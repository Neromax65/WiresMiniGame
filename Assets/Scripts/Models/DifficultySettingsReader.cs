using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Models
{
    /// <summary>
    /// Класс чтения настроек сложности
    /// </summary>
    public static class DifficultySettingsReader
    {
        private const string ConfigName = "DifficultySettings.json";

        /// <summary>
        /// Прочитать JSON файл с настройками
        /// </summary>
        /// <returns>Модель конфигурации настроек сложности</returns>
        public static DifficultySettings Read()
        {
            var path = Path.Combine(Application.streamingAssetsPath, ConfigName);
            if (!File.Exists(path))
            {
                Debug.LogWarning($"Could not find {ConfigName} at {path}. Creating a new one with defaults.");
                return new DifficultySettings();
            }
            var jsonString = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<DifficultySettings>(jsonString);
        }
    }
}