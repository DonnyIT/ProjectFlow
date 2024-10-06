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

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
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

        var model = new Home
        {
            Projects = projects,
            Tasks = tasks
        };

        return View(model);
    }
}
