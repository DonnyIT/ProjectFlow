using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectFlow.Data;
using ProjectFlow.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var today = DateTime.Today;
        var projects = await _context.Projects
            .Include(p => p.Tasks)
            .Where(p => p.Tasks.Any(t => t.DueDate == today && !t.IsCompleted))
            .ToListAsync();

        var tasks = await _context.Tasks
            .Where(t => t.DueDate == today && !t.IsCompleted)
            .Include(t => t.Project)
            .ToListAsync();

        var users = await _userManager.Users.ToListAsync();
        var userRoles = new List<UserRole>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            userRoles.Add(new UserRole
            {
                UserName = user.UserName,
                Roles = roles
            });
        }

        ViewBag.UserRoles = userRoles;

        var model = new Home
        {
            Projects = projects,
            Tasks = tasks,
        };

        return View(model);
    }
}
