using FinancialPlanner.Logic.Entities;
using FinancialPlanner.Logic.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FinancialPlanner.Logic.Dtos
{
    //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class TransactionDto : Entity
    {
        public Currency Currency { get; set; }

        public TypeOfTransaction Type { get; set; }

        [Required]
        [JsonProperty(PropertyName = "UserId")]
        public string UserId { get; set; }

        public CategoryOfTransaction Category { get; set; }

        [Required]
        [Range(0, double.PositiveInfinity)]
        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+(.\d{1,2})?$")]
        [JsonProperty(PropertyName = "Amount")]
        public decimal Amount { get; set; }

        [DataType(DataType.Currency)]
        [JsonProperty(PropertyName = "BalanceAfterTransaction")]
        public decimal BalanceAfterTransaction { get; set; }
        public string? Description { get; set; }

        public DateTime Date { get; set; }
    }
}