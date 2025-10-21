using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")]
public class DeleteModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    public DeleteModel(UserManager<IdentityUser> userManager) => _userManager = userManager;

    [BindProperty]
    public string Id { get; set; } = default!;
    public IdentityUser? UserToDelete { get; set; }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        UserToDelete = await _userManager.FindByIdAsync(id);
        if (UserToDelete == null) return NotFound();
        Id = id;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.FindByIdAsync(Id);
        if (user == null) return NotFound();
        await _userManager.DeleteAsync(user);
        TempData["StatusMessage"] = "User deleted.";
        return RedirectToPage("Index");
    }
}