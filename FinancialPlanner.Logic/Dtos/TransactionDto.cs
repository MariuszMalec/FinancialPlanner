using FinancialPlanner.Logic.Entities;
using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        public string FullName
        {
            get
            {
                return GetFullName(UserId);
            }
            private set
            {
                FullName = GetFullName(UserId);
            }
        }

        private string GetFullName(string userId)//TODO jak to zrobic inaczej!!
        {
            string connString = "Server=localhost\\sqlexpress;Database=PlannerDb;Trusted_Connection=True;MultipleActiveResultSets=True;Encrypt=false;";
            string table = "Users";
            List<FullNameView> columns = new List<FullNameView>();
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = connString;
                conn.Open();
                SqlCommand command = new SqlCommand($"SELECT * FROM {table}", conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        columns.Add(new FullNameView() { Id = (string)reader[0] , FirstName = (string)reader[5], LastName = (string)reader[6] });          
                    }
                    Console.WriteLine("Data displayed! Now press enter to move to the next section!");
                }
            }
            if (columns.Count>0)
            {
                var fullName = columns.Where(u=>u.Id == userId).Select(u=>$"{u.FirstName} {u.LastName}").FirstOrDefault();
                return fullName;
            }

            return string.Empty;
        }
    }

    public class FullNameView
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}