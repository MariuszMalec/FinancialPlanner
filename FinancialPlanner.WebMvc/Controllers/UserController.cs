﻿using FinancialPlanner.Logic.Dtos;
using FinancialPlanner.Logic.Entities;
using FinancialPlanner.Logic.Interfaces;
using FinancialPlanner.Logic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialPlanner.WebMvc.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: UserController
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAll();
            if (!users.Any())
            {
                return View("No users!");
            }

            //var model = _mapper.Map<List<UserView>>(users);

            var model = users.Select(x=> new UserDto() 
            { 
                Id = x.Id , 
                CreatedAt = x.Registered, 
                Company = x.Company,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                IsActive = x.IsActive
            });

            return View(model);
        }

        // GET: UserController/Details/5
        public async Task<ActionResult> Details(string id)
        {
            var user = await _userService.GetById(id);

            if (user == null)
            {
                return BadRequest($"Brak uzytkownika {id}");
            }

            var model = new UserDto()
            {

            };

            return View(user);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(User model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                if (model == null)
                    return NotFound("404 Brak roli!");

                await _userService.Insert(model);
                if (model == null)
                {
                    return NotFound("404 user not created!");
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var model = await _userService.GetById(id);
            if (model == null)
            {
                return NotFound($"Not found user with {id}");
            }
            return View(model);
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string id, User model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                await _userService.Update(model);

                if (model.Id == null)
                {
                    return NotFound("No user!");
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public async Task<ActionResult<User>> Delete(string id)
        {
            var model = await _userService.GetById(id);
            if (model == null)
            {
                return NotFound($"Not found user with {id}");
            }
            return View(model);
        }

        // POST: RoleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id, User model)
        {
            try
            {
                await _userService.Delete(model);
                if (model.Id == null)
                {
                    return RedirectToAction("EmptyList");
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
