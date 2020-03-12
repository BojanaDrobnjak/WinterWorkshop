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
    public class ReservationsControllerTests
    {
        private Mock<IReservationService> _mockReservationService;
        private Mock<IUserService> _mockUserService;
        private Mock<IProjectionService> _mockProjectionService;
        private ReservationDomainModel _reservationDomainModel;
        private CreateReservationResultModel _reservationResultModel;
        private CreateReservationModel _createReservationModel;
        private CreateReservationResultModel _failedReservationResultModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _reservationDomainModel = new ReservationDomainModel
            {
                ProjectionId = Guid.NewGuid(),
                SeatId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            _reservationResultModel = new CreateReservationResultModel
            {
                ErrorMessage = null,
                IsSuccessful = true,
                Reservation = _reservationDomainModel
            };

            _failedReservationResultModel = new CreateReservationResultModel
            {
                ErrorMessage = "Error",
                IsSuccessful = false,
                Reservation = null
            };

            _createReservationModel = new CreateReservationModel
            {
                ProjectionId = _reservationDomainModel.ProjectionId,
                SeatIds = new List<Guid>
                {
                    _reservationDomainModel.SeatId
                },
                UserId = _reservationDomainModel.UserId
            };

            List<ReservationDomainModel> reservationList = new List<ReservationDomainModel>();
            reservationList.Add(_reservationDomainModel);
            IEnumerable<ReservationDomainModel> reservations = reservationList;
            Task<IEnumerable<ReservationDomainModel>> responseTask = Task.FromResult(reservations);

            _mockReservationService = new Mock<IReservationService>();
            _mockUserService = new Mock<IUserService>();
            _mockProjectionService = new Mock<IProjectionService>();
        }

        [TestMethod]
        public void ReservationsController_GetAllReservations_ReturnsListOfReservations()
        {
            //Arrange
            List<ReservationDomainModel> reservationList = new List<ReservationDomainModel>();
            reservationList.Add(_reservationDomainModel);
            IEnumerable<ReservationDomainModel> reservations = reservationList;
            Task<IEnumerable<ReservationDomainModel>> responseTask = Task.FromResult(reservations);
            int expectedCount = 1;
            int expectedStatusCode = 200;

            ReservationsController reservationsController = new ReservationsController(_mockReservationService.Object, _mockUserService.Object, _mockProjectionService.Object);

            _mockReservationService.Setup(x => x.GetAllReservations()).Returns(responseTask);

            //Act
            var result = reservationsController.GetAllReservations().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var reservationDomainModelResultList = (List<ReservationDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(reservationDomainModelResultList.Count, expectedCount);
            Assert.IsInstanceOfType(reservationDomainModelResultList[0], typeof(ReservationDomainModel));
            Assert.AreEqual(reservationDomainModelResultList[0].ProjectionId, _reservationDomainModel.ProjectionId);
            Assert.AreEqual(reservationDomainModelResultList[0].SeatId, _reservationDomainModel.SeatId);
            Assert.AreEqual(reservationDomainModelResultList[0].UserId, _reservationDomainModel.UserId);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void MovieTagsController_GetAllReservations_ReservationsReturnsNull_ReturnError()
        {
            //Arrange
            IEnumerable<ReservationDomainModel> nullDomainModel = null;
            Task<IEnumerable<ReservationDomainModel>> nullTask = Task.FromResult(nullDomainModel);

            string expectedMessage = Messages.RESERVATION_GET_ALL_ERROR;
            int expectedStatusCode = 500;

            ErrorResponseModel errorResponseModel = new ErrorResponseModel();
            errorResponseModel.ErrorMessage = expectedMessage;
            errorResponseModel.StatusCode = System.Net.HttpStatusCode.InternalServerError;

            ReservationsController reservationsController = new ReservationsController(_mockReservationService.Object, _mockUserService.Object, _mockProjectionService.Object);

            _mockReservationService.Setup(x => x.GetAllReservations()).Returns(nullTask);

            //Act
            var result = reservationsController.GetAllReservations().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            ObjectResult viewResult = (ObjectResult)result;
            var viewMessage = (ErrorResponseModel)viewResult.Value;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(viewResult.Value.ToString(), errorResponseModel.ToString());
            Assert.AreEqual(errorResponseModel.ErrorMessage, viewMessage.ErrorMessage);
            Assert.AreEqual(viewResult.StatusCode, expectedStatusCode);
        }

        [TestMethod]
        public void ReservationsController_GetByProjectionId_ReturnsListOfReservations()
        {
            //Arrange
            List<ReservationDomainModel> reservationList = new List<ReservationDomainModel>();
            reservationList.Add(_reservationDomainModel);
            IEnumerable<ReservationDomainModel> reservations = reservationList;
            Task<IEnumerable<ReservationDomainModel>> responseTask = Task.FromResult(reservations);
            int expectedCount = 1;
            int expectedStatusCode = 200;

            ReservationsController reservationsController = new ReservationsController(_mockReservationService.Object, _mockUserService.Object, _mockProjectionService.Object);

            _mockReservationService.Setup(x => x.GetByProjectionId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var result = reservationsController.GetAllByProjectionId(_reservationDomainModel.ProjectionId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var reservationDomainModelResultList = (List<ReservationDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(reservationDomainModelResultList.Count, expectedCount);
            Assert.IsInstanceOfType(reservationDomainModelResultList[0], typeof(ReservationDomainModel));
            Assert.AreEqual(reservationDomainModelResultList[0].ProjectionId, _reservationDomainModel.ProjectionId);
            Assert.AreEqual(reservationDomainModelResultList[0].SeatId, _reservationDomainModel.SeatId);
            Assert.AreEqual(reservationDomainModelResultList[0].UserId, _reservationDomainModel.UserId);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void ReservationsController_GetByProjectionId_ReservationServiceReturnsNull()
        {
            //Arrange
            IEnumerable<ReservationDomainModel> reservations = null;
            Task<IEnumerable<ReservationDomainModel>> responseTask = Task.FromResult(reservations);

            string expectedMessage = Messages.RESERVATION_GET_ALL_BY_PROJECTIONID_ERROR;
            int expectedStatusCode = 500;
            ErrorResponseModel errorResponseModel = new ErrorResponseModel();
            errorResponseModel.ErrorMessage = expectedMessage;
            errorResponseModel.StatusCode = System.Net.HttpStatusCode.InternalServerError;

            ReservationsController reservationsController = new ReservationsController(_mockReservationService.Object, _mockUserService.Object, _mockProjectionService.Object);

            _mockReservationService.Setup(x => x.GetByProjectionId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var result = reservationsController.GetAllByProjectionId(_reservationDomainModel.ProjectionId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var viewResult = (ObjectResult)result;
            var viewMessage = (ErrorResponseModel)viewResult.Value;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(viewResult.Value.ToString(), errorResponseModel.ToString());
            Assert.AreEqual(errorResponseModel.ErrorMessage, viewMessage.ErrorMessage);
            Assert.AreEqual(viewResult.StatusCode, expectedStatusCode);
        }

        [TestMethod]
        public void ReservationsController_GetByProjectionIdUserId_ReturnsListOfReservations()
        {
            //Arrange
            List<ReservationDomainModel> reservationList = new List<ReservationDomainModel>();
            reservationList.Add(_reservationDomainModel);
            IEnumerable<ReservationDomainModel> reservations = reservationList;
            Task<IEnumerable<ReservationDomainModel>> responseTask = Task.FromResult(reservations);
            int expectedCount = 1;
            int expectedStatusCode = 200;

            ReservationsController reservationsController = new ReservationsController(_mockReservationService.Object, _mockUserService.Object, _mockProjectionService.Object);

            _mockReservationService.Setup(x => x.GetByProjectionIdUserId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var result = reservationsController.GetByProjectionIdUserId(_reservationDomainModel.ProjectionId, _reservationDomainModel.UserId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var reservationDomainModelResultList = (List<ReservationDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(reservationDomainModelResultList.Count, expectedCount);
            Assert.IsInstanceOfType(reservationDomainModelResultList[0], typeof(ReservationDomainModel));
            Assert.AreEqual(reservationDomainModelResultList[0].ProjectionId, _reservationDomainModel.ProjectionId);
            Assert.AreEqual(reservationDomainModelResultList[0].SeatId, _reservationDomainModel.SeatId);
            Assert.AreEqual(reservationDomainModelResultList[0].UserId, _reservationDomainModel.UserId);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void ReservationsController_GetByProjectionIdUserId_ReservationServiceReturnsNull()
        {
            //Arrange
            IEnumerable<ReservationDomainModel> reservations = null;
            Task<IEnumerable<ReservationDomainModel>> responseTask = Task.FromResult(reservations);

            string expectedMessage = Messages.RESERVATION_GET_ALL_BY_PROJECTIONID_USERID_ERROR;
            int expectedStatusCode = 500;
            ErrorResponseModel errorResponseModel = new ErrorResponseModel();
            errorResponseModel.ErrorMessage = expectedMessage;
            errorResponseModel.StatusCode = System.Net.HttpStatusCode.InternalServerError;

            ReservationsController reservationsController = new ReservationsController(_mockReservationService.Object, _mockUserService.Object, _mockProjectionService.Object);

            _mockReservationService.Setup(x => x.GetByProjectionIdUserId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var result = reservationsController.GetByProjectionIdUserId(_reservationDomainModel.ProjectionId, _reservationDomainModel.UserId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var viewResult = (ObjectResult)result;
            var viewMessage = (ErrorResponseModel)viewResult.Value;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(viewResult.Value.ToString(), errorResponseModel.ToString());
            Assert.AreEqual(errorResponseModel.ErrorMessage, viewMessage.ErrorMessage);
            Assert.AreEqual(viewResult.StatusCode, expectedStatusCode);
        }

        [TestMethod]
        public void ReservationsController_GetByUserId_ReturnsListOfReservations()
        {
            //Arrange
            List<ReservationDomainModel> reservationList = new List<ReservationDomainModel>();
            reservationList.Add(_reservationDomainModel);
            IEnumerable<ReservationDomainModel> reservations = reservationList;
            Task<IEnumerable<ReservationDomainModel>> responseTask = Task.FromResult(reservations);
            int expectedCount = 1;
            int expectedStatusCode = 200;

            ReservationsController reservationsController = new ReservationsController(_mockReservationService.Object, _mockUserService.Object, _mockProjectionService.Object);

            _mockReservationService.Setup(x => x.GetByUserId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var result = reservationsController.GetByUserId(_reservationDomainModel.UserId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var reservationDomainModelResultList = (List<ReservationDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(reservationDomainModelResultList.Count, expectedCount);
            Assert.IsInstanceOfType(reservationDomainModelResultList[0], typeof(ReservationDomainModel));
            Assert.AreEqual(reservationDomainModelResultList[0].ProjectionId, _reservationDomainModel.ProjectionId);
            Assert.AreEqual(reservationDomainModelResultList[0].SeatId, _reservationDomainModel.SeatId);
            Assert.AreEqual(reservationDomainModelResultList[0].UserId, _reservationDomainModel.UserId);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void ReservationsController_GetByUserId_ReservationServiceReturnsNull()
        {
            //Arrange
            IEnumerable<ReservationDomainModel> reservations = null;
            Task<IEnumerable<ReservationDomainModel>> responseTask = Task.FromResult(reservations);

            string expectedMessage = Messages.RESERVATION_GET_ALL_BY_USERID_ERROR;
            int expectedStatusCode = 500;
            ErrorResponseModel errorResponseModel = new ErrorResponseModel();
            errorResponseModel.ErrorMessage = expectedMessage;
            errorResponseModel.StatusCode = System.Net.HttpStatusCode.InternalServerError;

            ReservationsController reservationsController = new ReservationsController(_mockReservationService.Object, _mockUserService.Object, _mockProjectionService.Object);

            _mockReservationService.Setup(x => x.GetByUserId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var result = reservationsController.GetByUserId(_reservationDomainModel.UserId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var viewResult = (ObjectResult)result;
            var viewMessage = (ErrorResponseModel)viewResult.Value;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(viewResult.Value.ToString(), errorResponseModel.ToString());
            Assert.AreEqual(errorResponseModel.ErrorMessage, viewMessage.ErrorMessage);
            Assert.AreEqual(viewResult.StatusCode, expectedStatusCode);
        }

        [TestMethod]
        public void ReservationsController_DeleteByUserIdProjectionId_ReturnsListOfDeletedReservations()
        {
            //Arrange
            List<ReservationDomainModel> reservationList = new List<ReservationDomainModel>();
            reservationList.Add(_reservationDomainModel);
            IEnumerable<ReservationDomainModel> reservations = reservationList;
            Task<IEnumerable<ReservationDomainModel>> responseTask = Task.FromResult(reservations);
            int expectedCount = 1;
            int expectedStatusCode = 202;

            ReservationsController reservationsController = new ReservationsController(_mockReservationService.Object, _mockUserService.Object, _mockProjectionService.Object);

            _mockReservationService.Setup(x => x.DeleteByUserIdProjectionId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var result = reservationsController.DeleteByUserIdProjectionId(_reservationDomainModel.UserId, _reservationDomainModel.ProjectionId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((AcceptedResult)result).Value;
            var reservationDomainModelResultList = (List<ReservationDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(reservationDomainModelResultList.Count, expectedCount);
            Assert.IsInstanceOfType(reservationDomainModelResultList[0], typeof(ReservationDomainModel));
            Assert.AreEqual(reservationDomainModelResultList[0].ProjectionId, _reservationDomainModel.ProjectionId);
            Assert.AreEqual(reservationDomainModelResultList[0].SeatId, _reservationDomainModel.SeatId);
            Assert.AreEqual(reservationDomainModelResultList[0].UserId, _reservationDomainModel.UserId);
            Assert.AreEqual(expectedStatusCode, ((AcceptedResult)result).StatusCode);
        }

        [TestMethod]
        public void ReservationsController_DeleteByUserIdProjectionId_ThrowsDbUpdateException()
        {
            //Arrange
            string expectedMessage = "Inner exception error message.";
            int expectedStatusCode = 400;
            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);

            ReservationsController reservationsController = new ReservationsController(_mockReservationService.Object, _mockUserService.Object, _mockProjectionService.Object);

            _mockReservationService.Setup(x => x.DeleteByUserIdProjectionId(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(dbUpdateException);

            //Act

            var result = reservationsController.DeleteByUserIdProjectionId(_reservationDomainModel.UserId, _reservationDomainModel.ProjectionId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
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
        public void ReservationsController_DeleteByUserIdProjectionId_ReservationServiceReturnsNull()
        {
            //Arrange
            IEnumerable<ReservationDomainModel> reservations = null;
            Task<IEnumerable<ReservationDomainModel>> responseTask = Task.FromResult(reservations);

            string expectedMessage = Messages.RESERVATION_DOES_NOT_EXIST;
            int expectedStatusCode = 500;
            ErrorResponseModel errorResponseModel = new ErrorResponseModel();
            errorResponseModel.ErrorMessage = expectedMessage;
            errorResponseModel.StatusCode = System.Net.HttpStatusCode.InternalServerError;

            ReservationsController reservationsController = new ReservationsController(_mockReservationService.Object, _mockUserService.Object, _mockProjectionService.Object);

            _mockReservationService.Setup(x => x.DeleteByUserIdProjectionId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var result = reservationsController.DeleteByUserIdProjectionId(_reservationDomainModel.UserId, _reservationDomainModel.ProjectionId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var viewResult = (ObjectResult)result;
            var viewMessage = (ErrorResponseModel)viewResult.Value;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(viewResult.Value.ToString(), errorResponseModel.ToString());
            Assert.AreEqual(errorResponseModel.ErrorMessage, viewMessage.ErrorMessage);
            Assert.AreEqual(viewResult.StatusCode, expectedStatusCode);
        }



        [TestMethod]
        public void ReservationsController_DeleteByProjectionId_ReturnsListOfDeletedReservations()
        {
            //Arrange
            List<ReservationDomainModel> reservationList = new List<ReservationDomainModel>();
            reservationList.Add(_reservationDomainModel);
            IEnumerable<ReservationDomainModel> reservations = reservationList;
            Task<IEnumerable<ReservationDomainModel>> responseTask = Task.FromResult(reservations);
            int expectedCount = 1;
            int expectedStatusCode = 202;

            ReservationsController reservationsController = new ReservationsController(_mockReservationService.Object, _mockUserService.Object, _mockProjectionService.Object);

            _mockReservationService.Setup(x => x.DeleteByProjectionId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var result = reservationsController.DeleteByProjectionId(_reservationDomainModel.ProjectionId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((AcceptedResult)result).Value;
            var reservationDomainModelResultList = (List<ReservationDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(reservationDomainModelResultList.Count, expectedCount);
            Assert.IsInstanceOfType(reservationDomainModelResultList[0], typeof(ReservationDomainModel));
            Assert.AreEqual(reservationDomainModelResultList[0].ProjectionId, _reservationDomainModel.ProjectionId);
            Assert.AreEqual(reservationDomainModelResultList[0].SeatId, _reservationDomainModel.SeatId);
            Assert.AreEqual(reservationDomainModelResultList[0].UserId, _reservationDomainModel.UserId);
            Assert.AreEqual(expectedStatusCode, ((AcceptedResult)result).StatusCode);
        }

        [TestMethod]
        public void ReservationsController_DeleteByProjectionId_ThrowsDbUpdateException()
        {
            //Arrange
            string expectedMessage = "Inner exception error message.";
            int expectedStatusCode = 400;
            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);

            ReservationsController reservationsController = new ReservationsController(_mockReservationService.Object, _mockUserService.Object, _mockProjectionService.Object);

            _mockReservationService.Setup(x => x.DeleteByProjectionId(It.IsAny<Guid>())).Throws(dbUpdateException);

            //Act

            var result = reservationsController.DeleteByProjectionId(_reservationDomainModel.ProjectionId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
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
        public void ReservationsController_DeleteByProjectionId_ReservationServiceReturnsNull()
        {
            //Arrange
            IEnumerable<ReservationDomainModel> reservations = null;
            Task<IEnumerable<ReservationDomainModel>> responseTask = Task.FromResult(reservations);

            string expectedMessage = Messages.RESERVATION_DOES_NOT_EXIST;
            int expectedStatusCode = 500;
            ErrorResponseModel errorResponseModel = new ErrorResponseModel();
            errorResponseModel.ErrorMessage = expectedMessage;
            errorResponseModel.StatusCode = System.Net.HttpStatusCode.InternalServerError;

            ReservationsController reservationsController = new ReservationsController(_mockReservationService.Object, _mockUserService.Object, _mockProjectionService.Object);

            _mockReservationService.Setup(x => x.DeleteByProjectionId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var result = reservationsController.DeleteByProjectionId(_reservationDomainModel.ProjectionId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var viewResult = (ObjectResult)result;
            var viewMessage = (ErrorResponseModel)viewResult.Value;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(viewResult.Value.ToString(), errorResponseModel.ToString());
            Assert.AreEqual(errorResponseModel.ErrorMessage, viewMessage.ErrorMessage);
            Assert.AreEqual(viewResult.StatusCode, expectedStatusCode);
        }

        [TestMethod]
        public void ReservationsController_Post_ReturnsCreated()
        {
            //Arrange

            List<ReservationDomainModel> reservationList = new List<ReservationDomainModel>();
            reservationList.Add(_reservationDomainModel);
            IEnumerable<ReservationDomainModel> reservations = reservationList;
            Task<IEnumerable<ReservationDomainModel>> responseTask = Task.FromResult(reservations);
            int expectedStatusCode = 201;
            int expectedCount = 1;

            Task<CreateReservationResultModel> createReservationResultModel = Task.FromResult(_reservationResultModel);

            ReservationsController reservationsController = new ReservationsController(_mockReservationService.Object, _mockUserService.Object, _mockProjectionService.Object);

            _mockReservationService.Setup(x => x.AddReservation(It.IsAny<ReservationDomainModel>())).Returns(createReservationResultModel);

            //Act
            var result = reservationsController.PostAsync(_createReservationModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultMessage = ((CreatedResult)result);
            var resultModel = (List<ReservationDomainModel>)resultMessage.Value;

            //Assert
            Assert.IsNotNull(resultModel);
            Assert.AreEqual(resultModel.Count, expectedCount);
            Assert.AreEqual(resultMessage.StatusCode, expectedStatusCode);
            Assert.IsInstanceOfType(resultModel[0], typeof(ReservationDomainModel));
            Assert.AreEqual(resultModel[0].ProjectionId, _createReservationModel.ProjectionId);
            Assert.AreEqual(expectedCount, _createReservationModel.SeatIds.Count);
            Assert.AreEqual(resultModel[0].UserId, _createReservationModel.UserId);
        }

        [TestMethod]
        public void ReservationsController_Post_InvalidModelState()
        {
            //Arrange
            string expectedMessage = "Invalid Model State";
            int expectedStatusCode = 400;

            ReservationsController reservationsController = new ReservationsController(_mockReservationService.Object, _mockUserService.Object, _mockProjectionService.Object);
            reservationsController.ModelState.AddModelError("key", "Invalid Model State");

            //Act
            var result = reservationsController.PostAsync(_createReservationModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
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
        public void ReservationsController_Post_ThrowsDbUpdateException()
        {
            //Arrange
            string expectedMessage = "Inner exception error message.";
            int expectedStatusCode = 400;
            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);

            ReservationsController reservationsController = new ReservationsController(_mockReservationService.Object, _mockUserService.Object, _mockProjectionService.Object);

            _mockReservationService.Setup(x => x.AddReservation(It.IsAny<ReservationDomainModel>())).Throws(dbUpdateException);

            //Act

            var result = reservationsController.PostAsync(_createReservationModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
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
        public void ReservationsController_Post_UnsuccessfulCreation()
        {
            //Arrange
            int expectedStatusCode = 400;
            int expectedCount = 1;
            string expectedMessage = "Error";

            Task<CreateReservationResultModel> createReservationResultModel = Task.FromResult(_failedReservationResultModel);

            ReservationsController reservationsController = new ReservationsController(_mockReservationService.Object, _mockUserService.Object, _mockProjectionService.Object);

            _mockReservationService.Setup(x => x.AddReservation(It.IsAny<ReservationDomainModel>())).Returns(createReservationResultModel);

            //Act
            var result = reservationsController.PostAsync(_createReservationModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
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
