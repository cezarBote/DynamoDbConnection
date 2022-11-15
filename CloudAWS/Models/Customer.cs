using System.ComponentModel.DataAnnotations;

namespace CloudAWS.Models
{
    public class Customer
    {
        public Guid Id { get; set; }

        public string GitHubUsername { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime DateOfBirth { get; set; }
    }
}
