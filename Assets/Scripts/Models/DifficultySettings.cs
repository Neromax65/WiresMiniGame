using Newtonsoft.Json;

namespace Models
{
    /// <summary>
    /// Модель конфигурационного файла настройки сложности
    /// </summary>
    [JsonObject]
    public class DifficultySettings
    {
        /// <summary>
        /// Изначальное количество проводов на каждой стороне
        /// </summary>
        [JsonProperty] public int InitialWires { get; set; } = 2;
        
        /// <summary>
        /// Увеличение количества проводов с уровнем. Можно задавать дробные значения, например 0.5 для увеличение каждые
        /// 2 уровня, или 0.2 для увеличения каждые 5 уровней и т.д.
        /// </summary>
        [JsonProperty] public float WiresPerLevel { get; set; } = 0.5f;
        
        /// <summary>
        /// Изначальное количество времени в секундах для прохождения уровня
        /// </summary>
        [JsonProperty] public float InitialTime { get; set; } = 15;
        
        /// <summary>
        /// Изменение времени с уровнем, можно задавать как положительные, так и отрицательные значения
        /// </summary>
        [JsonProperty] public float TimePerLevel { get; set; } = -1f;
    }
}