using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFlow.Data;
using ProjectFlow.Models;

public class ProjectsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ProjectsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
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
        var applicationDbContext = _context.Projects.Include(p => p.Owner);
        return View(await applicationDbContext.ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        await SetUserRolesInViewBag();
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
    public async Task<IActionResult> Create()
    {
        await SetUserRolesInViewBag();
        ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id");
        return View();
    }

    // POST: Projects/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin, Project Manager")]
    public async Task<IActionResult> Create([Bind("ProjectId,Name,Description,CreatedAt,UpdatedAt,OwnerId")] Project project)
    {
        await SetUserRolesInViewBag();
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
        await SetUserRolesInViewBag();
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
        await SetUserRolesInViewBag();
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
        await SetUserRolesInViewBag();
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
        await SetUserRolesInViewBag();
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
