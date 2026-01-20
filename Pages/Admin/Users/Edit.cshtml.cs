using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")]
public class EditModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public EditModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [BindProperty]
    public EditInputModel Input { get; set; }

    public class EditInputModel
    {
        public string Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public bool IsManager { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();
        var isManager = await _userManager.IsInRoleAsync(user, "Manager");
        Input = new EditInputModel { Id = user.Id, Email = user.Email, IsManager = isManager };
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();
        var user = await _userManager.FindByIdAsync(Input.Id);
        if (user == null) return NotFound();
        user.Email = Input.Email;
        user.UserName = Input.Email;
        await _userManager.UpdateAsync(user);

        var isManager = await _userManager.IsInRoleAsync(user, "Manager");
        if (Input.IsManager && !isManager)
            await _userManager.AddToRoleAsync(user, "Manager");
        else if (!Input.IsManager && isManager)
            await _userManager.RemoveFromRoleAsync(user, "Manager");

        TempData["StatusMessage"] = "User updated.";
        return RedirectToPage("Index");
    }
}