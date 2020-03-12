using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IReservationService _reservationService;

        public UserService(IUsersRepository usersRepository, IReservationService reservationService)
        {
            _usersRepository = usersRepository;
            _reservationService = reservationService;
        }

        public async Task<IEnumerable<UserDomainModel>> GetAllAsync()
        {
            var data = await _usersRepository.GetAll();

            if (data == null)
            {
                return null;
            }

            List<UserDomainModel> result = new List<UserDomainModel>();
            UserDomainModel model;
            foreach (var item in data)
            {
                model = new UserDomainModel
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    UserName = item.UserName,
                    IsAdmin = item.IsAdmin,
                    IsSuperUser = item.IsSuperUser,
                    IsUser = item.IsUser,
                    BonusPoints = item.BonusPoints
                };
                result.Add(model);
            }

            return result;
        }

        public async Task<UserDomainModel> AddUser(UserDomainModel newUser)
        {
            User userToCreate = new User()
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                UserName = newUser.UserName,
                IsAdmin = false,
                IsSuperUser = false,
                IsUser = true,
                BonusPoints = 0
            };

            var data = _usersRepository.Insert(userToCreate);

            if (data == null)
            {
                return null;
            }

            _usersRepository.Save();

            UserDomainModel domainModel = new UserDomainModel()
            {
                Id = data.Id,
                FirstName = data.FirstName,
                LastName = newUser.LastName,
                UserName = newUser.UserName,
                IsAdmin = data.IsAdmin,
                IsSuperUser = data.IsSuperUser,
                IsUser = data.IsUser,
                BonusPoints = 0
            };

            return domainModel;
        }

        public int AddBonusPointsByUserId(Guid userId, int bonusPoints)
        {
            var bonusPointState = _usersRepository.AddBonusPointsByUserId(userId, bonusPoints);
            if (bonusPointState == null || bonusPointState < 0)
            {
                return -1;
            }
            _usersRepository.Save();

            return bonusPointState;
        }

        public async Task<UserDomainModel> GetUserByIdAsync(Guid id)
        {
            var data = await _usersRepository.GetByIdAsync(id);

            if (data == null)
            {
                return null;
            }

            UserDomainModel domainModel = new UserDomainModel
            {
                Id = data.Id,
                FirstName = data.FirstName,
                LastName = data.LastName,
                UserName = data.UserName,
                IsAdmin = data.IsAdmin,
                IsSuperUser = data.IsSuperUser,
                IsUser = data.IsUser,
                BonusPoints = data.BonusPoints
            };

            return domainModel;
        }

        public async Task<UserDomainModel> UpdateUser(UserDomainModel updateUser)
        {
            User user = new User()
            {
                Id = updateUser.Id,
                UserName = updateUser.UserName,
                FirstName = updateUser.FirstName,
                LastName = updateUser.LastName,
                IsAdmin = updateUser.IsAdmin,
                IsSuperUser = updateUser.IsSuperUser,
                IsUser = true,
                BonusPoints = updateUser.BonusPoints
            };

            var data = _usersRepository.Update(user);

            if (data == null)
            {
                return null;
            }
            _usersRepository.Save();

            UserDomainModel domainModel = new UserDomainModel()
            {
                Id = data.Id,
                UserName = data.UserName,
                FirstName = data.FirstName,
                LastName = data.LastName,
                IsAdmin = data.IsAdmin,
                IsSuperUser = data.IsSuperUser,
                IsUser = data.IsUser,
                BonusPoints = data.BonusPoints
            };

            return domainModel;
        }

        public async Task<UserDomainModel> GetUserByUserName(string username)
        {
            var data = _usersRepository.GetByUserName(username);

            if (data == null)
            {
                return null;
            }

            UserDomainModel domainModel = new UserDomainModel
            {
                Id = data.Id,
                FirstName = data.FirstName,
                LastName = data.LastName,
                UserName = data.UserName,
                IsAdmin = data.IsAdmin,
                IsSuperUser = data.IsSuperUser,
                IsUser = data.IsUser,
                BonusPoints = data.BonusPoints
            };

            return domainModel;
        }


        public async Task<UserDomainModel> DeleteUserById(Guid userId)
        {
            var reservationData = await _reservationService.DeleteByUserId(userId);

            if (reservationData == null)
            {
                return null;
            }

            var userData = _usersRepository.Delete(userId);
            if (userData == null)
            {
                return null;
            }

            _usersRepository.Save();

            UserDomainModel domainModel = new UserDomainModel
            {
                Id = userData.Id,
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                UserName = userData.UserName,
                IsAdmin = userData.IsAdmin,
                IsSuperUser = userData.IsSuperUser,
                IsUser = userData.IsUser,
                BonusPoints = userData.BonusPoints
            };

            return domainModel;
        }
    }
}
