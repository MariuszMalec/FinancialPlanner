using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Validation;

namespace FinancialPlanner.ConsoleApp.Service
{
    public static class EditService
    {
        public static User EditUser(User newUser, int minNameLength, int maxNameLength, int minAge, int maxAge)
        {
            //readline here
            var firstName = GetNonDigString("FirstName", minNameLength, maxNameLength);
            if (firstName != null)
                newUser.FirstName = firstName;

            var lastName = GetNonDigString("LastName", minNameLength, maxNameLength);
            if (lastName != null)
                newUser.LastName = lastName;

            var email = GetEmail();
            if (email != null)
                newUser.Email = email;

            newUser.Gender = GetGender();

            var balance = GetDecimalInput("current balance");
            if (balance != -1)
                newUser.Balance = balance;

            var age = GetIntInput("Age", minAge, maxAge);
            if (age != 0)
                newUser.Age = age;

            return newUser;
        }

        public static string GetNonDigString(string name, int minLength, int maxNameLength)
        {
            while (true)
            {
                var input = string.Empty;
                if (minLength == 0)
                {
                    Console.Write($"{name}(press enter if null): ");
                    return Console.ReadLine()?.Trim();
                }

                Console.WriteLine($"write exit or press enter if you want leave");
                Console.Write($"{name}: ");
                input = Console.ReadLine()?.Trim();

                if (input.ToLower() == "exit" || input == "")
                    return null;

                if (input == null || input.Length < minLength || input.Length > maxNameLength || input.Any(char.IsWhiteSpace))
                    Console.WriteLine($"Invalid data. {name} should have at least {minLength} char long and in correct format Retry!");
                else
                    return input;
            }
        }

        private static string GetEmail()
        {
            while (true)
            {
                Console.Write("email: ");
                var input = Console.ReadLine()?.Trim();

                if (input.ToLower() == "exit" || input == "")
                    return null;

                var message = UserValidate.ValidateEmail(input);

                if (string.IsNullOrEmpty(message))
                    return input;

                Console.WriteLine(message);
            }
        }

        private static Gender GetGender()
        {
            var genderArray = Enum.GetNames(typeof(Gender));

            Console.WriteLine("Choose your gender:");
            for (int i = 0; i < genderArray.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {genderArray[i]}");
            }
            while (true)
            {
                var input = Console.ReadKey();

                Console.WriteLine();
                if (!char.IsDigit(input.KeyChar))
                {
                    Console.WriteLine("Wrong value, try again!\n");
                    continue;
                }

                var isParsed = int.TryParse(input.KeyChar.ToString(), out var selection);

                if (isParsed && selection <= genderArray.Length)
                    return (Gender)selection - 1;

                Console.WriteLine("Wrong selection, try Again!");
            }
        }

        private static decimal GetDecimalInput(string name)
        {
            while (true)
            {
                Console.WriteLine($"write exit or press enter if you want leave");
                Console.Write($"{name}: ");
                var input = Console.ReadLine().Replace(',', '.');
                if (input.ToLower() == "exit" || input == "")
                    return -1;
                var isDig = decimal.TryParse(input, out var result);
                if (isDig && result >= 0)
                    return result;

                Console.WriteLine($"{name} should be no less than 0");
            }
        }

        private static int GetIntInput(string name, int min, int max)
        {
            while (true)
            {
                Console.Write($"{name}: ");
                var input = Console.ReadLine();
                if (input.ToLower() == "exit" || input == "")
                    return 0;
                var isDig = int.TryParse(input, out var result);
                if (isDig && result >= min && result <= max)
                    return result;

                Console.WriteLine($"{name} should be between {min} and {max}");
            }
        }
    }
}
