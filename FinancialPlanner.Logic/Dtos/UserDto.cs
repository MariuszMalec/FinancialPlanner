using FinancialPlanner.Logic.Entities;
using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanner.Logic.Dtos
{
    public class UserDto : Entity
    {
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Please enter balance")]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+(.\d{1,2})?$", ErrorMessage = "Provide valid balance")]
        [Column(TypeName = "decimal(18,2)")]//TODO have to be without this error 3000!
        public decimal Balance { get; set; }
        
        public Currency Currency { get; set; }

        //[Required(ErrorMessage = "Please enter age!")]
        //public int? Age { get; set; }

        [Required(ErrorMessage = "Please enter first name")]
        [StringLength(25)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please provide last name")]
        [StringLength(25)]
        public string LastName { get; set; }
        //[Required]
        //public Gender Gender { get; set; }
        public string? Company { get; set; }

        [EmailAddress(ErrorMessage = "Email is not valid.")]

        //public string? Phone { get; set; }
        //public string? Address { get; set; }
        //public virtual Role Role { get; set; }
        //public string PasswordHash { get; set; }
    }
}
