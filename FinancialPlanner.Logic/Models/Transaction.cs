using FinancialPlanner.Logic.Entities;
using FinancialPlanner.Logic.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanner.Logic.Models
{
    public class Transaction : Entity
    {
        public Currency Currency { get; set; }

        public TypeOfTransaction Type { get; set; }

        public CategoryOfTransaction Category { get; set; }

        [Required]
        [Range(0, double.PositiveInfinity)]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+(.\d{1,2})?$")]
        [JsonProperty(PropertyName = "Amount")]
        [Column(TypeName = "decimal(18,2)")]//TODO have to be without this error 3000!
        public decimal Amount { get; set; }

        [DataType(DataType.Currency)]
        [JsonProperty(PropertyName = "BalanceAfterTransaction")]
        [Column(TypeName = "decimal(18,2)")]//TODO have to be without this error 3000!
        public decimal BalanceAfterTransaction { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string? UserId { get; set; }

        public User? User { get; set;}
    }
}
