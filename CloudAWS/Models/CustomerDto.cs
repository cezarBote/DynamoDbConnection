using System.Text.Json.Serialization;

namespace CloudAWS.Models
{
    public class CustomerDto
    {
        [JsonPropertyName("pk")]
        public string Pk { get; set; } = default!;

        [JsonPropertyName("sk")]
        public string Sk { get; set; } = default!;

        [JsonPropertyName("id")]
        public string Id { get; set; } = default!;

        [JsonPropertyName("gitHubUsername")]
        public string GitHubUsername { get; init; } = default!;

        [JsonPropertyName("fullName")]
        public string FullName { get; init; } = default!;

        [JsonPropertyName("email")]
        public string Email { get; init; } = default!;

        [JsonPropertyName("dateOfBirth")]
        public DateTime DateOfBirth { get; init; }
    }
}
