using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineLibrary.Data;
using OnlineLibrary.Models;

namespace OnlineLibrary.Services;
public class UserService
{
    private ApplicationDbContext _context;
    private readonly UserManager<CustomUser> _userManager;
    private readonly RoleManager<CustomRole> _roleManager;
    public UserService(ApplicationDbContext context, UserManager<CustomUser> usermanager, RoleManager<CustomRole> roleManager)
    {
        _context = context;
        _userManager = usermanager;
        _roleManager = roleManager;
    }
    public async Task<List<CustomUser>> GetUserListAsync()
    {
        var customUser = _context.Users
                            .Where(u => !_context.UserRoles.Any(ur => ur.UserId == u.Id))
                            .AsQueryable();

        return await customUser.ToListAsync();
    }
    public async Task<CustomUser?> GetCustomUserByIdAsync(string id)
    {
        return await _context.Users.FindAsync(id) ?? null;
    }
    public async Task<CustomUser> UpdateUserRole(string id,CustomUser user)
    {
        // var user = await _context.Users.FindAsync(id);

        if (user == null)
            return null!;

        var memberRole = await _roleManager.FindByNameAsync("Member");

        if (memberRole != null)
        {
            _context.UserRoles.Add(new IdentityUserRole<string>
            {
                UserId = user.Id,
                RoleId = memberRole.Id
            });
            //await _userManager.AddToRoleAsync(user, memberRole.Name!);
            await _context.SaveChangesAsync();
        }

        return user!;
    }
}
