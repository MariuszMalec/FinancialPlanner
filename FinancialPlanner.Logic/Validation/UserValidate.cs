using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Enums;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using System.Security.Cryptography.X509Certificates;

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
            if (user.Currency == null)
            {
                return "Currency can't by null";
            }
            if (user.Age == null)
            {
                return "Age can't by null";
            }
            if (users.FirstOrDefault(e => e.Email == user.Email) is not null)
            {
				return "User with this email exist yet";
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

        public static string Delete(User user, ApplicationDbContext context)
        {
            var roleId = context.Roles
                .Where(r => r.Name == EnumRole.SuperAdmin.ToString())
                .Select(r=>r.Id).FirstOrDefault();
            if (roleId == null) 
            {
                return "Role doesn't exist!";
            }
            var getIdRoleUser = context.Users.Where(u => u.Id == user.Id)
                .Select(u=>u.Role.Id).First();
            if (getIdRoleUser == roleId)
            {
                return "You can't delete superadmin!";
            }
            return string.Empty;
        }

        public static string ValidateEmail(string input)
        {
            if (input == null)
                return "Input is empty, retry!";

            else if (input.ToLower() == "exit")
                return "exit";

            else if (!input.Contains('@') || !input.Contains('.') || input.Length < 7)
                return "Email has to be in correct format";

            else if (input.LastIndexOf(".", StringComparison.Ordinal) > input.Length - 3)
                return "Email should be in correct format";

            return string.Empty;
        }
    }
}
