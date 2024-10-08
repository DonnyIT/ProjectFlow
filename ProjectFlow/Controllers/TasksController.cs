using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFlow.Data;
using ProjectFlow.Models;

public class TasksController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public TasksController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    private async System.Threading.Tasks.Task SetUserRolesInViewBag()
    {
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
    }

    public async Task<IActionResult> Index()
    {
        await SetUserRolesInViewBag();
        var tasks = await _context.Tasks.Include(t => t.AssignedUser).Include(t => t.Project).ToListAsync();
        return View(tasks);
    }

    // GET: Tasks/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        await SetUserRolesInViewBag();
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
    public async Task<IActionResult> Create()
    {
        await SetUserRolesInViewBag();
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
        await SetUserRolesInViewBag();
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
        await SetUserRolesInViewBag();
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
        await SetUserRolesInViewBag();
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
        await SetUserRolesInViewBag();
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
        await SetUserRolesInViewBag();
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
