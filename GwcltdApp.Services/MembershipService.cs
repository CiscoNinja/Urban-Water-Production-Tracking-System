using GwcltdApp.Data.Infrastructure;
using GwcltdApp.Data.Repositories;
using GwcltdApp.Entities;
using GwcltdApp.Services.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using GwcltdApp.Data.Extensions;

namespace GwcltdApp.Services
{
    public class MembershipService : IMembershipService
    {
        #region Variables
        private readonly IEntityBaseRepository<User> _userRepository;
        private readonly IEntityBaseRepository<Role> _roleRepository;
        private readonly IEntityBaseRepository<UserRole> _userRoleRepository;
        private readonly IEntityBaseRepository<GwclRegion> _gwclRegionRepository;
        private readonly IEntityBaseRepository<GwclStation> _gwclStationRepository;
        private readonly IEntityBaseRepository<UserRegion> _userRegionRepository;
        private readonly IEntityBaseRepository<UserStation> _userStationRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IUnitOfWork _unitOfWork;
        #endregion
        public MembershipService(IEntityBaseRepository<User> userRepository, IEntityBaseRepository<Role> roleRepository,
        IEntityBaseRepository<UserRole> userRoleRepository, IEntityBaseRepository<GwclRegion> gwclRegionRepository,
        IEntityBaseRepository<GwclStation> gwclStationRepository, IEntityBaseRepository<UserRegion> userRegionRepository,
        IEntityBaseRepository<UserStation> userStationRepository, IEncryptionService encryptionService, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _gwclRegionRepository = gwclRegionRepository;
            _gwclStationRepository = gwclStationRepository;
            _userRegionRepository = userRegionRepository;
            _userStationRepository = userStationRepository;
            _encryptionService = encryptionService;
            _unitOfWork = unitOfWork;
        }

        #region IMembershipService Implementation

        public MembershipContext ValidateUser(string username, string password)
        {
            var membershipCtx = new MembershipContext();

            var user = _userRepository.GetSingleByUsername(username);
            if (user != null && isUserValid(user, password))
            {
                var userRoles = GetUserRoles(user.Username);
                membershipCtx.User = user;
                
                var identity = new GenericIdentity(user.Username);
                membershipCtx.Principal = new GenericPrincipal(
                    identity,
                    userRoles.Select(x => x.Name).ToArray());
            }

            return membershipCtx;
        }
        public User CreateUser(string username, string email, string password, int gwclregion, int gwclstation, int[] roles)
        {
            var existingUser = _userRepository.GetSingleByUsername(username);

            if (existingUser != null)
            {
                throw new Exception("Username is already in use");
            }

            var passwordSalt = _encryptionService.CreateSalt();

            var user = new User()
            {
                Username = username,
                Salt = passwordSalt,
                Email = email,
                IsLocked = false,
                HashedPassword = _encryptionService.EncryptPassword(password, passwordSalt),
                DateCreated = DateTime.Now,
                GwclRegionID = gwclregion,
                GwclStationId = gwclstation,
                RoleId = roles[0]
            };

            _userRepository.Add(user);

            _unitOfWork.Commit();

            if (roles != null || roles.Length > 0)
            {
                foreach (var role in roles)
                {
                    addUserToRole(user, role);
                }
            }

            _unitOfWork.Commit();

            if (gwclregion > 0 )
            {
                addUserToRegion(user, gwclregion);
            }

            _unitOfWork.Commit();

            if (gwclstation > 0)
            {
                addUserToStation(user, gwclstation);
            }

            _unitOfWork.Commit();

            return user;
        }

        public User GetUser(int userId)
        {
            return _userRepository.GetSingle(userId);
        }

        public User GetUserStation(string username)
        {
            return _userRepository.GetSingleByUsername(username);
        }

        public User GetUserRegion(string username)
        {
            return _userRepository.GetSingleByUsername(username);
        }

        public List<Role> GetUserRoles(string username)
        {
            List<Role> _result = new List<Role>();

            var existingUser = _userRepository.GetSingleByUsername(username);

            if (existingUser != null)
            {
                foreach (var userRole in existingUser.UserRoles)
                {
                    _result.Add(userRole.Role);
                }
            }

            return _result.Distinct().ToList();
        }
        #endregion

        #region Helper methods
        private void addUserToRole(User user, int roleId)
        {
            var role = _roleRepository.GetSingle(roleId);
            if (role == null)
                throw new ApplicationException("Role doesn't exist.");

            var userRole = new UserRole()
            {
                RoleId = role.ID,
                UserId = user.ID
            };
            _userRoleRepository.Add(userRole);
        }

        private void addUserToRegion(User user, int regionId)
        {
            var gwclregion = _gwclRegionRepository.GetSingle(regionId);
            if (gwclregion == null)
                throw new ApplicationException("Region doesn't exist.");

            var userRegion = new UserRegion()
            {
                GwclRegionId = gwclregion.ID,
                UserId = user.ID
            };
            _userRegionRepository.Add(userRegion);
        }

        private void addUserToStation(User user, int stationId)
        {
            var gwclstation = _gwclStationRepository.GetSingle(stationId);
            if (gwclstation == null)
                throw new ApplicationException("Station doesn't exist.");

            var userStation = new UserStation()
            {
                GwclStationId = gwclstation.ID,
                UserId = user.ID
            };
            _userStationRepository.Add(userStation);
        }

        private bool isPasswordValid(User user, string password)
        {
            return string.Equals(_encryptionService.EncryptPassword(password, user.Salt), user.HashedPassword);
        }

        private bool isUserValid(User user, string password)
        {
            if (isPasswordValid(user, password))
            {
                return !user.IsLocked;
            }

            return false;
        }
        #endregion
    }
}
