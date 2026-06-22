using Eshop.Application.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTO.Account;
using Pharmacy.Application.Extensions;
using Pharmacy.Application.Services.Interfaces;
using Pharmacy.Application.Utilities;
using Pharmacy.Domain.Entities.Account;
using Pharmacy.Domain.IRepository;

namespace Pharmacy.Application.Services.Implementation
{
    public class UserService : IUserService
    {
        #region Fields and ctor
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Role> _roleRepository;

        public UserService(IGenericRepository<User> userRepository, IGenericRepository<Role> roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }


        #endregion
        #region User Methods

        #region Register
        public async Task<RegisterUserResult> RegisterUser(RegisterUserDto register)
        {
            if (await IsUserExistByMobile(register.Mobile) == false)
            {
                var user = new User
                {
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Email = register.Email,
                    Mobile = register.Mobile,
                    Password = register.Password,
                    Avatar = null,
                    RoleId = 2,
                };

                await _userRepository.AddEntity(user);
                await _userRepository.SaveChanges();
                return RegisterUserResult.Success;

            }

            return RegisterUserResult.MobileExists;

        }

        public async Task<bool> IsUserExistByMobile(string mobile)
        {
            return await _userRepository
              .GetQuery()
              .AsQueryable()
              .AnyAsync
               (x => x.Mobile == mobile);
        }
        #endregion

        #region Get User
        public async Task<User> GetUserByMobile(string mobile)
        {
            return await _userRepository
                .GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync
                (x => x.Mobile == mobile);
        }

        public async Task<string?> GetUserImage(long userId)
        {
            var user = await _userRepository.GetQuery().AsQueryable()
                .FirstOrDefaultAsync(x => x.Id == userId);

            return user?.Avatar;
        }
        #endregion

        #region Login
        public async Task<UserLoginResult> UserLogin(LoginUserDto login)
        {
            var user = await _userRepository.GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync(x => x.Mobile == login.Mobile);

            if (user == null)
            {
                return UserLoginResult.UserNotFound;
            }

            if (user.IsBlocked)
            {
                return UserLoginResult.IsBlocked;
            }

            if (user.Password != login.Password)
            {
                return UserLoginResult.WrongPassword;
            }
            return UserLoginResult.Success;
            //return user.Password != _passwordHasher.EncodePasswordMd5(login.Password)
            //    ? UserLoginResult.UserNotFound : UserLoginResult.Success;
        }

        #endregion

        #region EditUserProfile
        public async Task<EditUserProfileDto> GetProfileForEdit(long userId)
        {
            var user = await _userRepository.GetQuery().AsQueryable().Where(x => !x.IsDelete).FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {

                return null;

            }
            return new EditUserProfileDto
            {
                Id = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Avatar = user.Avatar,

            };
        }

        public async Task<EditUserProfileResult> EditUserProfile(EditUserProfileDto profile, long userId, IFormFile avatarImage)
        {
            var user = await _userRepository.GetQuery().AsQueryable().SingleOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return EditUserProfileResult.NotFound;
            }

            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;
            user.Email = profile.Email;
           
            //user.EditProfile(profile.FirstName, profile.LastName, profile.Email);




            if (avatarImage != null && avatarImage.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(avatarImage.FileName);
                avatarImage.AddImageToServer(imageName, PathExtension.UserAvatarOriginServer,
                    100, 100, PathExtension.UserAvatarThumbServer, user.Avatar);
                user.Avatar = imageName;

                _userRepository.EditEntity(user);
                await _userRepository.SaveChanges();
                return EditUserProfileResult.Success;

            }



            return EditUserProfileResult.NotImage;
        }
        #endregion

        #endregion

        #region dipose

        public async ValueTask DisposeAsync()
        {
            if (_userRepository != null)
            {

                await _userRepository.DisposeAsync();

            }
        }




        #endregion
    }
}
