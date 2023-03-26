using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;

namespace FinancialPlanner.Logic.Validation
{
    public static class UserValidate
    {
        public static string Create(User user, ApplicationDbContext context)
        {
            var users = context.Users;
            var checkEmail = users.Any(x => x.Email == user.Email);
            if (checkEmail)
            {
                return "Email exist yet, fix to new one";
            }
            return string.Empty;
        }

        public static string Edit(User user, ApplicationDbContext context)
        {
            var users = context.Users;
            var checkEmails = users.Where(x => x.Email == user.Email).Count();
            if (checkEmails>1)
            {
                return "Email exist yet, fix to new one";
            }
            return string.Empty;
        }
    }
}
