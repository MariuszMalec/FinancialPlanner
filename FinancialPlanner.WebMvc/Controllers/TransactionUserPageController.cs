using AutoMapper;
using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FinancialPlanner.WebMvc.Controllers
{
    public class TransactionUserPageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IUserService _userService;

        public TransactionUserPageController(ApplicationDbContext context, IMapper mapper, ITransactionService transactionService, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _transactionService = transactionService;
            _userService = userService;
        }

        public async Task<IActionResult> Index(int? page, DateTime? selectMounth)
        {
            var transactions = await _transactionService.GetAllQueryable();

            var model = _mapper.Map<List<TransactionUserDto>>(transactions);

            model = model.OrderByDescending(t=>t.CreatedAt).ToList();

            //add pictures
            //model.ForEach(x => { if (x.Category == Logic.Enums.CategoryOfTransaction.Food) x.Picture = "https://images.unsplash.com/photo-1557821552-17105176677c?ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&w=1332&q=80"; });
            //model.ForEach(x => { if (x.Category == Logic.Enums.CategoryOfTransaction.Salary) x.Picture = "https://images.unsplash.com/photo-1553729459-efe14ef6055d?auto=format&fit=crop&q=80&w=2070&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"; });
            //model.ForEach(x => { if (x.Category == Logic.Enums.CategoryOfTransaction.Medicine) x.Picture = "https://images.unsplash.com/photo-1584308666744-24d5c474f2ae?auto=format&fit=crop&q=80&w=2030&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"; });
            //model.ForEach(x => { if (x.Category == Logic.Enums.CategoryOfTransaction.Credit) x.Picture = "https://images.unsplash.com/photo-1624811532681-e58a7e25f273?auto=format&fit=crop&q=80&w=2070&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"; });

            var pageNumber = page ?? 1;
            var perPage = 5;
            var onePageOfTransaction = await model.ToPagedListAsync(pageNumber, perPage);


            return onePageOfTransaction != null ?
                          View(onePageOfTransaction) :
                          NotFound("transaction is null!");
        }
    }
}
