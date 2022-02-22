using E_poles.Dal;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace E_poles.services.Services
{
    public interface IUserService
    {
        Task<User> Get(int Id);
        Task<bool> UpdateAsync(User model);
        Task<IdentityResult> CreateAsync(User model, string password);
        Task<bool> DeleteAsync(User model);
        Task<IEnumerable<User>> GetAll(int groupsId);
    }
}
