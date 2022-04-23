using Microsoft.AspNetCore.Identity;
using News_Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News_Business.Repository.IRepository
{
    public interface IRoleRepository
    {
        public  Task<IEnumerable<IdentityRoleDTO>> GetAllRoles();
    }
}
