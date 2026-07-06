using Microsoft.AspNetCore.Http;
using Pharmacy.Application.DTO.Account;
using Pharmacy.Domain.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pharmacy.Application.DTO.Account.RegisterUserDto;

namespace Pharmacy.Application.Services.Interfaces
{
    public interface IUserService : IAsyncDisposable
    {
        #region User
        Task<RegisterUserResult> RegisterUser(RegisterUserDto register);
        Task<bool> IsUserExistByMobile(string mobile);
        Task<User> GetUserByMobile(string mobile);
        Task<UserLoginResult> UserLogin(LoginUserDto login);
        Task<string?> GetUserImage(long userId);
        Task<User> GetUserById(long id);
        Task<EditUserProfileDto> GetProfileForEdit(long userId);
        Task<EditUserProfileResult> EditUserProfile(EditUserProfileDto profile, long userId, IFormFile avatarImage);
        Task<ChangePasswordResult> ChangeUserPassword(ChangePasswordDto changePassword, long userId);
        Task<FilterUserDto> FilterUser(FilterUserDto filter);
        Task<EditUserDto> GetUserForEdit(long userId);
        Task<EditUserResult> EditUser(EditUserDto edit, string username);

        #endregion

        #region Role

        Task<FilterRoleDto> FilterRole(FilterRoleDto filter);
        Task<CreateRoleResult> CreateRole(CreateRoleDto role);
        Task<EditRoleDto> GetRoleForEdit(long roleId);
        Task<EditRoleResult> EditRole(EditRoleDto edit, string username);
        Task<List<Role>> GetRoles();

        #endregion

    }
}
