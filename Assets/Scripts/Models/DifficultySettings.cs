using Newtonsoft.Json;

namespace Models
{
    [JsonObject]
    public class DifficultySettings
    {
        [JsonProperty] public int InitialWires { get; set; } = 2;
        [JsonProperty] public float WiresPerLevel { get; set; } = 0.5f;
        [JsonProperty] public float InitialTime { get; set; } = 15;
        [JsonProperty] public float TimePerLevel { get; set; } = -1f;
    }
}