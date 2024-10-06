using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectFlow.Data;
using ProjectFlow.Models;

public class ProjectsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProjectsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Projects
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.Projects.Include(p => p.Owner);
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: Projects/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var project = await _context.Projects
            .Include(p => p.Owner)
            .FirstOrDefaultAsync(m => m.ProjectId == id);
        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }

    // GET: Projects/Create
    [Authorize(Roles = "Admin, Project Manager")]
    public IActionResult Create()
    {
        ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id");
        return View();
    }

    // POST: Projects/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, Project Manager")]
    public async Task<IActionResult> Create([Bind("ProjectId,Name,Description,CreatedAt,UpdatedAt,OwnerId")] Project project)
    {
        if (ModelState.IsValid)
        {
            _context.Add(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", project.OwnerId);
        return View(project);
    }

    // GET: Projects/Edit/5
    [Authorize(Roles = "Admin, Project Manager")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var project = await _context.Projects.FindAsync(id);
        if (project == null)
        {
            return NotFound();
        }
        ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", project.OwnerId);
        return View(project);
    }

    // POST: Projects/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, Project Manager")]
    public async Task<IActionResult> Edit(int id, [Bind("ProjectId,Name,Description,CreatedAt,UpdatedAt,OwnerId")] Project project)
    {
        if (id != project.ProjectId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(project);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(project.ProjectId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", project.OwnerId);
        return View(project);
    }

    // GET: Projects/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var project = await _context.Projects
            .Include(p => p.Owner)
            .FirstOrDefaultAsync(m => m.ProjectId == id);
        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }

    // POST: Projects/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
        {
            return NotFound();
        }

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProjectExists(int id)
    {
        return _context.Projects.Any(e => e.ProjectId == id);
    }
}
