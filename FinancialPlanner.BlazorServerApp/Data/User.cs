using System.ComponentModel.DataAnnotations;

namespace FinancialPlanner.BlazorServerApp.Data
{
    public class User
    {
        public string Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string LastName { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "Please provide Email")]
        [EmailAddress(ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }

        public string? Phone { get; set; }

    }
}
