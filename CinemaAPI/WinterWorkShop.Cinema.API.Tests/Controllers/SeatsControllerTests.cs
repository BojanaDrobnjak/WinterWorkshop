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
    public class SeatsControllerTests
    {
        private Mock<ISeatService> _mockSeatService;
        private SeatDomainModel _seatDomainModel;
        private NumberOfSeatsModel _numberOfSeats;

        [TestInitialize]
        public void TestIni()
        {
            _seatDomainModel = new SeatDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Number = 1,
                Row = 1
            };

            _numberOfSeats = new NumberOfSeatsModel
            {
                MaxNumber = 1,
                MaxRow = 1
            };

            List<SeatDomainModel> seatList = new List<SeatDomainModel>();
            seatList.Add(_seatDomainModel);
            IEnumerable<SeatDomainModel> seats = seatList;
            Task<IEnumerable<SeatDomainModel>> responseTask = Task.FromResult(seats);

            _mockSeatService = new Mock<ISeatService>();

        }

        [TestMethod]
        public void SeatsController_GetAllSeats_ReturnsAllSeats()
        {
            //Arrange
            List<SeatDomainModel> seatList = new List<SeatDomainModel>();
            seatList.Add(_seatDomainModel);
            IEnumerable<SeatDomainModel> seats = seatList;
            Task<IEnumerable<SeatDomainModel>> responseTask = Task.FromResult(seats);
            int expectedCount = 1;
            int expectedStatusCode = 200;

            SeatsController seatsController = new SeatsController(_mockSeatService.Object);

            _mockSeatService.Setup(x => x.GetAllAsync()).Returns(responseTask);

            //Act
            var result = seatsController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var seatDomainModelResultList = (List<SeatDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(seatDomainModelResultList.Count, expectedCount);
            Assert.IsInstanceOfType(seatDomainModelResultList[0], typeof(SeatDomainModel));
            Assert.AreEqual(seatDomainModelResultList[0].Id, _seatDomainModel.Id);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void SeatsController_GetAllSeats_RepositoryReturnsEmptyList()
        {
            //Arrange
            IEnumerable<SeatDomainModel> seats = null;
            Task<IEnumerable<SeatDomainModel>> responseTask = Task.FromResult(seats);
            int expectedCount = 0;
            int expectedStatusCode = 200;

            SeatsController seatsController = new SeatsController(_mockSeatService.Object);

            _mockSeatService.Setup(x => x.GetAllAsync()).Returns(responseTask);

            //Act
            var result = seatsController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var seatDomainModelResultList = (List<SeatDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(seatDomainModelResultList.Count, expectedCount);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void SeatsController_GetByAuditoriumId_ReturnsAllSeats()
        {
            //Arrange
            List<SeatDomainModel> seatList = new List<SeatDomainModel>();
            seatList.Add(_seatDomainModel);
            IEnumerable<SeatDomainModel> seats = seatList;
            Task<IEnumerable<SeatDomainModel>> responseTask = Task.FromResult(seats);
            int expectedCount = 1;
            int expectedStatusCode = 200;

            SeatsController seatsController = new SeatsController(_mockSeatService.Object);

            _mockSeatService.Setup(x => x.GetSeatsByAuditoriumId(It.IsAny<int>())).Returns(responseTask);

            //Act
            var result = seatsController.GetByAuditoriumId(_seatDomainModel.AuditoriumId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var seatDomainModelResultList = (List<SeatDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(seatDomainModelResultList.Count, expectedCount);
            Assert.IsInstanceOfType(seatDomainModelResultList[0], typeof(SeatDomainModel));
            Assert.AreEqual(seatDomainModelResultList[0].AuditoriumId, _seatDomainModel.AuditoriumId);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }


        [TestMethod]
        public void SeatsController_GetAllSeatsByAuditoriumId_RepositoryReturnsEmptyList()
        {
            //Arrange
            IEnumerable<SeatDomainModel> seats = null;
            Task<IEnumerable<SeatDomainModel>> responseTask = Task.FromResult(seats);
            int expectedCount = 0;
            int expectedStatusCode = 200;

            SeatsController seatsController = new SeatsController(_mockSeatService.Object);

            _mockSeatService.Setup(x => x.GetSeatsByAuditoriumId(It.IsAny<int>())).Returns(responseTask);

            //Act
            var result = seatsController.GetByAuditoriumId(_seatDomainModel.AuditoriumId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var seatDomainModelResultList = (List<SeatDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(seatDomainModelResultList.Count, expectedCount);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void SeatsController_GetNumberOfSeats_ReturnsNumberOfSeatsModel()
        {
            //Arrange
            Task<NumberOfSeatsModel> numberOfSeats = Task.FromResult(_numberOfSeats);
            SeatsController seatsController = new SeatsController(_mockSeatService.Object);
            int expectedStatusCode = 200;

            _mockSeatService.Setup(x => x.GetNumberOfSeats(It.IsAny<int>())).Returns(numberOfSeats);

            //Act
            var result = seatsController.GetNumberOfSeats(_seatDomainModel.AuditoriumId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var seatDomainModelResultList = (NumberOfSeatsModel)resultList;

            //Assert
            Assert.IsNotNull(seatDomainModelResultList);
            Assert.IsInstanceOfType(seatDomainModelResultList, typeof(NumberOfSeatsModel));
            Assert.AreEqual(seatDomainModelResultList.MaxNumber, _numberOfSeats.MaxNumber);
            Assert.AreEqual(seatDomainModelResultList.MaxRow, _numberOfSeats.MaxRow);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void SeatsController_GetNumberOfSeats_SeatServiceReturnsNull_404()
        {
            //Arrange
            NumberOfSeatsModel nullNum = null;
            Task<NumberOfSeatsModel> numberOfSeats = Task.FromResult(nullNum);
            SeatsController seatsController = new SeatsController(_mockSeatService.Object);
            int expectedStatusCode = 404;
            string expectedMessage = Messages.AUDITORIUM_DOES_NOT_EXIST;

            _mockSeatService.Setup(x => x.GetNumberOfSeats(It.IsAny<int>())).Returns(numberOfSeats);

            //Act
            var result = seatsController.GetNumberOfSeats(_seatDomainModel.AuditoriumId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultResponse = ((NotFoundObjectResult)result);
            var badObjectResult = ((NotFoundObjectResult)result).Value;

            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, badObjectResult);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }

    }
}
