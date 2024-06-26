﻿using FinalProject.DAL;
using FinalProject.Models;
using FinalProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _appDbContext;

        public UserController(UserManager<AppUser> userManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        public async Task <IActionResult> Index()
        {
            AppUser? user;
            List<UserSong> songs = new();
            if (User.Identity != null && User.Identity.IsAuthenticated && User.Identity.Name != null)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null)
                {
                    return NotFound();
                }
                songs = await _appDbContext.UserSongs.Where(s => s.User.Id == user.Id).Include(s => s.Song).ToListAsync();

            }
          
           return View(songs);
        }

        public async Task<IActionResult> RemoveSong(int? id)
        {
            if (User.Identity != null && !User.Identity.IsAuthenticated && User.Identity.Name == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (id==null)
            {
                return NotFound();
            }
            UserSong? userSong = await _appDbContext.UserSongs.FirstOrDefaultAsync(s => s.SongId == id);
            if (userSong == null)
            {
                return NotFound();
            }
            _appDbContext.UserSongs.Remove(userSong);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }


    }
}
