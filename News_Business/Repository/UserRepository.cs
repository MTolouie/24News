using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using News_Business.Repository.IRepository;
using News_Common;
using News_DataLayer.Data;
using News_DataLayer.Models;
using News_Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News_Business.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        //private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> ActiveUserById(string UserId)
        {
            try
            {
                var user = await _db.ApplicationUser.FindAsync(UserId);
                if (user == null)
                {
                    return false;
                }
                user.IsActive = true;
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeActivateUserById(string UserId)
        {
            try
            {
                var user = await _db.ApplicationUser.FindAsync(UserId);
                if (user == null)
                {
                    return false;
                }
                user.IsActive = false;
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserById(string UserId)
        {
            try
            {
                var user = await _db.ApplicationUser.FindAsync(UserId);
                if (user == null)
                {
                    return false;
                }
                _db.Users.Remove(user);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public int GetAllUserCount()
        {
            try
            {
                return _db.ApplicationUser.Count();
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public async Task<Tuple<List<ApplicationUserDTO>, int>> GetAllUsers(int pageId, string Name = "", string Role = "", string FromDate = "", string ToDate = "")
        {
            try
            {
                IQueryable<ApplicationUser> users = _db.ApplicationUser.Where(u => u.IsActive);

                if (!string.IsNullOrEmpty(Name))
                {
                    users = users.Where(u => u.Name.Contains(Name));
                   
                }
                if (!string.IsNullOrEmpty(Role))
                {
                    var roleId = _db.Roles.SingleOrDefault(r => r.Name == Role)?.Id;
                    var UserIds = _db.UserRoles.Where(ur => ur.RoleId == roleId).Select(s=>s.UserId).ToList();
                    users = users.Where(u =>UserIds.Contains(u.Id));
                }
                if (!string.IsNullOrEmpty(FromDate))
                {
                    DateTime fromdate = DateTime.ParseExact(FromDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    users = users.Where(u=>u.CreateDate >= fromdate);
                }
                if (!string.IsNullOrEmpty(ToDate))
                {
                    DateTime toDate = DateTime.ParseExact(ToDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    users = users.Where(u => u.CreateDate <= toDate);
                }


                var userDTO = _mapper.Map<IQueryable<ApplicationUser>, IEnumerable<ApplicationUserDTO>>(users);

                int take = 5;
                int skip = (pageId - 1) * take;
                var pageCount = userDTO.Count() / take;
                if (pageCount % 2 != 0)
                {
                    pageCount++;
                }

                var query = userDTO.OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();
                return Tuple.Create(query, pageCount);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Tuple<List<ApplicationUserDTO>, int>> GetDeActiveUsers(int pageId, string Name = "", string Role = "", string FromDate = "", string ToDate = "")
        {
            try
            {
                IQueryable<ApplicationUser> users = _db.ApplicationUser.Where(u => !u.IsActive);

                if (!string.IsNullOrEmpty(Name))
                {
                    users = users.Where(u => u.Name.Contains(Name));

                }
                if (!string.IsNullOrEmpty(Role))
                {
                    var roleId = _db.Roles.SingleOrDefault(r => r.Name == Role)?.Id;
                    var UserIds = _db.UserRoles.Where(ur => ur.RoleId == roleId).Select(s => s.UserId).ToList();
                    users = users.Where(u => UserIds.Contains(u.Id));
                }
                if (!string.IsNullOrEmpty(FromDate))
                {
                    DateTime fromdate = DateTime.ParseExact(FromDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    users = users.Where(u => u.CreateDate >= fromdate);
                }
                if (!string.IsNullOrEmpty(ToDate))
                {
                    DateTime toDate = DateTime.ParseExact(ToDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
                    users = users.Where(u => u.CreateDate <= toDate);
                }


                var userDTO = _mapper.Map<IQueryable<ApplicationUser>, IEnumerable<ApplicationUserDTO>>(users);

                int take = 5;
                int skip = (pageId - 1) * take;
                var pageCount = userDTO.Count() / take;
                if (pageCount % 2 != 0)
                {
                    pageCount++;
                }

                var query = userDTO.OrderByDescending(c => c.CreateDate).Skip(skip).Take(take).ToList();
                return Tuple.Create(query, pageCount);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public int GetJournalistCount()
        {
            try
            {
                var roleId = _db.Roles.SingleOrDefault(r => r.Name == SD.Role_Journalist)?.Id;
                var UserIds = _db.UserRoles.Where(ur => ur.RoleId == roleId).Select(s => s.UserId).ToList();
                return _db.Users.Where(u => UserIds.Contains(u.Id)).Count();
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public async Task<ApplicationUserDTO> GetUserById(string UserId)
        {
            try
            {
                var user =  _db.ApplicationUser.SingleOrDefault(u=>u.Id == UserId);
                if (user == null)
                {
                    return null;
                }
                var userDTO = _mapper.Map<ApplicationUser, ApplicationUserDTO>(user);
               
                return userDTO;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ApplicationUserDTO GetUserByUserName(string Name)
        {
            try
            {
               var user = _db.ApplicationUser.SingleOrDefault(u=>u.UserName.ToLower().Trim() == Name.Trim().ToLower());
                if (user == null)
                {
                    return null;
                }
                return _mapper.Map<ApplicationUser,ApplicationUserDTO>(user);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<int> GetUserCount()
        {
            try
            {
                var users = await _db.ApplicationUser.ToListAsync();

                return users.Count();
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public string GetUserRoleName(string UserId)
        {
            try
            {
                var userRole = _db.UserRoles.SingleOrDefault(r => r.UserId == UserId);
                var role = _db.Roles.SingleOrDefault(r => r.Id == userRole.RoleId);
                return role.Name;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> GiveUserJournalistRole(string UserId)
        {
            try
            {
                var user = _db.ApplicationUser.Find(UserId);
                if (user == null)
                {
                    return false;
                }

                var role = _db.Roles.SingleOrDefault(r => r.Name == SD.Role_Journalist);


                var userRole = _db.UserRoles.SingleOrDefault(r => r.UserId == UserId);

                _db.UserRoles.Remove(userRole);

                _db.UserRoles.Add(new Microsoft.AspNetCore.Identity.IdentityUserRole<string>()
                {
                    UserId = UserId,
                    RoleId = role.Id,
                });

                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> GiveUserUserRole(string UserId)
        {
            try
            {
                var user = _db.ApplicationUser.Find(UserId);
                if (user == null)
                {
                    return false;
                }

                var role = _db.Roles.SingleOrDefault(r => r.Name == SD.Role_User);


                var userRole = _db.UserRoles.SingleOrDefault(r => r.UserId == UserId);

                _db.UserRoles.Remove(userRole);



                _db.UserRoles.Add(new Microsoft.AspNetCore.Identity.IdentityUserRole<string>()
                {
                    UserId = UserId,
                    RoleId = role.Id,
                });

                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> UpdateUserAvatar(string UserId, string FileName)
        {
            try
            {
                var user = await _db.ApplicationUser.FindAsync(UserId);
                if (user != null)
                {
                    user.Avatar = FileName;
                    _db.Users.Update(user);
                    await _db.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
