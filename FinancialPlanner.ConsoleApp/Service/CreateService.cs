using FinancialPlanner.ConsoleApp.Validators;
using FinancialPlanner.Logic.Models;

namespace FinancialPlanner.ConsoleApp.Service
{
    public static class CreateService
    {
        internal static User Insert(int minNameLength, int maxNameLength, int minAge, int maxAge)
        {
            var newUser = new User() { };

            newUser.Id = Guid.NewGuid().ToString();

            var firstName = ValidateUser.GetNonDigString("FirstName", minNameLength, maxNameLength);
            if (firstName != null)
                newUser.FirstName = firstName;

            var lastName = ValidateUser.GetNonDigString("LastName", minNameLength, maxNameLength);
            if (lastName != null)
                newUser.LastName = lastName;

            var email = ValidateUser.GetEmail();
            if (email != null)
                newUser.Email = email;

            newUser.Gender = ValidateUser.GetGender();

            var balance = ValidateUser.GetDecimalInput("current balance");
            if (balance != -1)
                newUser.Balance = balance;

            var age = ValidateUser.GetIntInput("Age", minAge, maxAge);
            if (age != 0)
                newUser.Age = age;

            newUser.Company = null;
            newUser.IsActive = true;
            newUser.Currency = Logic.Enums.Currency.PLN;

            return newUser;
        }
    }
}
