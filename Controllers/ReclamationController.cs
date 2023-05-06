using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HelpDesk4GL.Data; // Replace HelpDesk with your actual namespace
using HelpDesk4GL.Models; // Replace HelpDesk with your actual namespace
using HelpDesk4GL.ViewModels;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

[Authorize]
public class ReclamationController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;

    public ReclamationController(ApplicationDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;

    }
    public ReclamationsViewModel filtreReclamation(string? userId, string? etat, Boolean status, string message, User user)
    {
        List<Reclamation>? reclamations;
        Console.WriteLine("mouadh-Etat: " + etat);
        Console.WriteLine("mouadh-User: " + userId);


        if (user.role == "TECK" || user.role == "ADMIN")

        {
            Console.WriteLine("mouadh");

            if (userId != "-1" && etat != "-1" && userId != "" && etat != "" && userId != null && etat != null)
            {
                Console.WriteLine("mouadh 1.1");
                reclamations = _context.Reclamations.Where(item => item.UserId == userId)
                           .Where(x => x.etat == etat).ToList();
            }
            else if (userId != "-1" && userId != "" && userId != null)
            {
                Console.WriteLine("mouadh 1.2");
                reclamations = _context.Reclamations.Where(item => item.UserId == userId).ToList();
            }
            else if (etat != "-1" && etat != "" && etat != null)
            {
                Console.WriteLine("mouadh 1.3");
                reclamations = _context.Reclamations.Where(x => x.etat == etat).ToList();
            }
            else
            {
                Console.WriteLine("mouadh 1.4");
                reclamations = _context.Reclamations.ToList();
            }
        }
        else
        {
            if (etat != "-1" && etat != "" && etat != null)
            {
                Console.WriteLine("mouadh 0.1");
                reclamations = _context.Reclamations.Where(x => x.etat == etat)
                .Where(y => y.user == user).ToList();
            }
            else
            {
                Console.WriteLine("mouadh 0.2");
                reclamations = _context.Reclamations.Where(y => y.user == user).ToList();
            }
        }



        var viewModel = new ReclamationsViewModel
        {
            reclamations = reclamations,
            users = _context.Users.ToList(),
            Message = "",
            status = true,
            role = user.role
        };
        return viewModel;
    }

    // GET: /Reclamation
    [Authorize]
    public async Task<IActionResult> Index(string? userId, string? etat)
    {
        var user = await _userManager.GetUserAsync(User);

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
        var viewModel = filtreReclamation(userId, etat, status, message, user);

        // var viewModel = new ReclamationsViewModel
        // {
        //     reclamations = reclamations,
        //     users = users,
        //     Message = message,
        //     status = status
        // };
        TempData.Clear();
        return View("index", viewModel);
    }

    // GET: /Reclamation/Details/5
    [Authorize]
    public async Task<IActionResult> Details(int? id)
    {
        var user = await _userManager.GetUserAsync(User);

        if (id == null)
        {
            return NotFound();
        }

        var reclamation = _context.Reclamations.FirstOrDefault(r => r.Id == id);
        if (reclamation.user != user)
        {
            return RedirectToAction("Index", "Reclamation");
        }
        if (reclamation == null)
        {
            TempData["MessageDetail"] = "Reclamation ajouté avec succée.";
            return RedirectToAction("Index");
        }
        var actions = _context.Actions.Where(item => item.reclamation == reclamation).ToList();

        var message = "";
        if (TempData != null && TempData["MessageDetail"] != null && TempData["MessageDetail"].ToString() != "")
        {
            message = TempData["MessageDetail"].ToString();
        }
        var status = false;
        if (TempData != null && TempData["status"] != null && TempData["status"].ToString() != "")
        {
            status = (bool)TempData["status"];
        }

        var viewModel = new ReclamationsDetailViewModel
        {
            reclamation = reclamation,
            actions = actions,
            Message = message,
            status = status,
            role = user.role

        };
        TempData.Clear();

        return View("detail", viewModel);
    }

    // POST: /Reclamation/Create
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Filtre(string? userId, string? etat)
    {

        // List<Reclamation>? reclamations;

        // if (userId != "-1" && etat != "-1")
        // {
        //     reclamations = _context.Reclamations.Where(item => item.UserId == userId)
        //                .Where(x => x.etat == etat).ToList();
        // }
        // else if (userId != "-1")
        // {
        //     reclamations = _context.Reclamations.Where(item => item.UserId == userId).ToList();
        // }
        // else if (etat != "-1")
        // {
        //     reclamations = _context.Reclamations.Where(x => x.etat == etat).ToList();
        // }
        // else
        // {
        //     reclamations = _context.Reclamations.ToList();
        // }

        // var viewModel = new ReclamationsViewModel
        // {
        //     reclamations = reclamations,
        //     users = _context.Users.ToList(),
        //     Message = "",
        //     status = true
        // };
        // TempData["user"] = userId;
        // TempData["etat"] = etat;

        return RedirectToAction(nameof(Index), new { userId = userId, etat = etat });

    }

    // POST: /Reclamation/Create
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Reclamation reclamation)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            reclamation.createdAt = DateTime.UtcNow;
            reclamation.updatedAt = DateTime.UtcNow;
            reclamation.etat = "1";
            reclamation.user = user;
            reclamation.etatString = "Pas encore traitée";
            _context.Reclamations.Add(reclamation);
            _context.SaveChanges();

            int id = reclamation.Id;
            reclamation.code = "REC" + id;

            _context.SaveChanges();
            TempData["status"] = true;
            TempData["Message"] = "Reclamation ajouté avec succée.";
            return RedirectToAction(nameof(Index));
        }

        // return View(reclamation);
        TempData["status"] = false;
        TempData["Message"] = "Merci de verifier vos informations";
        return RedirectToAction("index");
    }


    // POST: /Reclamation/Edit/5
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int idrec, int etat, string? desciption, string? nature)
    {
        var reclamation = _context.Reclamations.FirstOrDefault(r => r.Id == idrec);
        if (reclamation == null)
        {
            TempData["status"] = false;
            TempData["Message"] = "Reclamation Introuvable !";
            return RedirectToAction("index");
        }
        if (int.Parse(reclamation.etat) == 3 || int.Parse(reclamation.etat) == 5)
        {
            TempData["status"] = false;
            TempData["MessageDetail"] = "Vous pouvez pas modifié cette reclamation.";
            return RedirectToAction("Details", new { id = idrec });
        }

        switch (etat)
        {
            case 1:
                reclamation.etat = "1";
                reclamation.etatString = "Pas encore traitée";
                break;
            case 2:
                reclamation.etat = "2";
                reclamation.etatString = "En cours";
                break;
            case 3:
                reclamation.etat = "3";
                reclamation.etatString = "Résolue";
                break;
            case 4:
                reclamation.etat = "4";
                reclamation.etatString = "Pas de solution";
                break;
            case 5:
                reclamation.etat = "5";
                reclamation.etatString = "Approuvé";
                break;
        }
        reclamation.updatedAt = DateTime.UtcNow;
        reclamation.desciption = desciption;
        reclamation.nature = nature;
        _context.Reclamations.Update(reclamation);
        _context.SaveChanges();
        TempData["status"] = true;
        TempData["MessageDetail"] = "Reclamation modifié avec succée.";
        return RedirectToAction("Details", new { id = idrec });

    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Etat(int id, int etat)
    {
        var reclamation = _context.Reclamations.FirstOrDefault(r => r.Id == id);
        if (reclamation == null)
        {
            TempData["status"] = false;
            TempData["Message"] = "Reclamation Introuvable !";
            return RedirectToAction("index");
        }
        if (int.Parse(reclamation.etat) == 3 || int.Parse(reclamation.etat) == 5)
        {
            TempData["status"] = false;

            if (etat == 5)
            {
                TempData["MessageDetail"] = "Vous pouvez pas modifié cette reclamation.";
                return RedirectToAction("Details", new { id = reclamation.Id });
            }
            else
            {
                TempData["Message"] = "Vous pouvez pas modifié cette reclamation.";
                return RedirectToAction("index");
            }

        }

        switch (etat)
        {
            case 1:
                reclamation.etat = "1";
                reclamation.etatString = "Pas encore traitée";
                break;
            case 2:
                reclamation.etat = "2";
                reclamation.etatString = "En cours";
                break;
            case 3:
                reclamation.etat = "3";
                reclamation.etatString = "Résolue";
                break;
            case 4:
                reclamation.etat = "4";
                reclamation.etatString = "Pas de solution";
                break;
            case 5:
                reclamation.etat = "5";
                reclamation.etatString = "Approuvé";
                break;
        }
        reclamation.updatedAt = DateTime.UtcNow;
        _context.Reclamations.Update(reclamation);
        _context.SaveChanges();
        TempData["status"] = true;
        if (etat == 5)
        {
            TempData["MessageDetail"] = "Reclamation modifié avec succée.";
            return RedirectToAction("Details", new { id = reclamation.Id });
        }
        else
        {
            TempData["Message"] = "Reclamation modifié avec succée.";
            return RedirectToAction("index");
        }
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddAction(int id, string description)
    {
        var reclamation = _context.Reclamations.FirstOrDefault(r => r.Id == id);
        TempData["status"] = false;
        if (reclamation == null)
        {

            TempData["Message"] = "Reclamation Introuvable !";
            return RedirectToAction("index");
        }
        if (int.Parse(reclamation.etat) == 3 || int.Parse(reclamation.etat) == 5)
        {
            TempData["MessageDetail"] = "Vous pouvez pas modifié cette reclamation.";
            return RedirectToAction("Details", new { id = id });
        }

        ActionReclamation action = new ActionReclamation();
        action.createdAt = DateTime.UtcNow;
        action.updatedAt = DateTime.UtcNow;
        action.desciption = description;
        action.reclamation = reclamation;
        _context.Actions.Add(action);
        _context.SaveChanges();
        reclamation.updatedAt = DateTime.UtcNow;
        _context.Reclamations.Update(reclamation);
        _context.SaveChanges();
        TempData["status"] = true;
        TempData["MessageDetail"] = "Action ajouté avec succée.";
        return RedirectToAction("Details", new { id = id });
    }
    // GET: /Reclamation/Delete/5
    [Authorize]
    public IActionResult Delete(int? id)
    {
        var reclamation = _context.Reclamations.FirstOrDefault(r => r.Id == id);
        TempData["status"] = false;
        if (reclamation == null)
        {

            TempData["Message"] = "Reclamation Introuvable !";
            return RedirectToAction("index");

        }
        if (int.Parse(reclamation.etat) != 1)
        {
            TempData["Message"] = "Vous pouvez pas supprimé cette reclamation.";
            return RedirectToAction("index");
        }

        _context.Reclamations.Remove(reclamation);
        _context.SaveChanges();
        TempData["status"] = true;
        TempData["Message"] = "Reclamation supprimé avec succée.";
        return RedirectToAction("index");

    }


    // GET: /Reclamation/DeleteAction/5
    [Authorize]
    public IActionResult DeleteAction(int? id)
    {
        var action = _context.Actions.Include(o => o.reclamation).FirstOrDefault(r => r.Id == id);
        int id_rec = action.reclamation.Id;
        _context.Actions.Remove(action);
        _context.SaveChanges();
        TempData["status"] = true;
        TempData["MessageDetail"] = "Reclamation supprimé avec succée.";
        return RedirectToAction("Details", new { id = id_rec });
    }

}
