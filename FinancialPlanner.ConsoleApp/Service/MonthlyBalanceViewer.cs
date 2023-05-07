using FinancialPlanner.Logic.Dtos;

namespace FinancialPlanner.ConsoleApp.Service
{
    public class MonthlyBalanceViewer
    {
        public static void Show(List<MonthlyIncomeAndExpenses> balance)
        {
            if (balance.Count() > 0)
            {
                var textPaddingWidth = 14;
                var paddingChar = ' ';
                var numberOfCollumn = 5;
                Console.WriteLine(("").PadRight(textPaddingWidth * numberOfCollumn, '='));
                Console.WriteLine($"|{"Month".PadRight(textPaddingWidth, paddingChar)}" +
                                  $"|{"Income".PadRight(textPaddingWidth, paddingChar)}" +
                                  $"|{"Expanses".PadRight(textPaddingWidth, paddingChar)}" +
                                  $"|{"Income-Outcome".PadRight(textPaddingWidth, paddingChar)}" +
                                  $"|{"Balance".PadRight(textPaddingWidth, paddingChar)}");
                Console.WriteLine(("").PadRight(textPaddingWidth * numberOfCollumn, '='));

                balance.ForEach(x => Console.WriteLine($"|{x.Month.ToString("MMMM").ToString().PadRight(textPaddingWidth, paddingChar)}" +
                                                       $"|{x.Income.ToString().PadRight(textPaddingWidth, paddingChar)}" +
                                                       $"|{x.Expenses.ToString().PadRight(textPaddingWidth, paddingChar)}" +
                                                       $"|{(x.Income - x.Expenses).ToString().PadRight(textPaddingWidth, paddingChar)}" +
                                                       $"|{x.Balance.ToString().PadRight(textPaddingWidth, paddingChar)}"));
            }
        }
    }
}