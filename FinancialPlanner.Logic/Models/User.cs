using FinancialPlanner.Logic.Entities;
using FinancialPlanner.Logic.Enums;
using System.ComponentModel.DataAnnotations;

namespace FinancialPlanner.Logic.Models
{
    public class User : Entity
    {
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Please enter balance")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+(.\d{1,2})?$", ErrorMessage = "Provide valid balance")]
        public decimal Balance { get; set; }
        public Currency Currency { get; set; }

        [Required(ErrorMessage = "Please enter age!")]
        public int? Age { get; set; }

        [Required(ErrorMessage = "Please enter first name")]
        [StringLength(25)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please provide last name")]
        [StringLength(25)]
        public string LastName { get; set; }
        [Required]
        public Gender Gender { get; set; }
        public string? Company { get; set; }

        [EmailAddress(ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public virtual Role Role { get; set; }// = new Role() { Id = Guid.NewGuid().ToString(), Name = "User" };
        public string? PasswordHash { get; set; }
        public DateTime Registered { get; set; }

    }
}
