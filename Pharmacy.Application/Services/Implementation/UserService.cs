using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Application.DTO.Account;
using Pharmacy.Application.DTO.Paging;
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
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IGenericRepository<User> userRepository, IGenericRepository<Role> roleRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
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
                    Password = _passwordHasher.EncodePasswordMd5(register.Password),
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

        public async Task<User> GetUserById(long id)
        {
            return await _userRepository
                 .GetQuery()
                 .AsQueryable()
            .SingleOrDefaultAsync
                 (x => x.Id == id);
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


            if (user.Password != _passwordHasher.EncodePasswordMd5(login.Password))
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

        public async Task<EditUserProfileResult> EditUserProfile(EditUserProfileDto profile, long userId, IFormFile? avatarImage)
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



            }

            _userRepository.EditEntity(user);
            await _userRepository.SaveChanges();
            return EditUserProfileResult.Success;

            
        }
        #endregion

        #region Change User Password

        public async Task<ChangePasswordResult> ChangeUserPassword(ChangePasswordDto changePassword, long userId)
        {
            var user = await _userRepository.GetEntityById(userId);
            if (user == null)
                return ChangePasswordResult.Error;


            var currentPasswordHash = _passwordHasher.EncodePasswordMd5(changePassword.CurrentPassword);
            if (user.Password != currentPasswordHash)
                return ChangePasswordResult.WrongCurrentPassword;


            var newPasswordHash = _passwordHasher.EncodePasswordMd5(changePassword.NewPassword);
            if (user.Password == newPasswordHash)
                return ChangePasswordResult.NewPasswordSameAsOld;


            user.Password = newPasswordHash;
            await _userRepository.SaveChanges();
            return ChangePasswordResult.Success;
        }


        #endregion

        #region Filter
        public async Task<FilterUserDto> FilterUser(FilterUserDto filter)
        {
            var query = _userRepository
                 .GetQuery()
                 .Include(x => x.Role)
                 .AsQueryable();

            if (filter.RoleId > 0)
            {
                query = query.Where(x => x.RoleId == filter.RoleId);
            }
            if (!string.IsNullOrEmpty(filter.FirstName))
            {
                query = query.Where(x => EF.Functions.Like(x.FirstName, $"%{filter.FirstName}%"));
            }
            if (!string.IsNullOrEmpty(filter.LastName))
            {
                query = query.Where(x => EF.Functions.Like(x.LastName, $"%{filter.LastName}%"));
            }
            if (!string.IsNullOrEmpty(filter.Mobile))
            {
                query = query.Where(x => EF.Functions.Like(x.Mobile, $"%{filter.Mobile}%"));
            }
            if (!string.IsNullOrEmpty(filter.Email))
            {
                query = query.Where(x => EF.Functions.Like(x.Email, $"%{filter.Email}%"));
            }

            #region Paging


            var roleCount = await query.CountAsync();

            var pager = Pager.Build(filter.PageId, roleCount, filter.TakeEntity,
                filter.HowManyShowPageAfterAndBefore);

            var allEntities = await query.Paging(pager).OrderByDescending(x => x.Id).ToListAsync();


            #endregion

            return filter.SetPaging(pager).SetUsers(allEntities);
        }
        #endregion

        #region EditUser
        public async Task<EditUserDto> GetUserForEdit(long userId)
        {
            var user = await _userRepository.GetQuery().AsQueryable().Include(x => x.Role).SingleOrDefaultAsync(x => x.Id == userId);

            if (user == null)
            {
                return null;
            }

            return new EditUserDto
            {
                Id = user.Id,
                RoleId = user.Role.Id,
                Email = user.Email,
                Mobile = user.Mobile,
                IsBlocked = user.IsBlocked,

                FirstName = user.FirstName,
                LastName = user.LastName,
            };
        }

        public async Task<EditUserResult> EditUser(EditUserDto edit, string username)
        {
            var user = await _userRepository.GetQuery().AsQueryable().Include(x => x.Role).SingleOrDefaultAsync(x => x.Id == edit.Id);
            if (user == null)
            {
                return EditUserResult.UserNotFound;

            }

            user.Id = edit.Id;
            user.RoleId = edit.RoleId;
            user.FirstName = edit.FirstName;
            user.LastName = edit.LastName;
            user.Mobile = edit.Mobile;
            user.Email = edit.Email;
            user.IsBlocked = edit.IsBlocked;


            _userRepository.EditEntityByUser(user, username);
            _userRepository.SaveChanges();
            return EditUserResult.Success;
        }
        #endregion

        #endregion

        #region Role Methods

        #region Filter
        public async Task<FilterRoleDto> FilterRole(FilterRoleDto filter)
        {
            var query = _roleRepository
                .GetQuery()
                .Include(x => x.Users)
                .AsQueryable();


            if (!string.IsNullOrEmpty(filter.RoleName))
            {
                query = query.Where(x => EF.Functions.Like(x.RoleName, $"%{filter.RoleName}%"));
            }

            #region Paging

            var roleCount = await query.CountAsync();
            var pager = Pager.Build(filter.PageId, roleCount, filter.TakeEntity,
                filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();
            #endregion
            return filter.SetPaging(pager).SetRoles(allEntities);
        }
        #endregion

        #region Create
        public async Task<CreateRoleResult> CreateRole(CreateRoleDto role)
        {
            var newRole = new Role
            {
                RoleName = role.RoleName,
            };
            await _roleRepository.AddEntity(newRole);
            _roleRepository.SaveChanges();
            return CreateRoleResult.Success;
        }
        #endregion

        #region Edit
        public async Task<EditRoleDto> GetRoleForEdit(long roleId)
        {
            var edit = await _roleRepository.GetQuery().AsQueryable().SingleOrDefaultAsync(x => x.Id == roleId);
            if (edit == null)
            {
                return null;
            }
            return new EditRoleDto
            {
                Id = edit.Id,
                RoleName = edit.RoleName,

            };
        }

        public async Task<EditRoleResult> EditRole(EditRoleDto edit, string username)
        {
            var role = await _roleRepository.GetQuery().AsQueryable().SingleOrDefaultAsync(x => x.Id == edit.Id);
            if (role == null)
            {
                return EditRoleResult.Error;
            }

            role.Id = edit.Id;
            role.RoleName = edit.RoleName;

            _roleRepository.EditEntityByUser(role, username);
            _roleRepository.SaveChanges();
            return EditRoleResult.Success;



        }
        #endregion

        #region GetAll
        public async Task<List<Role>> GetRoles()
        {
            return await _roleRepository.GetQuery().AsQueryable().Select(x => new Role
            {
                Id = x.Id,
                RoleName = x.RoleName,
            }).ToListAsync();

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
