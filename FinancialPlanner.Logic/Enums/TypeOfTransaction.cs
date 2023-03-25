using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FinancialPlanner.Logic.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TypeOfTransaction
    {
        All = 0,
        Income,
        Outcome,
    }
}
