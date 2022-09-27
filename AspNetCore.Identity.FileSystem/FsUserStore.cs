using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace AspNetCore.Identity.FileSystem
{
  
    /// <summary>
    /// Simple class for storing users on in a simple json File
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FsUserStore<T> : IUserPasswordStore<T>,
        IUserLoginStore<T>,
        IUserRoleStore<T>,
        IUserEmailStore<T>,
        IUserStore<T> where T :IdentityUser
    {

        public FsUserDal<T> FsUserDal1 => _dal;
        private readonly FsUserDal<T> _dal;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workingDir">with trailing slash </param>
        public FsUserStore(string workingDir)
        {
            _dal = new FsUserDal<T>(workingDir);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(T user, CancellationToken cancellationToken)
        {
            T userr;
            if (_dal._store.Users.TryGetValue(user.NormalizedUserName, out userr))
            {
                return Task.FromResult(userr.NormalizedUserName);
            }
            return Task.FromResult(default(string));
        }

        public Task<string> GetUserNameAsync(T user, CancellationToken cancellationToken)
        {
            T userr;
            if (_dal._store.Users.TryGetValue(user.Email, out userr))
            {
                return Task.FromResult(userr.UserName);
            }
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(T user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(T user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(T user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> CreateAsync(T user, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = user.Email.ToUpperInvariant();
            if (_dal._store.Users.TryGetValue(user.NormalizedUserName, out T u))
            {
                throw new Exception("User already exists");
            }
            _dal._store.Users[user.NormalizedUserName] = user;
            _dal.Persist();
            var ir = new IdentityResult();
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(T user, CancellationToken cancellationToken)
        {
            _dal._store.Users[user.UserName] = user;
            _dal.Persist();
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(T user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<T> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            userId = userId.ToUpperInvariant();
            T userr;
            if (_dal._store.Users.TryGetValue(userId, out userr))
            {
                return Task.FromResult((T)userr);
            }
            return Task.FromResult(default(T));
        }

        public Task<T> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            T userr;
            if (_dal._store.Users.TryGetValue(normalizedUserName, out userr))
            {
                return Task.FromResult((T)userr);
            }

            var user = _dal._store.Users.Values.FirstOrDefault(u => u.NormalizedUserName.Equals(normalizedUserName));
            if (user != null)
            {
                return Task.FromResult((T)user);
            }
            return Task.FromResult(default(T));
        }

        public Task SetPasswordHashAsync(T user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;

            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task AddLoginAsync(T user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            _dal._store.Logins[login.LoginProvider + login.ProviderKey] = user.Email;
            //_dal._store.Users[user.Email]. = _dal._store.Users[user.Email].Logins ?? new List<UserLoginInfo>();
            //_dal._store.Users[user.Email].Logins.Add(new UserLoginInfo(login.LoginProvider, login.ProviderKey, string.Empty));
            _dal.Persist();
            return Task.CompletedTask;
        }

        public Task RemoveLoginAsync(T user, string loginProvider, string providerKey,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(T user, CancellationToken cancellationToken)
        {
            //return Task.FromResult(_dal._store.Users[user.Email].Logins);
            //todo
            throw new NotImplementedException();
        }

        public Task<T> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            //Bei Facebook z.b. die Facebook ID 10159689967790424

            if (_dal._store.Logins.TryGetValue(loginProvider + providerKey, out string userId))
            {
                return Task.FromResult(_dal._store.Users[userId]);
            }
            return Task.FromResult(default(T));
        }

        public Task AddToRoleAsync(T user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(T user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(T user, CancellationToken cancellationToken)
        {
            IList<string> ret = new List<string>();
            return Task.FromResult(ret);
        }

        public Task<bool> IsInRoleAsync(T user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<T>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailAsync(T user, string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEmailAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);

        }

        public Task<bool> GetEmailConfirmedAsync(T user, CancellationToken cancellationToken)
        {
            return Task.FromResult(_dal._store.Users[user.Email].EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(T user, bool confirmed, CancellationToken cancellationToken)
        {
            _dal._store.Users[user.NormalizedUserName].EmailConfirmed = true;
            _dal.Persist();
            return Task.CompletedTask;
        }

        public Task<T> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return FindByIdAsync(normalizedEmail, cancellationToken);
        }

        public Task<string> GetNormalizedEmailAsync(T user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedEmailAsync(T user, string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
            //throw new NotImplementedException();
        }
    }
}
