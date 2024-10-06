using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectFlow.Data;
using ProjectFlow.Models;

public class TasksController : Controller
{
    private readonly ApplicationDbContext _context;

    public TasksController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Tasks
    [Authorize(Roles = "Admin, Project Manager, Team Member")]
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.Tasks.Include(t => t.AssignedUser).Include(t => t.Project);
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: Tasks/Details/5
    [Authorize(Roles = "Admin, Project Manager, Team Member")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var task = await _context.Tasks
            .Include(t => t.AssignedUser)
            .Include(t => t.Project)
            .FirstOrDefaultAsync(m => m.TaskId == id);
        if (task == null)
        {
            return NotFound();
        }

        return View(task);
    }

    // GET: Tasks/Create
    [Authorize(Roles = "Admin, Project Manager")]
    public IActionResult Create()
    {
        ViewData["AssignedUserId"] = new SelectList(_context.Users, "Id", "Id");
        ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Name");
        return View();
    }

    // POST: Tasks/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, Project Manager")]
    public async Task<IActionResult> Create([Bind("TaskId,Title,Description,DueDate,IsCompleted,UpdatedAt,ProjectId,AssignedUserId")] ProjectFlow.Models.Task task)
    {
        if (ModelState.IsValid)
        {
            _context.Add(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["AssignedUserId"] = new SelectList(_context.Users, "Id", "Id", task.AssignedUserId);
        ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Name", task.ProjectId);
        return View(task);
    }

    // GET: Tasks/Edit/5
    [Authorize(Roles = "Admin, Project Manager")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        ViewData["AssignedUserId"] = new SelectList(_context.Users, "Id", "Id", task.AssignedUserId);
        ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Name", task.ProjectId);
        return View(task);
    }

    // POST: Tasks/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, Project Manager")]
    public async Task<IActionResult> Edit(int id, [Bind("TaskId,Title,Description,DueDate,IsCompleted,UpdatedAt,ProjectId,AssignedUserId")] ProjectFlow.Models.Task task)
    {
        if (id != task.TaskId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(task);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(task.TaskId))
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
        ViewData["AssignedUserId"] = new SelectList(_context.Users, "Id", "Id", task.AssignedUserId);
        ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Name", task.ProjectId);
        return View(task);
    }

    // GET: Tasks/Delete/5
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var task = await _context.Tasks
            .Include(t => t.AssignedUser)
            .Include(t => t.Project)
            .FirstOrDefaultAsync(m => m.TaskId == id);
        if (task == null)
        {
            return NotFound();
        }

        return View(task);
    }

    // POST: Tasks/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TaskExists(int id)
    {
        return _context.Tasks.Any(e => e.TaskId == id);
    }
}
