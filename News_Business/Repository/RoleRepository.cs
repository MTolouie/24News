using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using News_Business.Repository.IRepository;
using News_DataLayer.Data;
using News_Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News_Business.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public RoleRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<IEnumerable<IdentityRoleDTO>> GetAllRoles()
        {
            try
            {
                var roles = await _db.Roles.ToListAsync();
                var rolesDTO = _mapper.Map<IEnumerable<IdentityRole>,IEnumerable<IdentityRoleDTO>>(roles);
                return rolesDTO;
            }
            catch (Exception e)
            {
                return null;
            }
        }

       
    }
}
