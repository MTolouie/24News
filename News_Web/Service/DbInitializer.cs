using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using News_Common;
using News_DataLayer.Data;
using News_DataLayer.Models;
using News_Web.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News_Web.Service
{
    public static class DbInitializer
    {
        //private UserManager<IdentityUser> _userManager;
        //private RoleManager<IdentityRole> _roleManager;

        //public DbInitializer(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        //{
        //    _userManager = userManager;
        //    _roleManager = roleManager;
        //}

        public static async void Initialize(IServiceProvider serviceProvider)
        {
            using (var _db = new ApplicationDbContext(
               serviceProvider.GetRequiredService<
                   DbContextOptions<ApplicationDbContext>>()))
            {
                var _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var _userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

                try
                {
                    if (_db.Database.GetPendingMigrations().Count() > 0)
                    {
                        _db.Database.Migrate();
                    }
                }
                catch (Exception)
                {

                }

                if (_db.Roles.Any(r => r.Name == SD.Role_Admin))
                {
                    return;
                }

                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Journalist)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User)).GetAwaiter().GetResult();

                if (_db.Users.Any(u => u.Email.ToLower() == "Admin@Admin.com".ToLower()))
                {
                    return;
                }

                //var Url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}/";
                //var UploadedfilePath = $"{Url}/images/UserImages/Admin.jpg";



                _userManager.CreateAsync(new ApplicationUser
                {
                    Name = "Admin",
                    UserName = "Admin@Admin.com",
                    Email = "Admin@Admin.com",
                    EmailConfirmed = true,
                }, "Admin123!").GetAwaiter().GetResult();

                IdentityUser user = _db.Users.FirstOrDefault(u => u.Email == "Admin@Admin.com");
                //_userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();

                var AdminRole = _db.Roles.SingleOrDefault(r => r.Name == SD.Role_Admin);

                _db.UserRoles.Add(new IdentityUserRole<string>()
                {
                    RoleId = AdminRole.Id,
                    UserId = user.Id
                });
                _db.SaveChanges();

            }
        }
    }
}
