using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using DotNetTrainingBatch0.Database;
using Microsoft.EntityFrameworkCore;

namespace DotNetTrainingBatch0.MvcCookieAuthDynamicRbac.Filters;

public class PermissionAttribute : TypeFilterAttribute
{
    public PermissionAttribute(string permission) : base(typeof(PermissionFilter))
    {
        Arguments = new object[] { permission };
    }
}

public class PermissionFilter : IAsyncAuthorizationFilter
{
    private readonly string _permission;
    private readonly AppDbContext _context;

    public PermissionFilter(string permission, AppDbContext context)
    {
        _permission = permission;
        _context = context;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new ChallengeResult();
            return;
        }

        var userIdString = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(userIdString, out int userId))
        {
            var user = await _context.TblUsers.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                var hasPermission = await (from rp in _context.TblRolePermissions
                                       join p in _context.TblPermissions on rp.PermissionId equals p.Id
                                       where rp.RoleId == user.RoleId && p.PermissionName == _permission
                                       select p.Id).AnyAsync();

                if (hasPermission) return;
            }
        }

        context.Result = new ForbidResult();
    }
}
