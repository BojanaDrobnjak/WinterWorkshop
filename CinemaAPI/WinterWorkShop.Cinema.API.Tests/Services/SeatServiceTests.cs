using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SeatServiceTests
    {
        private Mock<ISeatsRepository> _mockSeatsRepository;
        private Seat _seat;
        private SeatDomainModel _seatDomainModel;
        private Mock<IReservationService> _mockReservationService;
        private ReservationDomainModel _reservationDomainModel;
        private NumberOfSeatsModel _numberOfSeatsModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _seat = new Seat
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Row = 2,
                Number = 2
            };

            _seatDomainModel = new SeatDomainModel
            {
                Id = _seat.Id,
                AuditoriumId = 1,
                Row = 2,
                Number = 2
            };

            _reservationDomainModel = new ReservationDomainModel
            {
                ProjectionId = Guid.NewGuid(),
                SeatId = _seat.Id,
                UserId = Guid.NewGuid()
            };

            _numberOfSeatsModel = new NumberOfSeatsModel
            {
                MaxNumber = 2,
                MaxRow = 2
            };


            List<Seat> seatsModelsList = new List<Seat>();
            seatsModelsList.Add(_seat);
            IEnumerable<Seat> seats = seatsModelsList;
            Task<IEnumerable<Seat>> responseTask = Task.FromResult(seats);

            _mockSeatsRepository = new Mock<ISeatsRepository>();

            List<ReservationDomainModel> reservationDomainModelsList = new List<ReservationDomainModel>();

            reservationDomainModelsList.Add(_reservationDomainModel);
            IEnumerable<ReservationDomainModel> reservations = reservationDomainModelsList;
            Task<IEnumerable<ReservationDomainModel>> reservationResponseTask = Task.FromResult(reservations);

            _mockReservationService = new Mock<IReservationService>();
        }

        [TestMethod]
        public void SeatService_GetAllAsync_ReturnsListOfAllSeats()
        {
            //Arrange
            List<Seat> seatsModelsList = new List<Seat>();
            seatsModelsList.Add(_seat);
            IEnumerable<Seat> seats = seatsModelsList;
            Task<IEnumerable<Seat>> responseTask = Task.FromResult(seats);
            int expectedCount = 1;
            SeatService seatService = new SeatService(_mockSeatsRepository.Object, _mockReservationService.Object);

            _mockSeatsRepository.Setup(x => x.GetAll()).Returns(responseTask);

            //Act
            var result = seatService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult().ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.IsInstanceOfType(result[0], typeof(SeatDomainModel));
            Assert.AreEqual(result[0].Id, _seat.Id);
        }

        [TestMethod]
        public void SeatService_AddSeat_ReturnsInsertedSeat()
        {
            //Arrange
            SeatService seatService = new SeatService(_mockSeatsRepository.Object, _mockReservationService.Object);

            _mockSeatsRepository.Setup(x => x.Insert(It.IsAny<Seat>())).Returns(_seat);


            //Act
            var result = seatService.AddSeat(_seatDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(SeatDomainModel));
            Assert.AreEqual(result.Id, _seat.Id);
        }

        [TestMethod]
        public void SeatService_AddSeat_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            Seat nullSeat = null;

            SeatService seatService = new SeatService(_mockSeatsRepository.Object, _mockReservationService.Object);

            _mockSeatsRepository.Setup(x => x.Insert(It.IsAny<Seat>())).Returns(nullSeat);

            //Act
            var result = seatService.AddSeat(_seatDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert
            Assert.IsNull(result);
        }


        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void SeatService_AddSeat_ThrowsDbUpdateException()
        {
            //Arrange
            SeatService seatService = new SeatService(_mockSeatsRepository.Object, _mockReservationService.Object);

            _mockSeatsRepository.Setup(x => x.Insert(It.IsAny<Seat>())).Throws(new DbUpdateException());


            //Act
            var result = seatService.AddSeat(_seatDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void SeatService_DeleteSeat_ReturnsDeletedSeat()
        {
            //Arrange
            SeatService seatService = new SeatService(_mockSeatsRepository.Object, _mockReservationService.Object);

            _mockSeatsRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(_seat);

            //Act

            var result = seatService.DeleteSeat(_seat.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(SeatDomainModel));
            Assert.AreEqual(result.Id, _seat.Id);

        }

        [TestMethod]
        public void SeatService_DeleteSeat_ReservationServiceReturnsNull_ReturnsNull()
        {
            //Arrange

            IEnumerable<ReservationDomainModel> reservations = null;
            Task<IEnumerable<ReservationDomainModel>> reservationResponseTask = Task.FromResult(reservations);

            _mockReservationService = new Mock<IReservationService>();
            _mockReservationService.Setup(x => x.DeleteBySeatId(It.IsAny<Guid>())).Returns(reservationResponseTask);

            SeatService seatService = new SeatService(_mockSeatsRepository.Object, _mockReservationService.Object);

            _mockSeatsRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(_seat);

            //Act
            var result = seatService.DeleteSeat(_seat.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void SeatService_DeleteSeat_SeatRepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            Seat nullSeat = null;

            SeatService seatService = new SeatService(_mockSeatsRepository.Object, _mockReservationService.Object);

            _mockSeatsRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(nullSeat);

            //Act
            var result = seatService.DeleteSeat(_seat.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }
        [TestMethod]
        public void SeatService_GetSeatsByAuditoriumId_ReturnsListOfSeats()
        {
            //Arrange
            List<Seat> seatsModelsList = new List<Seat>();
            seatsModelsList.Add(_seat);
            IEnumerable<Seat> seats = seatsModelsList;
            int expectedCount = 1;

            SeatService seatService = new SeatService(_mockSeatsRepository.Object, _mockReservationService.Object);

            _mockSeatsRepository.Setup(x => x.GetByAuditoriumId(It.IsAny<int>())).Returns(seats);

            //Act
            var result = seatService.GetSeatsByAuditoriumId(_seat.AuditoriumId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.IsInstanceOfType(result[0], typeof(SeatDomainModel));
            Assert.AreEqual(result[0].Id, _seat.Id);
            Assert.AreEqual(result[0].AuditoriumId, _seat.AuditoriumId);
        }

        [TestMethod]
        public void SeatService_GetSeatsByAuditoriumId_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            List<Seat> seatsModelsList = null;
            IEnumerable<Seat> seats = seatsModelsList;

            SeatService seatService = new SeatService(_mockSeatsRepository.Object, _mockReservationService.Object);

            _mockSeatsRepository.Setup(x => x.GetByAuditoriumId(It.IsAny<int>())).Returns(seats);

            //Act
            var result = seatService.GetSeatsByAuditoriumId(_seat.AuditoriumId).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void SeatService_GetSeatByAuditoriumIdRowSeatNum_ReturnsSeat()
        {
            //Arrange
            Task<Seat> seat = Task.FromResult(_seat);

            SeatService seatService = new SeatService(_mockSeatsRepository.Object, _mockReservationService.Object);

            _mockSeatsRepository.Setup(x => x.GetByAuditoriumIdRowColumn(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(seat);

            // Act
            var result = seatService.GetSeatByAuditoriumIdRowSeatnum(_seat.AuditoriumId, _seat.Row, _seat.Number).ConfigureAwait(false).GetAwaiter().GetResult();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(SeatDomainModel));
            Assert.AreEqual(result.Id, _seat.Id);
        }

        [TestMethod]
        public void SeatService_GetSeatByAuditoriumIdRowSeatNum_RepositoryReturnsNull()
        {
            //Arrange
            Seat nullSeat = null;
            Task<Seat> seat = Task.FromResult(nullSeat);

            SeatService seatService = new SeatService(_mockSeatsRepository.Object, _mockReservationService.Object);

            _mockSeatsRepository.Setup(x => x.GetByAuditoriumIdRowColumn(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(seat);

            //Act
            var result = seatService.GetSeatByAuditoriumIdRowSeatnum(_seat.AuditoriumId, _seat.Row, _seat.Number).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }


        [TestMethod]
        public void SeatService_GetNumberOfSeats_ReturnsNumberOfSeatsModel()
        {
            //Arrange
            List<int> numberOfSeats = new List<int> {2, 2};

            SeatService seatService = new SeatService(_mockSeatsRepository.Object, _mockReservationService.Object);

            _mockSeatsRepository.Setup(x => x.GetMaxValuesByAuditoriumId(It.IsAny<int>())).Returns(numberOfSeats);

            //Act
            var result = seatService.GetNumberOfSeats(_seat.AuditoriumId).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NumberOfSeatsModel));
            Assert.AreEqual(result.MaxNumber, _numberOfSeatsModel.MaxNumber);
            Assert.AreEqual(result.MaxRow, _numberOfSeatsModel.MaxRow);
        }

        [TestMethod]
        public void SeatService_GetNumberOfSeats_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange

            List<int> numberOfSeats = null;

            SeatService seatService = new SeatService(_mockSeatsRepository.Object, _mockReservationService.Object);

            _mockSeatsRepository.Setup(x => x.GetMaxValuesByAuditoriumId(It.IsAny<int>())).Returns(numberOfSeats);

            //Act
            var result = seatService.GetNumberOfSeats(_seat.AuditoriumId).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);

        }

        [TestMethod]
        public void SeatService_DeleteByAuditoriumId_ReturnsListOfDeletedSeats()
        {
            //Arrange
            List<Seat> seatsModelsList = new List<Seat>();
            seatsModelsList.Add(_seat);
            IEnumerable<Seat> seats = seatsModelsList;
            Task<IEnumerable<Seat>> responseTask = Task.FromResult(seats);
            int expectedCount = 1;

            SeatService seatService = new SeatService(_mockSeatsRepository.Object, _mockReservationService.Object);

            _mockSeatsRepository.Setup(x => x.DeleteByAuditoriumId(It.IsAny<int>())).Returns(responseTask);

            //Act
            var result = seatService.DeleteByAuditoriumId(_seat.AuditoriumId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result[0], typeof(SeatDomainModel));
            Assert.AreEqual(expectedCount, result.Count);
            Assert.AreEqual(result[0].Id, _seat.Id);
        }

        [TestMethod]
        public void SeatService_DeleteByAuditoriumId_SeatsRepository_GetByAuditoriumId_ReturnsNull_ReturnsNull()
        {
            //Arrange
            IEnumerable<Seat> seats = null;

            SeatService seatService = new SeatService(_mockSeatsRepository.Object, _mockReservationService.Object);

            _mockSeatsRepository.Setup(x => x.GetByAuditoriumId(It.IsAny<int>())).Returns(seats);

            //Act
            var result = seatService.DeleteByAuditoriumId(_seat.AuditoriumId).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);

        }

        [TestMethod]
        public void SeatService_DeleteByAuditoriumId_ReservationRepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            List<Seat> seatsModelsList = new List<Seat>();
            seatsModelsList.Add(_seat);
            IEnumerable<Seat> seats = seatsModelsList;

            IEnumerable<ReservationDomainModel> reservations = null;
            Task<IEnumerable<ReservationDomainModel>> reservationResponseTask = Task.FromResult(reservations);

            SeatService seatService = new SeatService(_mockSeatsRepository.Object, _mockReservationService.Object);

            _mockSeatsRepository.Setup(x => x.GetByAuditoriumId(It.IsAny<int>())).Returns(seats);
            _mockReservationService.Setup(x => x.DeleteBySeatId(It.IsAny<Guid>())).Returns(reservationResponseTask);


            //Act
            var result = seatService.DeleteByAuditoriumId(_seat.AuditoriumId).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void SeatService_deleteByAuditoriumId_SeatsRepository_DeleteByAuditoriumId_ReturnsNull_ReturnsNull()
        {
            //Arrange
            List<Seat> seatsModelsList = new List<Seat>();
            seatsModelsList.Add(_seat);
            IEnumerable<Seat> seats = seatsModelsList;

            List<ReservationDomainModel> reservationDomainModelsList = new List<ReservationDomainModel>();
            reservationDomainModelsList.Add(_reservationDomainModel);
            IEnumerable<ReservationDomainModel> reservations = reservationDomainModelsList;
            Task<IEnumerable<ReservationDomainModel>> reservationResponseTask = Task.FromResult(reservations);

            IEnumerable<Seat> nullList = null;
            Task<IEnumerable<Seat>> nullSeats = Task.FromResult(nullList);

            _mockSeatsRepository.Setup(x => x.GetByAuditoriumId(It.IsAny<int>())).Returns(seats);
            _mockReservationService.Setup(x => x.DeleteBySeatId(It.IsAny<Guid>())).Returns(reservationResponseTask);
            _mockSeatsRepository.Setup(x => x.DeleteByAuditoriumId(It.IsAny<int>())).Returns(nullSeats);

            SeatService seatService = new SeatService(_mockSeatsRepository.Object, _mockReservationService.Object);


            //Act
            var result = seatService.DeleteByAuditoriumId(_seat.AuditoriumId).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);

        }
    }
}
