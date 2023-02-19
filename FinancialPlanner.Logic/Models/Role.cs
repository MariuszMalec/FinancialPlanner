using FinancialPlanner.Logic.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialPlanner.Logic.Models
{
    public class Role : Entity
    {
        public string Name { get; set; } = "User";
    }
}
