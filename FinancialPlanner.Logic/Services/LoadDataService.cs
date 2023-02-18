using FinancialPlanner.Logic.Models;
using Newtonsoft.Json;

namespace FinancialPlanner.Logic.Services
{
    public static class LoadDataService
    {
        public static List<User> ReadUserFile()
        {
            string fileName = @"Source\users.json";
            string jsonString = File.ReadAllText(fileName);
            List<User> userData = JsonConvert.DeserializeObject<List<User>>(jsonString);
            return userData;
        }
    }
}
