using E_poles.Dal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_poles.Services
{
    public interface IGroupService
    {
        Task<Groups> Get(int Id);
        Task<UserGroups> GetGroupByUserId(int userId);
        Task<IEnumerable<Groups>> GetAll(int groupsId);
        Task<bool> DeleteAsync(Groups model);
        Task<Groups> CreateAsync(Groups model);
        Task<bool> UpdateAsync(Groups model);
        Task CreateUserGroupsAsync(UserGroups usergrp);
        Task DeleteUserGroupsAsync(UserGroups usergrp);
    }
}
