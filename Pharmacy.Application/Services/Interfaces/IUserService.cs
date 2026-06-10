using Pharmacy.Application.DTO.Account;
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
        #endregion

    }
}
