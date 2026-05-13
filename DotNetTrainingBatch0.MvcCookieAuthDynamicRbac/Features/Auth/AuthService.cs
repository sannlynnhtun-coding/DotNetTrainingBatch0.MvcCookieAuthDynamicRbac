using DotNetTrainingBatch0.Database;
using DotNetTrainingBatch0.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

namespace DotNetTrainingBatch0.MvcCookieAuthDynamicRbac.Features.Auth;

public class AuthService
{
    private readonly AppDbContext _context;

    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(AppUser? User, string? RoleName)> LoginAsync(AuthRequest request)
    {
        var user = await _context.TblUsers.FirstOrDefaultAsync(x =>
            x.Username == request.Username &&
            x.Password == request.Password);

        if (user == null) return (null, null);

        var role = await _context.TblRoles.FirstOrDefaultAsync(r => r.Id == user.RoleId);
        
        return (user, role?.RoleName);
    }

    public AppDbContext GetContext() => _context;
}
