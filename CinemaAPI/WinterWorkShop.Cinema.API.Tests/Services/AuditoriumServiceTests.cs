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
    public class AuditoriumServiceTests
    {
        private Mock<IAuditoriumsRepository> _mockAuditoriumsRepository;
        private Mock<ISeatService> _mockSeatService;
        private Mock<IProjectionService> _mockProjectionService;
        private Auditorium _auditorium;
        private AuditoriumDomainModel _auditoriumDomainModel;
        private ProjectionDomainModel _projectionDomainModel;
        private SeatDomainModel _seatDomainModel;

        [TestInitialize]
        public void TestInitialize()
        {

            _auditorium = new Auditorium
            {
                Id = 1,
                CinemaId = 1,
                Name = "AuditoriumIme",
                Cinema = new Data.Cinema { Name = "Bioskop"}
            };

            _auditoriumDomainModel = new AuditoriumDomainModel
            {
                Id = _auditorium.Id,
                CinemaId = _auditorium.CinemaId,
                Name = _auditorium.Name
            };

            _projectionDomainModel = new ProjectionDomainModel
            {
                AuditoriumId = _auditoriumDomainModel.Id,
                Id = Guid.NewGuid(),
                AuditoriumName = _auditoriumDomainModel.Name,
                MovieId = Guid.NewGuid(),
                MovieRating = 1,
                MovieTitle = "imefilmea",
                MovieYear = 1992,
                ProjectionTime = DateTime.Now.AddDays(1)
            };

            _seatDomainModel = new SeatDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumId = _auditoriumDomainModel.Id,
                Row = 1,
                Number = 1
            };

            List<Auditorium> auditoriumsModelsList = new List<Auditorium>();
            auditoriumsModelsList.Add(_auditorium);
            IEnumerable<Auditorium> auditoriums = auditoriumsModelsList;
            Task<IEnumerable<Auditorium>> responseTask = Task.FromResult(auditoriums);

            List<ProjectionDomainModel> projectionsModelsList = new List<ProjectionDomainModel>();
            projectionsModelsList.Add(_projectionDomainModel);
            IEnumerable<ProjectionDomainModel> projections = projectionsModelsList;
            Task<IEnumerable<ProjectionDomainModel>> projectionsResponseTask = Task.FromResult(projections);

            List<SeatDomainModel> seatsModelsList = new List<SeatDomainModel>();
            seatsModelsList.Add(_seatDomainModel);
            IEnumerable<SeatDomainModel> seats = seatsModelsList;
            Task<IEnumerable<SeatDomainModel>> seatsResponseTask = Task.FromResult(seats);

            _mockAuditoriumsRepository = new Mock<IAuditoriumsRepository>();
            _mockProjectionService = new Mock<IProjectionService>();
            _mockSeatService = new Mock<ISeatService>();
        }

        [TestMethod]
        public void AuditoriumService_GetAllAsync_ReturnsListOfAllAuditoriums()
        {
            //Arrange
            List<Auditorium> auditoriumsModelsList = new List<Auditorium>();
            auditoriumsModelsList.Add(_auditorium);
            IEnumerable<Auditorium> auditoriums = auditoriumsModelsList;
            Task<IEnumerable<Auditorium>> responseTask = Task.FromResult(auditoriums);
            int expectedCount = 1;

            AuditoriumService auditoriumService = new AuditoriumService(_mockAuditoriumsRepository.Object, _mockProjectionService.Object, _mockSeatService.Object);

            _mockAuditoriumsRepository.Setup(x => x.GetAll()).Returns(responseTask);

            //Act
            var result = auditoriumService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult().ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.IsInstanceOfType(result[0], typeof(AuditoriumDomainModel));
            Assert.AreEqual(result[0].Id, _auditorium.Id);
        }

        [TestMethod]
        public void AuditoriumService_GetAllAsync_RepositoryReturnsNull_ReturnNull()
        {
            //Arrange
            IEnumerable<Auditorium> auditoriums = null;
            Task<IEnumerable<Auditorium>> responseTask = Task.FromResult(auditoriums);

            AuditoriumService auditoriumService = new AuditoriumService(_mockAuditoriumsRepository.Object, _mockProjectionService.Object, _mockSeatService.Object);

            _mockAuditoriumsRepository.Setup(x => x.GetAll()).Returns(responseTask);

            //Act
            var result = auditoriumService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AuditoriumService_GetByIdAsync_ReturnsAuditoriumModel()
        {
            //Arrange
            Task<Auditorium> auditorium = Task.FromResult(_auditorium);
            AuditoriumService auditoriumService = new AuditoriumService(_mockAuditoriumsRepository.Object, _mockProjectionService.Object, _mockSeatService.Object);

            _mockAuditoriumsRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns(auditorium);

            //Act
            var result = auditoriumService.GetByIdAsync(_auditorium.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(AuditoriumDomainModel));
            Assert.AreEqual(result.Id, _auditorium.Id);

        }

        [TestMethod]
        public void AuditoriumService_GetByIdAsync_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            Auditorium nullAuditorium = null;
            Task<Auditorium> auditorium = Task.FromResult(nullAuditorium);
            AuditoriumService auditoriumService = new AuditoriumService(_mockAuditoriumsRepository.Object, _mockProjectionService.Object, _mockSeatService.Object);

            _mockAuditoriumsRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns(auditorium);

            //Act
            var result = auditoriumService.GetByIdAsync(_auditorium.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AuditoriumService_GetByCinemaId_ReturnsListOfAuditoriums()
        {
            //Arrange
            List<Auditorium> auditoriumsModelsList = new List<Auditorium>();
            auditoriumsModelsList.Add(_auditorium);
            IEnumerable<Auditorium> auditoriums = auditoriumsModelsList;
            Task<IEnumerable<Auditorium>> responseTask = Task.FromResult(auditoriums);
            int expectedCount = 1;

            AuditoriumService auditoriumService = new AuditoriumService(_mockAuditoriumsRepository.Object, _mockProjectionService.Object, _mockSeatService.Object);

            _mockAuditoriumsRepository.Setup(x => x.GetByCinemaId(It.IsAny<int>())).Returns(responseTask);

            //Act
            var result = auditoriumService.GetByCinemaId(_auditorium.CinemaId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.IsInstanceOfType(result[0], typeof(AuditoriumDomainModel));
            Assert.AreEqual(result[0].Id, _auditorium.Id);
            Assert.AreEqual(result[0].CinemaId, _auditorium.CinemaId);
        }

        [TestMethod]
        public void AuditoriumService_GetByCinemaid_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            IEnumerable<Auditorium> auditoriums = null;
            Task<IEnumerable<Auditorium>> responseTask = Task.FromResult(auditoriums);

            AuditoriumService auditoriumService = new AuditoriumService(_mockAuditoriumsRepository.Object, _mockProjectionService.Object, _mockSeatService.Object);

            _mockAuditoriumsRepository.Setup(x => x.GetByCinemaId(It.IsAny<int>())).Returns(responseTask);

            //Act
            var result = auditoriumService.GetByCinemaId(_auditorium.CinemaId).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AuditoriumService_DeleteAuditorium_ReturnsDeletedAuditorium_ReturnsDeletedProjectionsAndSeats()
        {
            //Arrange
            List<ProjectionDomainModel> projectionsModelsList = new List<ProjectionDomainModel>();
            projectionsModelsList.Add(_projectionDomainModel);
            IEnumerable<ProjectionDomainModel> projections = projectionsModelsList;
            Task<IEnumerable<ProjectionDomainModel>> projectionsResponseTask = Task.FromResult(projections);

            List<SeatDomainModel> seatsModelsList = new List<SeatDomainModel>();
            seatsModelsList.Add(_seatDomainModel);
            IEnumerable<SeatDomainModel> seats = seatsModelsList;
            Task<IEnumerable<SeatDomainModel>> seatsResponseTask = Task.FromResult(seats);

            AuditoriumService auditoriumService = new AuditoriumService(_mockAuditoriumsRepository.Object, _mockProjectionService.Object, _mockSeatService.Object);

            _mockProjectionService.Setup(x => x.DeleteByAuditoriumId(It.IsAny<int>())).Returns(projectionsResponseTask);
            _mockSeatService.Setup(x => x.DeleteByAuditoriumId(It.IsAny<int>())).Returns(seatsResponseTask);
            _mockAuditoriumsRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns(_auditorium);

            //Act
            var result = auditoriumService.DeleteAuditorium(_auditorium.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(AuditoriumDomainModel));
            Assert.IsNotNull(result.SeatsList);
            Assert.AreEqual(result.Id, _auditorium.Id);
        }


        [TestMethod]
        public void AuditoriumService_DeleteAuditorium_ProjectionServiceReturnsNull_ReturnsNull()
        {
            //Arrange
            IEnumerable<ProjectionDomainModel> projections = null;
            Task<IEnumerable<ProjectionDomainModel>> projectionsResponseTask = Task.FromResult(projections);

            List<SeatDomainModel> seatsModelsList = new List<SeatDomainModel>();
            seatsModelsList.Add(_seatDomainModel);
            IEnumerable<SeatDomainModel> seats = seatsModelsList;
            Task<IEnumerable<SeatDomainModel>> seatsResponseTask = Task.FromResult(seats);

            AuditoriumService auditoriumService = new AuditoriumService(_mockAuditoriumsRepository.Object, _mockProjectionService.Object, _mockSeatService.Object);

            _mockProjectionService.Setup(x => x.DeleteByAuditoriumId(It.IsAny<int>())).Returns(projectionsResponseTask);
            _mockSeatService.Setup(x => x.DeleteByAuditoriumId(It.IsAny<int>())).Returns(seatsResponseTask);
            _mockAuditoriumsRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns(_auditorium);

            //Act
            var result = auditoriumService.DeleteAuditorium(_auditorium.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AuditoriumService_DeleteAuditorium_SeatServiceReturnsNull_ReturnNull()
        {
            //Arrange
            List<ProjectionDomainModel> projectionsModelsList = new List<ProjectionDomainModel>();
            projectionsModelsList.Add(_projectionDomainModel);
            IEnumerable<ProjectionDomainModel> projections = projectionsModelsList;
            Task<IEnumerable<ProjectionDomainModel>> projectionsResponseTask = Task.FromResult(projections);

            IEnumerable<SeatDomainModel> seats = null;
            Task<IEnumerable<SeatDomainModel>> seatsResponseTask = Task.FromResult(seats);

            AuditoriumService auditoriumService = new AuditoriumService(_mockAuditoriumsRepository.Object, _mockProjectionService.Object, _mockSeatService.Object);

            _mockProjectionService.Setup(x => x.DeleteByAuditoriumId(It.IsAny<int>())).Returns(projectionsResponseTask);
            _mockSeatService.Setup(x => x.DeleteByAuditoriumId(It.IsAny<int>())).Returns(seatsResponseTask);
            _mockAuditoriumsRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns(_auditorium);

            //Act
            var result = auditoriumService.DeleteAuditorium(_auditorium.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AuditoriumService_DeleteAuditorium_AuditoriumRepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            Auditorium nullAuditorium = null;

            List<ProjectionDomainModel> projectionsModelsList = new List<ProjectionDomainModel>();
            projectionsModelsList.Add(_projectionDomainModel);
            IEnumerable<ProjectionDomainModel> projections = projectionsModelsList;
            Task<IEnumerable<ProjectionDomainModel>> projectionsResponseTask = Task.FromResult(projections);

            List<SeatDomainModel> seatsModelsList = new List<SeatDomainModel>();
            seatsModelsList.Add(_seatDomainModel);
            IEnumerable<SeatDomainModel> seats = seatsModelsList;
            Task<IEnumerable<SeatDomainModel>> seatsResponseTask = Task.FromResult(seats);

            AuditoriumService auditoriumService = new AuditoriumService(_mockAuditoriumsRepository.Object, _mockProjectionService.Object, _mockSeatService.Object);

            _mockProjectionService.Setup(x => x.DeleteByAuditoriumId(It.IsAny<int>())).Returns(projectionsResponseTask);
            _mockSeatService.Setup(x => x.DeleteByAuditoriumId(It.IsAny<int>())).Returns(seatsResponseTask);
            _mockAuditoriumsRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns(nullAuditorium);

            //Act
            var result = auditoriumService.DeleteAuditorium(_auditorium.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }


        [TestMethod]
        public void AuditoriumService_DeleteAuditoriumByCinemaId_ReturnsDeletedAuditoriums()
        {
            //Arrange
            List<Auditorium> auditoriumsModelsList = new List<Auditorium>();
            auditoriumsModelsList.Add(_auditorium);
            IEnumerable<Auditorium> auditoriums = auditoriumsModelsList;
            Task<IEnumerable<Auditorium>> responseTask = Task.FromResult(auditoriums);
            int expectedCount = 1;

            List<ProjectionDomainModel> projectionsModelsList = new List<ProjectionDomainModel>();
            projectionsModelsList.Add(_projectionDomainModel);
            IEnumerable<ProjectionDomainModel> projections = projectionsModelsList;
            Task<IEnumerable<ProjectionDomainModel>> projectionsResponseTask = Task.FromResult(projections);

            List<SeatDomainModel> seatsModelsList = new List<SeatDomainModel>();
            seatsModelsList.Add(_seatDomainModel);
            IEnumerable<SeatDomainModel> seats = seatsModelsList;
            Task<IEnumerable<SeatDomainModel>> seatsResponseTask = Task.FromResult(seats);

            AuditoriumService auditoriumService = new AuditoriumService(_mockAuditoriumsRepository.Object, _mockProjectionService.Object, _mockSeatService.Object);

            _mockProjectionService.Setup(x => x.DeleteByAuditoriumId(It.IsAny<int>())).Returns(projectionsResponseTask);
            _mockSeatService.Setup(x => x.DeleteByAuditoriumId(It.IsAny<int>())).Returns(seatsResponseTask);
            _mockAuditoriumsRepository.Setup(x => x.GetByCinemaId(It.IsAny<int>())).Returns(responseTask);
            _mockAuditoriumsRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns(_auditorium);

            //Act
            var result = auditoriumService.DeleteAuditoriumsByCinemaId(_auditorium.CinemaId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.IsInstanceOfType(result[0], typeof(AuditoriumDomainModel));
            Assert.AreEqual(result[0].Id, _auditorium.Id);
            Assert.AreEqual(result[0].CinemaId, _auditorium.CinemaId);
        }

        [TestMethod]
        public void AuditoriumService_DeleteAuditoriumByCinemaId_AuditoriumsRepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            IEnumerable<Auditorium> auditoriums = null;
            Task<IEnumerable<Auditorium>> responseTask = Task.FromResult(auditoriums);
            int expectedCount = 1;

            List<ProjectionDomainModel> projectionsModelsList = new List<ProjectionDomainModel>();
            projectionsModelsList.Add(_projectionDomainModel);
            IEnumerable<ProjectionDomainModel> projections = projectionsModelsList;
            Task<IEnumerable<ProjectionDomainModel>> projectionsResponseTask = Task.FromResult(projections);

            List<SeatDomainModel> seatsModelsList = new List<SeatDomainModel>();
            seatsModelsList.Add(_seatDomainModel);
            IEnumerable<SeatDomainModel> seats = seatsModelsList;
            Task<IEnumerable<SeatDomainModel>> seatsResponseTask = Task.FromResult(seats);

            AuditoriumService auditoriumService = new AuditoriumService(_mockAuditoriumsRepository.Object, _mockProjectionService.Object, _mockSeatService.Object);

            _mockProjectionService.Setup(x => x.DeleteByAuditoriumId(It.IsAny<int>())).Returns(projectionsResponseTask);
            _mockSeatService.Setup(x => x.DeleteByAuditoriumId(It.IsAny<int>())).Returns(seatsResponseTask);
            _mockAuditoriumsRepository.Setup(x => x.GetByCinemaId(It.IsAny<int>())).Returns(responseTask);
            _mockAuditoriumsRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns(_auditorium);

            //Act
            var result = auditoriumService.DeleteAuditoriumsByCinemaId(_auditorium.CinemaId).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AuditoriumService_DeleteAuditoriumByCinemaId_ProjectionServiceReturnsNull_ReturnsNull()
        {
            //Arrange
            List<Auditorium> auditoriumsModelsList = new List<Auditorium>();
            auditoriumsModelsList.Add(_auditorium);
            IEnumerable<Auditorium> auditoriums = auditoriumsModelsList;
            Task<IEnumerable<Auditorium>> responseTask = Task.FromResult(auditoriums);
            int expectedCount = 1;

            IEnumerable<ProjectionDomainModel> projections = null;
            Task<IEnumerable<ProjectionDomainModel>> projectionsResponseTask = Task.FromResult(projections);

            List<SeatDomainModel> seatsModelsList = new List<SeatDomainModel>();
            seatsModelsList.Add(_seatDomainModel);
            IEnumerable<SeatDomainModel> seats = seatsModelsList;
            Task<IEnumerable<SeatDomainModel>> seatsResponseTask = Task.FromResult(seats);

            AuditoriumService auditoriumService = new AuditoriumService(_mockAuditoriumsRepository.Object, _mockProjectionService.Object, _mockSeatService.Object);

            _mockProjectionService.Setup(x => x.DeleteByAuditoriumId(It.IsAny<int>())).Returns(projectionsResponseTask);
            _mockSeatService.Setup(x => x.DeleteByAuditoriumId(It.IsAny<int>())).Returns(seatsResponseTask);
            _mockAuditoriumsRepository.Setup(x => x.GetByCinemaId(It.IsAny<int>())).Returns(responseTask);
            _mockAuditoriumsRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns(_auditorium);

            //Act
            var result = auditoriumService.DeleteAuditoriumsByCinemaId(_auditorium.CinemaId).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AuditoriumService_DeleteAuditoriumByCinemaId_SeatServiceReturnsNull_ReturnsNull()
        {
            //Arrange
            List<Auditorium> auditoriumsModelsList = new List<Auditorium>();
            auditoriumsModelsList.Add(_auditorium);
            IEnumerable<Auditorium> auditoriums = auditoriumsModelsList;
            Task<IEnumerable<Auditorium>> responseTask = Task.FromResult(auditoriums);
            int expectedCount = 1;

            List<ProjectionDomainModel> projectionsModelsList = new List<ProjectionDomainModel>();
            projectionsModelsList.Add(_projectionDomainModel);
            IEnumerable<ProjectionDomainModel> projections = projectionsModelsList;
            Task<IEnumerable<ProjectionDomainModel>> projectionsResponseTask = Task.FromResult(projections);

            IEnumerable<SeatDomainModel> seats = null;
            Task<IEnumerable<SeatDomainModel>> seatsResponseTask = Task.FromResult(seats);

            AuditoriumService auditoriumService = new AuditoriumService(_mockAuditoriumsRepository.Object, _mockProjectionService.Object, _mockSeatService.Object);

            _mockProjectionService.Setup(x => x.DeleteByAuditoriumId(It.IsAny<int>())).Returns(projectionsResponseTask);
            _mockSeatService.Setup(x => x.DeleteByAuditoriumId(It.IsAny<int>())).Returns(seatsResponseTask);
            _mockAuditoriumsRepository.Setup(x => x.GetByCinemaId(It.IsAny<int>())).Returns(responseTask);
            _mockAuditoriumsRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns(_auditorium);

            //Act
            var result = auditoriumService.DeleteAuditoriumsByCinemaId(_auditorium.CinemaId).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AuditoriumService_DeleteAuditoriumByCinemaId_AuditoriumsRepositoryDeleteReturnsNull_ReturnsNull()
        {
            //Arrange
            List<Auditorium> auditoriumsModelsList = new List<Auditorium>();
            auditoriumsModelsList.Add(_auditorium);
            IEnumerable<Auditorium> auditoriums = auditoriumsModelsList;
            Task<IEnumerable<Auditorium>> responseTask = Task.FromResult(auditoriums);
            int expectedCount = 1;

            List<ProjectionDomainModel> projectionsModelsList = new List<ProjectionDomainModel>();
            projectionsModelsList.Add(_projectionDomainModel);
            IEnumerable<ProjectionDomainModel> projections = projectionsModelsList;
            Task<IEnumerable<ProjectionDomainModel>> projectionsResponseTask = Task.FromResult(projections);

            List<SeatDomainModel> seatsModelsList = new List<SeatDomainModel>();
            seatsModelsList.Add(_seatDomainModel);
            IEnumerable<SeatDomainModel> seats = seatsModelsList;
            Task<IEnumerable<SeatDomainModel>> seatsResponseTask = Task.FromResult(seats);

            Auditorium nullAuditorium = null;

            AuditoriumService auditoriumService = new AuditoriumService(_mockAuditoriumsRepository.Object, _mockProjectionService.Object, _mockSeatService.Object);

            _mockProjectionService.Setup(x => x.DeleteByAuditoriumId(It.IsAny<int>())).Returns(projectionsResponseTask);
            _mockSeatService.Setup(x => x.DeleteByAuditoriumId(It.IsAny<int>())).Returns(seatsResponseTask);
            _mockAuditoriumsRepository.Setup(x => x.GetByCinemaId(It.IsAny<int>())).Returns(responseTask);
            _mockAuditoriumsRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns(nullAuditorium);

            //Act
            var result = auditoriumService.DeleteAuditoriumsByCinemaId(_auditorium.CinemaId).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }
    }
}

