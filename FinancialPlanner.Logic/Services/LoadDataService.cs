using FinancialPlanner.Logic.Models;
using Newtonsoft.Json;

namespace FinancialPlanner.Logic.Services
{
    public static class LoadDataService
    {
        public static List<User> ReadUserFile()
        {
            string fileName = @"Source/users.json";
            string getDir = Directory.GetCurrentDirectory();
            fileName = Path.Combine(getDir, fileName);
            Console.WriteLine($"{fileName}");
            if (File.Exists(fileName))
            {
                string jsonString = File.ReadAllText(fileName);
                List<User>? userData = JsonConvert.DeserializeObject<List<User>>(jsonString);
                return userData;
            }
            return new List<User>(){};
        }
    }
}
