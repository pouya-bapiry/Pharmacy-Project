using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTO.Account;
using Pharmacy.Application.Services.Interfaces;
using Pharmacy.Domain.Entities.Account;
using Pharmacy.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<User> GetUserByMobile(string mobile)
        {
            return await _userRepository
                .GetQuery()
                .AsQueryable()
                .SingleOrDefaultAsync
                (x => x.Mobile == mobile);
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
