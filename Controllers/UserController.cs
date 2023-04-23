using Microsoft.AspNetCore.Mvc;
using HelpDesk4GL.Models;
using HelpDesk4GL.Data;
using HelpDesk4GL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
[Authorize]

public class UsersController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public UsersController(ApplicationDbContext context,UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;

    }

    // GET: /Users
    [Authorize]
    public IActionResult Index()
    {
        var users = _context.Users.ToList();
        var message = "";
        if (TempData != null && TempData["Message"] != null && TempData["Message"].ToString() != "")
        {
            message = TempData["Message"].ToString();
        }
        var status = false;
        if (TempData != null && TempData["status"] != null && TempData["status"].ToString() != "")
        {
            status = (bool)TempData["status"];
        }
        var viewModel = new UserViewModel
        {
            users = users,
            Message = message,
            status = status
        };
        // Console.WriteLine(reclamations[0].Id);
        return View("index", viewModel);
    }

    // GET: /Users/Details/5
    [Authorize]
    public IActionResult Details(String? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }

    // POST: /Users/Create
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserRequest userRequest)
    {
        if (ModelState.IsValid)
        {   
            User user = new User();
            user.createdAt = DateTime.UtcNow;
            user.updatedAt = DateTime.UtcNow;
            user.role=userRequest.role;
            user.Email=userRequest.Email;
            user.UserName=userRequest.Email;
            user.NormalizedUserName=userRequest.Email;
            user.fullName=userRequest.fullName;
            var result = await _userManager.CreateAsync(user, userRequest.password);
            Console.WriteLine("res="+result);
            // _context.Users.Add(user);
            // _context.SaveChanges();
            TempData["status"] = true;
            TempData["Message"] = "Utilisateur ajouté avec succée.";
            return RedirectToAction(nameof(Index));
        }
        return RedirectToAction("index");
    }

    // POST: /Users/Edit/5
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, UserRequest user)
    {

        var getUser = _context.Users.FirstOrDefault(r => r.Id == id);

        if (ModelState.IsValid)
        {
            getUser.updatedAt = DateTime.UtcNow;
            getUser.fullName = user.fullName;
            getUser.Email = user.Email;
            getUser.role = user.role;
            _context.Users.Update(getUser);
            _context.SaveChanges();
            TempData["status"] = true;
            TempData["Message"] = "Utilisateur modifié avec succée.";
            return RedirectToAction(nameof(Index));
        }

        TempData["status"] = false;
        TempData["Message"] = "Modifié echoué.";
        return RedirectToAction("index");
    }

    // GET: /Users/Delete/5
    [Authorize]
    public IActionResult Delete(string? id)
    {

        var user = _context.Users.FirstOrDefault(r => r.Id == id);
        TempData["status"] = false;
        if (user == null)
        {

            TempData["Message"] = "Utilisateur Introuvable !";
            return RedirectToAction("index");

        }

        _context.Users.Remove(user);
        _context.SaveChanges();
        TempData["status"] = true;
        TempData["Message"] = "Utilisateur supprimé avec succée.";
        return RedirectToAction("index");
    }

}