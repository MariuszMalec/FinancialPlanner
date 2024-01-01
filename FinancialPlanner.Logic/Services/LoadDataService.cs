using Newtonsoft.Json;

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

        public static List<T> ReadTransacionFile()
        {
            string fileName = @"Source/transactions.json";
            string getDir = Directory.GetCurrentDirectory();
            fileName = Path.Combine(getDir, fileName);
            Console.WriteLine($"{fileName}");
            if (File.Exists(fileName))
            {
                string jsonString = File.ReadAllText(fileName);
                List<T>? data = JsonConvert.DeserializeObject<List<T>>(jsonString);
                return data;
            }
            return new List<T>(){};
        }

        public static List<T> ReadTransacionPictureFile()
        {
            string fileName = @"Source/transactionsPictures.json";
            string getDir = Directory.GetCurrentDirectory();
            fileName = Path.Combine(getDir, fileName);
            Console.WriteLine($"{fileName}");
            if (File.Exists(fileName))
            {
                string jsonString = File.ReadAllText(fileName);
                List<T>? data = JsonConvert.DeserializeObject<List<T>>(jsonString);
                return data;
            }
            return new List<T>() { };
        }

        public static List<T> ReadCategoryBudgetFile()
        {
            string fileName = @"Source/categoryBudget.json";
            string getDir = Directory.GetCurrentDirectory();
            fileName = Path.Combine(getDir, fileName);
            Console.WriteLine($"{fileName}");
            if (File.Exists(fileName))
            {
                string jsonString = File.ReadAllText(fileName);
                List<T>? data = JsonConvert.DeserializeObject<List<T>>(jsonString);
                return data;
            }
            return new List<T>() { };
        }
    }
}
