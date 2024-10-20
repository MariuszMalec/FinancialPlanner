using FinancialPlanner.Logic.Context;
using FinancialPlanner.Logic.Models;
using FinancialPlanner.Logic.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;

namespace FinancialPlanner.WebMvc.Controllers
{
    public class TransactionPictureController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ILogger _logger;
        private readonly IRepository<TransactionPicture> _repository;


        public TransactionPictureController(ApplicationDbContext context, ILogger logger, IRepository<TransactionPicture> repository)
        {
            _context = context;
            _logger = logger;
            _repository = repository;
        }
        public async Task<IActionResult> Index()
        {
            var models = await _repository.GetAll();
            if (!models.Any())
            {
                return Content("Pictures not found!");
            }
            return View(models);
        }

        public async Task<IActionResult> Details(string id)
        {
            var model = await _repository.GetById(id);
            if (model == null)
            {
                return NotFound($"no picture {id}");
            }
            return View(model);
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TransactionPicture model)
        {
            if (model == null)
            {
                _logger.Error("Not created picture");
                return Content("Not created picture");
            }
            await _repository.Insert(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || await _repository.GetById(id) == null)
            {
                return NotFound();
            }

            var model = await _repository.GetById(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, TransactionPicture model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            try
            {
                await _repository.Update(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return RedirectToAction(nameof(Index));
        }


        public async Task<ActionResult> Delete(string id)
        {
            var model = await _repository.GetById(id);
            if (model == null)
            {
                _logger.Error($"Not found picture with {id}!");
                return NotFound($"Not found picture with {id}");
            }
            return View(model);
        }

        // POST: RoleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(TransactionPicture model)
        {
            try
            {
                await _repository.Delete(model);
                _logger.Warning($"delete picture was deleted successful");
                return RedirectToAction("Index");
            }
            catch
            {
                _logger.Warning($"delete picture was not successful");
                return View();
            }
        }

    }
}
