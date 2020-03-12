using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Controllers;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Tests.Controllers
{
    [TestClass]
    public class UsersControllerTests
    {
        private Mock<IUserService> _mockUserService;
        private UserDomainModel _userDomainModel;
        private CreateUserModel _createUserModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _userDomainModel = new UserDomainModel
            {
                Id = Guid.NewGuid(),
                FirstName = "David",
                LastName = "Strbac",
                UserName = "StrbacD",
                BonusPoints = 0,
                IsAdmin = false,
                IsSuperUser = false,
                IsUser = true
            };

            _createUserModel = new CreateUserModel
            {
                FirstName = "David",
                LastName = "Strbac",
                UserName = "StrbacD"
            };

            List<UserDomainModel> userList = new List<UserDomainModel>();
            userList.Add(_userDomainModel);
            IEnumerable<UserDomainModel> users = userList;
            Task<IEnumerable<UserDomainModel>> responseTask = Task.FromResult(users);

            _mockUserService = new Mock<IUserService>();
        }

        [TestMethod]
        public void UsersController_GetAsync_ReturnsListOfAllUsers()
        {
            //Arrange
            List<UserDomainModel> userList = new List<UserDomainModel>();
            userList.Add(_userDomainModel);
            IEnumerable<UserDomainModel> users = userList;
            Task<IEnumerable<UserDomainModel>> responseTask = Task.FromResult(users);
            int expectedCount = 1;
            int expectedStatusCode = 200;

            UsersController usersController = new UsersController(_mockUserService.Object);

            _mockUserService.Setup(x => x.GetAllAsync()).Returns(responseTask);

            //Act
            var result = usersController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var usersDomainModelResultList = (List<UserDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(usersDomainModelResultList.Count, expectedCount);
            Assert.IsInstanceOfType(usersDomainModelResultList[0], typeof(UserDomainModel));
            Assert.AreEqual(usersDomainModelResultList[0].Id, _userDomainModel.Id);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void UsersController_GetAll_UserServiceReturnsNull_ReturnEmptyList()
        {
            //Arrange
            IEnumerable<UserDomainModel> movieTags = null;
            Task<IEnumerable<UserDomainModel>> responseTask = Task.FromResult(movieTags);
            int expectedCount = 0;
            int expectedStatusCode = 200;

            UsersController usersController = new UsersController(_mockUserService.Object);

            _mockUserService.Setup(x => x.GetAllAsync()).Returns(responseTask);

            //Act
            var result = usersController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var usersDomainModelResultList = (List<UserDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(usersDomainModelResultList.Count, expectedCount);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void UsersController_GetById_ReturnsUser()
        {
            //Arrange
            Task<UserDomainModel> user = Task.FromResult(_userDomainModel);
            UsersController usersController = new UsersController(_mockUserService.Object);
            int expectedStatusCode = 200;

            _mockUserService.Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>())).Returns(user);

            //Act
            var result = usersController.GetbyIdAsync(_userDomainModel.Id).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultModel = ((OkObjectResult)result);
            var resultList = ((OkObjectResult)result).Value;
            var usersDomainModelResult = (UserDomainModel)resultList;

            //Assert

            Assert.IsNotNull(usersDomainModelResult);
            Assert.IsInstanceOfType(usersDomainModelResult, typeof(UserDomainModel));
            Assert.IsInstanceOfType(resultModel, typeof(OkObjectResult));
            Assert.AreEqual(((OkObjectResult)result).StatusCode, expectedStatusCode);
            Assert.AreEqual(usersDomainModelResult.Id, _userDomainModel.Id);
        }

        [TestMethod]
        public void UsersController_GetById_UserServiceReturnsNull_404()
        {
            //Arrange
            UserDomainModel nullUser = null;
            Task<UserDomainModel> user = Task.FromResult(nullUser);

            UsersController usersController = new UsersController(_mockUserService.Object);
            int expectedStatusCode = 404;
            string expectedMessage = Messages.USER_NOT_FOUND;

            _mockUserService.Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>())).Returns(user);

            //Act
            var result = usersController.GetbyIdAsync(_userDomainModel.Id).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultResponse = ((NotFoundObjectResult)result);
            var badObjectResult = ((NotFoundObjectResult)result).Value;

            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, badObjectResult);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }

        [TestMethod]
        public void UserController_Post_ReturnsInsertedUser()
        {
            //Arrange
            Task<UserDomainModel> userDomainModel = Task.FromResult(_userDomainModel);
            int expectedStatusCode = 201;

            UsersController usersController = new UsersController(_mockUserService.Object);

            _mockUserService.Setup(x => x.AddUser(It.IsAny<UserDomainModel>())).Returns(userDomainModel);

            //Act

            var result = usersController.Post(_createUserModel).ConfigureAwait(false).GetAwaiter().GetResult();
            var resultMessage = ((CreatedResult)result).Value;
            var resultModel = (UserDomainModel)resultMessage;

            //Assert
            Assert.IsNotNull(resultMessage);
            Assert.AreEqual(expectedStatusCode, ((CreatedResult)result).StatusCode);
            Assert.IsInstanceOfType(resultModel, typeof(UserDomainModel));
            Assert.AreEqual(resultModel.Id, _userDomainModel.Id);
        }


        [TestMethod]
        public void UsersController_Post_ThrowsDbUpdateException()
        {
            //Arrange
            string expectedMessage = "Inner exception error message.";
            int expectedStatusCode = 400;
            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);

            UsersController usersController = new UsersController(_mockUserService.Object);

            _mockUserService.Setup(x => x.AddUser(It.IsAny<UserDomainModel>())).Throws(dbUpdateException);

            //Act

            var result = usersController.Post(_createUserModel).ConfigureAwait(false).GetAwaiter().GetResult();
            var resultResponse = ((BadRequestObjectResult)result);
            var badObjectResult = ((BadRequestObjectResult)result).Value;
            var errorResult = (ErrorResponseModel)badObjectResult;

            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }

        [TestMethod]
        public void UserController_Post_UserServiceReturnsNull()
        {
            //Arrange
            UserDomainModel nullDomainModel = null;
            Task<UserDomainModel> nullTask = Task.FromResult(nullDomainModel);

            string expectedMessage = Messages.USER_CREATION_ERROR;
            int expectedStatusCode = 500;

            ErrorResponseModel errorResponseModel = new ErrorResponseModel();
            errorResponseModel.ErrorMessage = expectedMessage;
            errorResponseModel.StatusCode = System.Net.HttpStatusCode.InternalServerError;

            UsersController usersController = new UsersController(_mockUserService.Object);

            _mockUserService.Setup(x => x.AddUser(It.IsAny<UserDomainModel>())).Returns(nullTask);

            //Act
            var result = usersController.Post(_createUserModel).ConfigureAwait(false).GetAwaiter().GetResult();
            ObjectResult viewResult = (ObjectResult)result;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(viewResult.Value.ToString(), errorResponseModel.ToString());
            Assert.AreEqual(viewResult.StatusCode, expectedStatusCode);
        }


        [TestMethod]
        public void UsersController_DeleteById_ReturnsDeletedUser()
        {
            //Arrange
            Task<UserDomainModel> user = Task.FromResult(_userDomainModel);
            UsersController usersController = new UsersController(_mockUserService.Object);
            int expectedStatusCode = 202;

            _mockUserService.Setup(x => x.DeleteUserById(It.IsAny<Guid>())).Returns(user);

            //Act
            var result = usersController.Delete(_userDomainModel.Id).ConfigureAwait(false).GetAwaiter().GetResult();
            var resultModel = ((AcceptedResult)result);
            var resultList = ((AcceptedResult)result).Value;
            var usersDomainModelResult = (UserDomainModel)resultList;

            //Assert

            Assert.IsNotNull(usersDomainModelResult);
            Assert.IsInstanceOfType(usersDomainModelResult, typeof(UserDomainModel));
            Assert.IsInstanceOfType(resultModel, typeof(AcceptedResult));
            Assert.AreEqual(((AcceptedResult)result).StatusCode, expectedStatusCode);
            Assert.AreEqual(usersDomainModelResult.Id, _userDomainModel.Id);
        }


        [TestMethod]
        public void UsersController_DeleteUserById_ThrowsDbUpdateException()
        {
            //Arrange
            string expectedMessage = "Inner exception error message.";
            int expectedStatusCode = 400;
            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);

            UsersController usersController = new UsersController(_mockUserService.Object);

            _mockUserService.Setup(x => x.DeleteUserById(It.IsAny<Guid>())).Throws(dbUpdateException);

            //Act

            var result = usersController.Delete(_userDomainModel.Id).ConfigureAwait(false).GetAwaiter().GetResult();
            var resultResponse = ((BadRequestObjectResult)result);
            var badObjectResult = ((BadRequestObjectResult)result).Value;
            var errorResult = (ErrorResponseModel)badObjectResult;

            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }

        [TestMethod]
        public void UserController_DeleteUserById_UserServiceReturnsNull()
        {
            //Arrange
            UserDomainModel nullDomainModel = null;
            Task<UserDomainModel> nullTask = Task.FromResult(nullDomainModel);

            string expectedMessage = Messages.USER_NOT_FOUND;
            int expectedStatusCode = 500;

            ErrorResponseModel errorResponseModel = new ErrorResponseModel();
            errorResponseModel.ErrorMessage = expectedMessage;
            errorResponseModel.StatusCode = System.Net.HttpStatusCode.InternalServerError;

            UsersController usersController = new UsersController(_mockUserService.Object);

            _mockUserService.Setup(x => x.DeleteUserById(It.IsAny<Guid>())).Returns(nullTask);

            //Act
            var result = usersController.Delete(_userDomainModel.Id).ConfigureAwait(false).GetAwaiter().GetResult();
            ObjectResult viewResult = (ObjectResult)result;
            var messageResult = (ErrorResponseModel)viewResult.Value;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedMessage, messageResult.ErrorMessage);
            Assert.AreEqual(expectedStatusCode, (int)messageResult.StatusCode);
        }


        [TestMethod]
        public void UserController_Put_ReturnsInsertedUser()
        {
            //Arrange
            Task<UserDomainModel> userDomainModel = Task.FromResult(_userDomainModel);
            int expectedStatusCode = 202;

            UsersController usersController = new UsersController(_mockUserService.Object);

            _mockUserService.Setup(x => x.UpdateUser(It.IsAny<UserDomainModel>())).Returns(userDomainModel);
            _mockUserService.Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>())).Returns(userDomainModel);

            //Act

            var result = usersController.Put(_userDomainModel.Id, _createUserModel).ConfigureAwait(false).GetAwaiter().GetResult();
            var resultMessage = ((AcceptedResult)result).Value;
            var resultModel = (UserDomainModel)resultMessage;

            //Assert
            Assert.IsNotNull(resultMessage);
            Assert.AreEqual(expectedStatusCode, ((AcceptedResult)result).StatusCode);
            Assert.IsInstanceOfType(resultModel, typeof(UserDomainModel));
            Assert.AreEqual(resultModel.Id, _userDomainModel.Id);
        }


        [TestMethod]
        public void UserController_Put_UserServiceReturnsNull()
        {
            //Arrange
            UserDomainModel nullUser = null;
            Task<UserDomainModel> nullDomainModel = Task.FromResult(nullUser);
            Task<UserDomainModel> userDomainModel = Task.FromResult(_userDomainModel);

            string expectedMessage = Messages.USER_NOT_FOUND;
            int expectedStatusCode = 400;

            ErrorResponseModel errorResponseModel = new ErrorResponseModel();
            errorResponseModel.ErrorMessage = expectedMessage;
            errorResponseModel.StatusCode = System.Net.HttpStatusCode.InternalServerError;

            UsersController usersController = new UsersController(_mockUserService.Object);

            _mockUserService.Setup(x => x.UpdateUser(It.IsAny<UserDomainModel>())).Returns(userDomainModel);
            _mockUserService.Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>())).Returns(nullDomainModel);

            //Act

            var result = usersController.Put(_userDomainModel.Id, _createUserModel).ConfigureAwait(false).GetAwaiter().GetResult();
            ObjectResult viewResult = (ObjectResult)result;
            var messageResult = (ErrorResponseModel)viewResult.Value;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedMessage, messageResult.ErrorMessage);
            Assert.AreEqual(expectedStatusCode,(int)messageResult.StatusCode);
        }

        [TestMethod]
        public void ReservationsController_Put_InvalidModelState()
        {
            //Arrange
            string expectedMessage = "Invalid Model State";
            int expectedStatusCode = 400;

            UsersController usersController = new UsersController(_mockUserService.Object);
            usersController.ModelState.AddModelError("key", "Invalid Model State");

            //Act
            var result = usersController.Put(_userDomainModel.Id, _createUserModel).ConfigureAwait(false).GetAwaiter().GetResult();
            var resultMessage = (BadRequestObjectResult)result;
            var resultMassageValue = ((BadRequestObjectResult)result).Value;
            var errorResult = ((SerializableError)resultMassageValue).GetValueOrDefault("key");
            var message = (string[])errorResult;


            //Assert
            Assert.IsNotNull(resultMessage);
            Assert.AreEqual(expectedMessage, message[0]);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultMessage.StatusCode);
        }


        [TestMethod]
        public void UsersController_Put_ThrowsDbUpdateException()
        {
            //Arrange
            string expectedMessage = Messages.USER_NOT_FOUND;
            int expectedStatusCode = 400;
            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);

            UsersController usersController = new UsersController(_mockUserService.Object);

            _mockUserService.Setup(x => x.UpdateUser(It.IsAny<UserDomainModel>())).Throws(dbUpdateException);

            //Act

            var result = usersController.Put(_userDomainModel.Id, _createUserModel).ConfigureAwait(false).GetAwaiter().GetResult();
            var resultResponse = ((BadRequestObjectResult)result);
            var badObjectResult = ((BadRequestObjectResult)result).Value;
            var errorResult = (ErrorResponseModel)badObjectResult;

            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }
    }
}
