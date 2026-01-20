using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IndexModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public IList<IdentityUser> Users { get; set; }
    public Dictionary<string, List<string>> UserRoles { get; set; } = new();

    public async Task OnGetAsync()
    {
        Users = _userManager.Users.ToList();
        UserRoles = new Dictionary<string, List<string>>();

        foreach (var user in Users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            UserRoles[user.Id] = roles.ToList();
        }
    }
}