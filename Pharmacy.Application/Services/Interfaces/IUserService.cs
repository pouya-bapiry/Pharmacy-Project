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
        Task<EditUserProfileDto> GetProfileForEdit(long userId);
        Task<EditUserProfileResult> EditUserProfile(EditUserProfileDto profile, long userId, IFormFile avatarImage);
        Task<ChangePasswordResult> ChangeUserPassword(ChangePasswordDto changePassword, long userId);

        #endregion

    }
}
