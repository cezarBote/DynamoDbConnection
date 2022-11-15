namespace CloudAWS.Models
{
    public class CustomerRequest
    {
        public Guid Id { get; set; }
        public string GitHubUsername { get; init; } = default!;

        public string FullName { get; init; } = default!;

        public string Email { get; init; } = default!;

        public DateTime DateOfBirth { get; init; } = default!;
    }
}
