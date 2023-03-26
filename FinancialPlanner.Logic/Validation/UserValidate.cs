using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;

namespace FinancialPlanner.Logic.Validation
{
    public static class UserValidate
    {
        public static string Check(User user, IUserService userService)
        {
            var users = userService.GetAll().Result;
            var checkEmail = users.Any(x => x.Email == user.Email);
            if (checkEmail)
            {
                return "Email exist yet, fix to new one";
            }

            return string.Empty;
        }
    }
}
