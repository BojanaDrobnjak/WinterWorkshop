using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using WinterWorkShop.Cinema.Repositories;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Data;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Services;
using System.Linq;
using WinterWorkShop.Cinema.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class ReservationServiceTests
    {
        private Mock<IReservationsRepository> _mockReservationsRepository;
        private Reservation _reservation;
        private ReservationDomainModel _reservationDomainModel;


        [TestInitialize]
        public void TestInitialize()
        {

            _reservation = new Reservation
            {
                ProjectionId = Guid.NewGuid(),
                SeatId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            _reservationDomainModel = new ReservationDomainModel
            {
                ProjectionId = _reservation.ProjectionId,
                SeatId = _reservation.SeatId,
                UserId = _reservation.UserId
            };

            List<Reservation> reservationList = new List<Reservation>();

            reservationList.Add(_reservation);
            IEnumerable<Reservation> reservations = reservationList;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            _mockReservationsRepository = new Mock<IReservationsRepository>();
        }

        [TestMethod]
        public void ReservationService_GetAllAsync_ReturnsListOfAllReservations()
        {
            //Arrange

            List<Reservation> reservationList = new List<Reservation>();

            reservationList.Add(_reservation);
            IEnumerable<Reservation> reservations = reservationList;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            int expectedResultCount = 1;
            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.GetAll()).Returns(responseTask);

            //Act
            var resultAction = reservationService.GetAllReservations().ConfigureAwait(false).GetAwaiter().GetResult().ToList();


            //Assert
            Assert.AreEqual(resultAction.Count, expectedResultCount);
            Assert.AreEqual(resultAction[0].ProjectionId, _reservation.ProjectionId);
            Assert.IsInstanceOfType(resultAction[0], typeof(ReservationDomainModel));
            Assert.IsNotNull(resultAction);

        }

        [TestMethod]
        public void ReservationService_GetAllAsync_RepositoryReturnsNull_ReturnsNull()
        {

            IEnumerable<Reservation> reservations = null;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            int expectedResultCount = 1;
            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.GetAll()).Returns(responseTask);

            //Act
            var resultAction = reservationService.GetAllReservations().ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert
            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void ReservationService_GetByProjectionId_ReturnsListOfReservations()
        {
            //Arrange

            List<Reservation> reservationList = new List<Reservation>();

            reservationList.Add(_reservation);
            IEnumerable<Reservation> reservations = reservationList;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            int expectedResultCount = 1;
            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.GetByProjectionId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.GetByProjectionId(_reservation.ProjectionId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();


            //Assert
            Assert.AreEqual(resultAction.Count, expectedResultCount);
            Assert.AreEqual(resultAction[0].ProjectionId, _reservation.ProjectionId);
            Assert.IsInstanceOfType(resultAction[0], typeof(ReservationDomainModel));
            Assert.IsNotNull(resultAction);
        }

        [TestMethod]
        public void ReservationService_GetByProjectionId_RepositoryReturnsNull_ReturnsNull()
        {
            IEnumerable<Reservation> reservations = null;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            int expectedResultCount = 1;
            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.GetByProjectionId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.GetByProjectionId(_reservation.ProjectionId).ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert
            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void ReservationService_GetByUserId_ReturnsListOfReservations()
        {
            //Arrange

            List<Reservation> reservationList = new List<Reservation>();

            reservationList.Add(_reservation);
            IEnumerable<Reservation> reservations = reservationList;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            int expectedResultCount = 1;
            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.GetByUserId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.GetByUserId(_reservation.ProjectionId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();


            //Assert
            Assert.AreEqual(resultAction.Count, expectedResultCount);
            Assert.AreEqual(resultAction[0].ProjectionId, _reservation.ProjectionId);
            Assert.IsInstanceOfType(resultAction[0], typeof(ReservationDomainModel));
            Assert.IsNotNull(resultAction);
        }

        [TestMethod]
        public void ReservationService_GetByUserId_RepositoryReturnsNull_ReturnsNull()
        {
            IEnumerable<Reservation> reservations = null;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            int expectedResultCount = 1;
            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.GetByUserId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.GetByUserId(_reservation.ProjectionId).ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert
            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void ReservationService_GetBySeatId_ReturnsListOfReservations()
        {
            //Arrange

            List<Reservation> reservationList = new List<Reservation>();

            reservationList.Add(_reservation);
            IEnumerable<Reservation> reservations = reservationList;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            int expectedResultCount = 1;
            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.GetBySeatId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.GetBySeatId(_reservation.ProjectionId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();


            //Assert
            Assert.AreEqual(resultAction.Count, expectedResultCount);
            Assert.AreEqual(resultAction[0].ProjectionId, _reservation.ProjectionId);
            Assert.IsInstanceOfType(resultAction[0], typeof(ReservationDomainModel));
            Assert.IsNotNull(resultAction);
        }

        [TestMethod]
        public void ReservationService_GetBySeatId_RepositoryReturnsNull_ReturnsNull()
        {
            IEnumerable<Reservation> reservations = null;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            int expectedResultCount = 1;
            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.GetBySeatId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.GetBySeatId(_reservation.ProjectionId).ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert
            Assert.IsNull(resultAction);
        }


        [TestMethod]
        public void ReservationService_GetByProjectionIdUserId_ReturnsListOfReservations()
        {
            //Arrange

            List<Reservation> reservationList = new List<Reservation>();

            reservationList.Add(_reservation);
            IEnumerable<Reservation> reservations = reservationList;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            int expectedResultCount = 1;
            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.GetByProjectionIdUserId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.GetByProjectionIdUserId(_reservation.ProjectionId, _reservation.UserId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();


            //Assert
            Assert.AreEqual(resultAction.Count, expectedResultCount);
            Assert.AreEqual(resultAction[0].ProjectionId, _reservation.ProjectionId);
            Assert.AreEqual(resultAction[0].UserId, _reservation.UserId);
            Assert.IsInstanceOfType(resultAction[0], typeof(ReservationDomainModel));
            Assert.IsNotNull(resultAction);
        }


        [TestMethod]
        public void ReservationService_GetByProjectionIdUserId_RepositoryReturnsNull_ReturnsNull()
        {
            IEnumerable<Reservation> reservations = null;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            int expectedResultCount = 1;
            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.GetByProjectionIdUserId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.GetByProjectionIdUserId(_reservation.ProjectionId, _reservation.UserId).ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert
            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void ReservationService_GetBySeatIdProjectionId_ReturnsListOfReservations()
        {
            //Arrange

            List<Reservation> reservationList = new List<Reservation>();

            reservationList.Add(_reservation);
            IEnumerable<Reservation> reservations = reservationList;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            int expectedResultCount = 1;
            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.GetBySeatIdProjectionId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.GetBySeatIdProjectionId(_reservation.SeatId, _reservation.ProjectionId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();


            //Assert
            Assert.AreEqual(resultAction.Count, expectedResultCount);
            Assert.AreEqual(resultAction[0].ProjectionId, _reservation.ProjectionId);
            Assert.AreEqual(resultAction[0].SeatId, _reservation.SeatId);
            Assert.IsInstanceOfType(resultAction[0], typeof(ReservationDomainModel));
            Assert.IsNotNull(resultAction);
        }


        [TestMethod]
        public void ReservationService_GetBySeatIdProjectionId_RepositoryReturnsNull_ReturnsNull()
        {
            IEnumerable<Reservation> reservations = null;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            int expectedResultCount = 1;
            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.GetBySeatIdProjectionId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.GetBySeatIdProjectionId(_reservation.SeatId, _reservation.ProjectionId).ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert
            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void ReservationService_GetBySeatIdUserId_ReturnsListOfReservations()
        {
            //Arrange

            List<Reservation> reservationList = new List<Reservation>();

            reservationList.Add(_reservation);
            IEnumerable<Reservation> reservations = reservationList;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            int expectedResultCount = 1;
            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.GetBySeatIdUserId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.GetBySeatIdUserId(_reservation.SeatId, _reservation.UserId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();


            //Assert
            Assert.AreEqual(resultAction.Count, expectedResultCount);
            Assert.AreEqual(resultAction[0].UserId, _reservation.UserId);
            Assert.AreEqual(resultAction[0].SeatId, _reservation.SeatId);
            Assert.IsInstanceOfType(resultAction[0], typeof(ReservationDomainModel));
            Assert.IsNotNull(resultAction);
        }

        [TestMethod]
        public void ReservationService_GetBySeatIdUserId_RepositoryReturnsNull_ReturnsNull()
        {
            IEnumerable<Reservation> reservations = null;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            int expectedResultCount = 1;
            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.GetBySeatIdUserId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.GetBySeatIdUserId(_reservation.SeatId, _reservation.UserId).ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert
            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void ReservationService_GetById_ReturnsListOfReservations()
        {
            //Arrange
            Task<Reservation> reservation = Task.FromResult(_reservation);

            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(reservation);

            //Act
            var result = reservationService.GetById(_reservation.SeatId,_reservation.ProjectionId, _reservation.UserId).ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ReservationDomainModel));
            Assert.AreEqual(_reservation.SeatId, result.SeatId);
            Assert.AreEqual(_reservation.ProjectionId, result.ProjectionId);
            Assert.AreEqual(_reservation.UserId, result.UserId);
        }

        [TestMethod]
        public void ReservationService_GetById_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            Reservation nullReservation = null;
            Task<Reservation> reservation = Task.FromResult(nullReservation);

            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(reservation);

            //Act
            var result = reservationService.GetById(_reservation.SeatId, _reservation.ProjectionId, _reservation.UserId).ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert

            Assert.IsNull(result);
        }

        [TestMethod]
        public void ReservationService_DeleteByProjectionId_ReturnsListOfDeletedReservations()
        {
            //Arrange

            List<Reservation> reservationList = new List<Reservation>();

            reservationList.Add(_reservation);
            IEnumerable<Reservation> reservations = reservationList;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            int expectedResultCount = 1;
            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.DeleteByProjectionId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.DeleteByProjectionId(_reservation.ProjectionId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();


            //Assert
            Assert.AreEqual(resultAction.Count, expectedResultCount);
            Assert.AreEqual(resultAction[0].ProjectionId, _reservation.ProjectionId);
            Assert.IsInstanceOfType(resultAction[0], typeof(ReservationDomainModel));
            Assert.IsNotNull(resultAction);
        }

        [TestMethod]
        public void ReservationService_DeleteByProjectionId_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            IEnumerable<Reservation> reservations = null;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.DeleteByProjectionId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.DeleteByProjectionId(_reservation.ProjectionId).ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert
            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void ReservationService_DeleteByUserId_ReturnsListOfDeletedReservations()
        {
            //Arrange

            List<Reservation> reservationList = new List<Reservation>();

            reservationList.Add(_reservation);
            IEnumerable<Reservation> reservations = reservationList;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            int expectedResultCount = 1;
            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.DeleteByUserId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.DeleteByUserId(_reservation.UserId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();


            //Assert
            Assert.AreEqual(resultAction.Count, expectedResultCount);
            Assert.AreEqual(resultAction[0].UserId, _reservation.UserId);
            Assert.IsInstanceOfType(resultAction[0], typeof(ReservationDomainModel));
            Assert.IsNotNull(resultAction);
        }

        [TestMethod]
        public void ReservationService_DeleteByUserId_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            IEnumerable<Reservation> reservations = null;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.DeleteByUserId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.DeleteByUserId(_reservation.UserId).ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert
            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void ReservationService_DeleteBySeatId_ReturnsListOfDeletedReservations()
        {
            //Arrange

            List<Reservation> reservationList = new List<Reservation>();

            reservationList.Add(_reservation);
            IEnumerable<Reservation> reservations = reservationList;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            int expectedResultCount = 1;
            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.DeleteBySeatId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.DeleteBySeatId(_reservation.SeatId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();


            //Assert
            Assert.AreEqual(resultAction.Count, expectedResultCount);
            Assert.AreEqual(resultAction[0].SeatId, _reservation.SeatId);
            Assert.IsInstanceOfType(resultAction[0], typeof(ReservationDomainModel));
            Assert.IsNotNull(resultAction);
        }

        [TestMethod]
        public void ReservationService_DeleteBySeatId_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            IEnumerable<Reservation> reservations = null;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.DeleteBySeatId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.DeleteBySeatId(_reservation.SeatId).ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert
            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void ReservationService_DeleteByUserIdProjectionId_ReturnsListOfDeletedReservations()
        {
            //Arrange

            List<Reservation> reservationList = new List<Reservation>();

            reservationList.Add(_reservation);
            IEnumerable<Reservation> reservations = reservationList;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            int expectedResultCount = 1;
            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.DeleteByUserIdProjectionId(It.IsAny<Guid>(),It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.DeleteByUserIdProjectionId(_reservation.UserId, _reservation.ProjectionId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();


            //Assert
            Assert.AreEqual(resultAction.Count, expectedResultCount);
            Assert.AreEqual(resultAction[0].UserId, _reservation.UserId);
            Assert.AreEqual(resultAction[0].ProjectionId, _reservation.ProjectionId);
            Assert.IsInstanceOfType(resultAction[0], typeof(ReservationDomainModel));
            Assert.IsNotNull(resultAction);
        }

        [TestMethod]
        public void ReservationService_DeleteByUserIdProjectionId_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            IEnumerable<Reservation> reservations = null;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.DeleteByUserIdProjectionId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.DeleteByUserIdProjectionId(_reservation.UserId, _reservation.ProjectionId).ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert
            Assert.IsNull(resultAction);
        }


        [TestMethod]
        public void ReservationService_DeleteBySeatIdProjectionId_ReturnsListOfDeletedReservations()
        {
            //Arrange

            List<Reservation> reservationList = new List<Reservation>();

            reservationList.Add(_reservation);
            IEnumerable<Reservation> reservations = reservationList;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            int expectedResultCount = 1;
            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.DeleteBySeatIdProjectionId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.DeleteBySeatIdProjectionId(_reservation.SeatId, _reservation.ProjectionId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();


            //Assert
            Assert.AreEqual(resultAction.Count, expectedResultCount);
            Assert.AreEqual(resultAction[0].SeatId, _reservation.SeatId);
            Assert.AreEqual(resultAction[0].ProjectionId, _reservation.ProjectionId);
            Assert.IsInstanceOfType(resultAction[0], typeof(ReservationDomainModel));
            Assert.IsNotNull(resultAction);
        }

        [TestMethod]
        public void ReservationService_DeleteBySeatIdProjectionId_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            IEnumerable<Reservation> reservations = null;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.DeleteBySeatIdProjectionId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.DeleteBySeatIdProjectionId(_reservation.SeatId, _reservation.ProjectionId).ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert
            Assert.IsNull(resultAction);
        }


        [TestMethod]
        public void ReservationService_DeleteBySeatIdUserId_ReturnsListOfDeletedReservations()
        {
            //Arrange

            List<Reservation> reservationList = new List<Reservation>();

            reservationList.Add(_reservation);
            IEnumerable<Reservation> reservations = reservationList;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            int expectedResultCount = 1;
            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.DeleteBySeatIdUserId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.DeleteBySeatIdUserId(_reservation.SeatId, _reservation.UserId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();


            //Assert
            Assert.AreEqual(resultAction.Count, expectedResultCount);
            Assert.AreEqual(resultAction[0].SeatId, _reservation.SeatId);
            Assert.AreEqual(resultAction[0].UserId, _reservation.UserId);
            Assert.IsInstanceOfType(resultAction[0], typeof(ReservationDomainModel));
            Assert.IsNotNull(resultAction);
        }

        [TestMethod]
        public void ReservationService_DeleteBySeatIdUserId_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            IEnumerable<Reservation> reservations = null;
            Task<IEnumerable<Reservation>> responseTask = Task.FromResult(reservations);

            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.DeleteBySeatIdUserId(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = reservationService.DeleteBySeatIdUserId(_reservation.SeatId, _reservation.UserId).ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert
            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void ReservationService_DeleteById_ReturnsDeletedReservation()
        {
            //Arrange
            Task<Reservation> reservation = Task.FromResult(_reservation);

            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.DeleteById(It.IsAny<Guid>(),It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(reservation);

            //Act
            var result = reservationService.DeleteById(_reservation.ProjectionId,_reservation.SeatId, _reservation.UserId).ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ReservationDomainModel));
            Assert.AreEqual(result.SeatId, _reservation.SeatId);
            Assert.AreEqual(result.UserId, _reservation.UserId);
            Assert.AreEqual(result.ProjectionId, _reservation.ProjectionId);
        }


        [TestMethod]
        public void ReservationService_DeleteById_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            Reservation nullReservation = null;
            Task<Reservation> reservation = Task.FromResult(nullReservation);

            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            _mockReservationsRepository.Setup(x => x.DeleteById(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(reservation);

            //Act
            var result = reservationService.DeleteById(_reservation.ProjectionId, _reservation.SeatId, _reservation.UserId).ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ReservationService_AddReservation_ReturnsCreateReservationResultModel()
        {
            //Arrange

            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);

            _mockReservationsRepository.Setup(x => x.Insert(It.IsAny<Reservation>())).Returns(_reservation);
            _mockReservationsRepository.Setup(x => x.Save());

            //Act
            var resultAction = reservationService.AddReservation(_reservationDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.IsInstanceOfType(resultAction, typeof(CreateReservationResultModel));
            Assert.IsTrue(resultAction.IsSuccessful);
            Assert.IsNull(resultAction.ErrorMessage);
            Assert.AreEqual(_reservation.ProjectionId, resultAction.Reservation.ProjectionId);
            Assert.AreEqual(_reservation.SeatId, resultAction.Reservation.SeatId);
            Assert.AreEqual(_reservation.UserId, resultAction.Reservation.UserId);

        }

        [TestMethod]
        public void ReservationService_AddReservation_RepositoryReturnsNull_ReturnUnseccessfulResultModel()
        {
            //Arrange

            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);
            Reservation reservation = null;

            _mockReservationsRepository.Setup(x => x.Insert(It.IsAny<Reservation>())).Returns(reservation);
            _mockReservationsRepository.Setup(x => x.Save());


            string expectedMessage = Messages.RESERVATION_CREATION_ERROR;
            //Act
            var resultAction = reservationService.AddReservation(_reservationDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.IsInstanceOfType(resultAction, typeof(CreateReservationResultModel));
            Assert.IsFalse(resultAction.IsSuccessful);
            Assert.IsNotNull(resultAction.ErrorMessage);
            Assert.AreEqual(expectedMessage, resultAction.ErrorMessage);
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void ReservationService_AddReservation_ThrowsDbUpdateException()
        {
            //Arrange

            ReservationService reservationService = new ReservationService(_mockReservationsRepository.Object);

            _mockReservationsRepository.Setup(x => x.Insert(It.IsAny<Reservation>())).Throws(new DbUpdateException());
            _mockReservationsRepository.Setup(x => x.Save());

            //Act
            var resultAction = reservationService.AddReservation(_reservationDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert

        }

    }
}
