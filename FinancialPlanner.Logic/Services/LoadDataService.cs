using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace FinancialPlanner.Logic.Services
{
    public static class LoadDataService<T>
    {
        public static List<T> ReadUserFile()
        {
            string fileName = @"Source/users.json";
            string getDir = Directory.GetCurrentDirectory();
            fileName = Path.Combine(getDir, fileName);
            Console.WriteLine($"{fileName}");
            if (File.Exists(fileName))
            {
                string jsonString = File.ReadAllText(fileName);
                List<T>? userData = JsonConvert.DeserializeObject<List<T>>(jsonString);
                return userData;
            }
            return new List<T>(){};
        }

        public static IList<T> ReadTransacionFile()
        {
            string fileName = @"Source/transactions.json";
            string getDir = Directory.GetCurrentDirectory();
            fileName = Path.Combine(getDir, fileName);
            Console.WriteLine($"{fileName}");
            if (File.Exists(fileName))
            {
                string jsonString = File.ReadAllText(fileName);
                IList<T>? data = JsonConvert.DeserializeObject<List<T>>(jsonString);
                return data;
            }
            return new List<T>(){};
        }
    }
}
