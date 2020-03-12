using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Services;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<IUsersRepository> _mockUsersRepository;
        private User _user;
        private UserDomainModel _userDomainModel;
        private Mock<IReservationService> _mockReservationService;
        private ReservationDomainModel _reservationDomainModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Ime",
                LastName = "Prezime",
                UserName = "i.Prezime",
                IsAdmin = false,
                IsSuperUser = false,
                IsUser = true
            };

            _userDomainModel = new UserDomainModel
            {
                Id = _user.Id,
                FirstName = "Ime",
                LastName = "Prezime",
                UserName = "i.Prezime",
                IsAdmin = false,
                IsSuperUser = false,
                IsUser = true
            };

            _reservationDomainModel = new ReservationDomainModel
            {
                ProjectionId = Guid.NewGuid(),
                SeatId = Guid.NewGuid(),
                UserId = _user.Id
            };

            List<User> userModelsList = new List<User>();
            userModelsList.Add(_user);
            IEnumerable<User> users = userModelsList;
            Task<IEnumerable<User>> responseTask = Task.FromResult(users);

            _mockUsersRepository = new Mock<IUsersRepository>();

            List<ReservationDomainModel> reservationDomainModelsList = new List<ReservationDomainModel>();
            reservationDomainModelsList.Add(_reservationDomainModel);
            IEnumerable<ReservationDomainModel> reservations = reservationDomainModelsList;
            Task<IEnumerable<ReservationDomainModel>> reservationResponseTask = Task.FromResult(reservations);

            _mockReservationService = new Mock<IReservationService>();
        }

        [TestMethod]
        public void UserService_GetAllAsync_ReturnsListOfUsers()
        {

            //Arrange
            List<User> userModelsList = new List<User>();
            userModelsList.Add(_user);
            IEnumerable<User> users = userModelsList;
            Task<IEnumerable<User>> responseTask = Task.FromResult(users);
            int expectedCount = 1;

            UserService userService = new UserService(_mockUsersRepository.Object, _mockReservationService.Object);

            _mockUsersRepository.Setup(x => x.GetAll()).Returns(responseTask);
            //Act

            var resultAction = userService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = (List<UserDomainModel>)resultAction;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.IsInstanceOfType(result[0], typeof(UserDomainModel));
            Assert.AreEqual(_user.Id, result[0].Id);
        }

        [TestMethod]
        public void UserService_GetAllAsync_RepositoryReturnsNull_ReturnNull()
        {

            //Arrange
            IEnumerable<User> users = null;
            Task<IEnumerable<User>> responseTask = Task.FromResult(users);

            UserService userService = new UserService(_mockUsersRepository.Object, _mockReservationService.Object);

            _mockUsersRepository.Setup(x => x.GetAll()).Returns(responseTask);
            //Act

            var resultAction = userService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = (List<UserDomainModel>)resultAction;

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void UserService_GetUserByIdAsync_ReturnsUserDomainModel()
        {
            //Arrange
            Task<User> user = Task.FromResult(_user);
            UserService userService = new UserService(_mockUsersRepository.Object, _mockReservationService.Object);

            _mockUsersRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(user);

            //Act

            var result = userService.GetUserByIdAsync(_user.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UserDomainModel));
            Assert.AreEqual(result.Id, _user.Id);

        }

        [TestMethod]
        public void UserService_GetUserByIdAsync_RepositoryReturnsNull_ReturnNull()
        {
            //Arrange
            User nullUser = null;
            Task<User> user = Task.FromResult(nullUser);
            UserService userService = new UserService(_mockUsersRepository.Object, _mockReservationService.Object);

            _mockUsersRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(user);

            //Act

            var result = userService.GetUserByIdAsync(_user.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);

        }

        [TestMethod]
        public void UserService_GetUserByUserName_ReturnsUserDomainModel()
        {
            //Arrange
            UserService userService = new UserService(_mockUsersRepository.Object, _mockReservationService.Object);

            _mockUsersRepository.Setup(x => x.GetByUserName(It.IsAny<string>())).Returns(_user);

            //Act

            var result = userService.GetUserByUserName(_user.UserName).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UserDomainModel));
            Assert.AreEqual(result.Id, _user.Id);
            Assert.AreEqual(result.UserName, _user.UserName);

        }

        [TestMethod]
        public void UserService_GetUserByUserName_RepositoryReturnsNull_ReturnNull()
        {
            //Arrange
            User nullUser = null;
            UserService userService = new UserService(_mockUsersRepository.Object, _mockReservationService.Object);

            _mockUsersRepository.Setup(x => x.GetByUserName(It.IsAny<string>())).Returns(nullUser);

            //Act

            var result = userService.GetUserByUserName(_user.UserName).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);

        }

        [TestMethod]
        public void UserService_DeleteUserById_ReturnsDeletedUser()
        {
            //Arrange
            UserService userService = new UserService(_mockUsersRepository.Object, _mockReservationService.Object);

            _mockUsersRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(_user);

            //Act

            var result = userService.DeleteUserById(_user.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UserDomainModel));
            Assert.AreEqual(result.Id, _user.Id);
            Assert.AreEqual(result.UserName, _user.UserName);
        }


        [TestMethod]
        public void UserService_DeleteUserById_UserRepositoryReturnsNull_ReturnNull()
        {
            //Arrange
            User nullUser = null;
            UserService userService = new UserService(_mockUsersRepository.Object, _mockReservationService.Object);

            _mockUsersRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(nullUser);

            //Act

            var result = userService.DeleteUserById(_user.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);

        }
        [TestMethod]
        public void UserService_DeleteUserById_ReservationServiceReturnsNull_ReturnNull()
        {
            //Arrange
            IEnumerable<ReservationDomainModel> reservations = null;
            Task<IEnumerable<ReservationDomainModel>> reservationResponseTask = Task.FromResult(reservations);
            UserService userService = new UserService(_mockUsersRepository.Object, _mockReservationService.Object);

            _mockReservationService.Setup(x => x.DeleteByUserId(It.IsAny<Guid>())).Returns(reservationResponseTask);

            //Act

            var result = userService.DeleteUserById(_user.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);

        }
    }
}
