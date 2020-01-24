using School.Contracts;
using School.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(UserRegistrationRequest model);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<string> CreateRole(string roleName);
    }
}
