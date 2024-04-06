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
    public UserService(ApplicationDbContext context, UserManager<CustomUser> usermanager)
    {
        _context = context;
        _userManager = usermanager;
    }
    public async Task<List<CustomUser>> GetUserListAsync()
    {
        //          var usersNoAdmin =   _context.CustomUser
        //     .Where(w=> w.isAdmin == false);

        // return await usersNoAdmin.ToListAsync();
         var customUser =  _context.Users;
        //   Console.WriteLine(customUser);

        // var users = _context.Users.AsQueryable();
        return await customUser.ToListAsync();
            // return await _context.Users
            // //.Where(u => !_context.UserRoles.Any(ur => ur.UserId == u.Id))
            // .AsQueryable();
    }
}
