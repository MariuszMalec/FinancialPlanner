using FinancialPlanner.Logic.Interfaces;
using Newtonsoft.Json;

namespace FinancialPlanner.Logic.Services
{
    public class ReadTransactions<T> : ILoadData<T>
    {
        public IList<T> GetAll(string file)
        {
            string fileName = file;
            string getDir = Directory.GetCurrentDirectory();
            fileName = Path.Combine(getDir, fileName);
            Console.WriteLine($"{fileName}");
            if (File.Exists(fileName))
            {
                string jsonString = File.ReadAllText(fileName);
                IList<T>? data = JsonConvert.DeserializeObject<List<T>>(jsonString);
                return data;
            }
            return new List<T>() { };
        }
    }
}
