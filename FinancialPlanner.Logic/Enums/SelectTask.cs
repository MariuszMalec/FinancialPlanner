using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanner.Logic.Enums
{
    public enum SelectTask
    {
        Load_users_from_database,
        Load_users_from_json_file,
        View_user_from_json_file,
        Load_transactions_from_json_file,
        View_user,
        Add_new_user,
        Edit_user,
        Delete_user,
        View_users,
        MonthlyBalance,
        Exit
    }
}
