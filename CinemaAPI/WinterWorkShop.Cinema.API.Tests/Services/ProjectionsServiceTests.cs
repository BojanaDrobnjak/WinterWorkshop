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
    public class ProjectionsServiceTests
    {
        private Mock<IProjectionsRepository> _mockProjectionsRepository;
        private Projection _projection;
        private ProjectionDomainModel _projectionDomainModel;
        private Mock<IReservationService> _mockReservationService;
        private ReservationDomainModel _reservationDomainModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _projection = new Projection
            {
                Id = Guid.NewGuid(),
                Auditorium = new Auditorium { Name = "ImeSale" },
                Movie = new Movie { Title = "ImeFilma" },
                MovieId = Guid.NewGuid(),
                DateTime = DateTime.Now.AddDays(1),
                AuditoriumId = 1
            };

            _projectionDomainModel = new ProjectionDomainModel
            {
                Id = _projection.Id,
                AuditoriumName = "ImeSale",
                AuditoriumId = 1,
                MovieId = Guid.NewGuid(),
                MovieTitle = "ImeFilma",
                ProjectionTime = DateTime.Now.AddDays(1)
            };

            _reservationDomainModel = new ReservationDomainModel
            {
                ProjectionId = _projection.Id,
                SeatId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            List<Projection> projectionsModelsList = new List<Projection>();

            projectionsModelsList.Add(_projection);
            IEnumerable<Projection> projections = projectionsModelsList;
            Task<IEnumerable<Projection>> responseTask = Task.FromResult(projections);

            _mockProjectionsRepository = new Mock<IProjectionsRepository>();
            _mockProjectionsRepository.Setup(x => x.GetAll()).Returns(responseTask);



            List<ReservationDomainModel> reservationDomainModelsList = new List<ReservationDomainModel>();

            reservationDomainModelsList.Add(_reservationDomainModel);
            IEnumerable<ReservationDomainModel> reservations = reservationDomainModelsList;
            Task<IEnumerable<ReservationDomainModel>> reservationResponseTask = Task.FromResult(reservations);

            _mockReservationService = new Mock<IReservationService>();
            _mockReservationService.Setup(x => x.DeleteByProjectionId(It.IsAny<Guid>())).Returns(reservationResponseTask);
        }

        [TestMethod]
        public void ProjectionService_GetAllAsync_ReturnListOfProjections()
        {
            //Arrange

            int expectedResultCount = 1;
            ProjectionService projectionsController = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            //Act
            var resultAction = projectionsController.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = (List<ProjectionDomainModel>)resultAction;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResultCount, result.Count);
            Assert.AreEqual(_projection.Id, result[0].Id);
            Assert.IsInstanceOfType(result[0], typeof(ProjectionDomainModel));
        }

        [TestMethod]
        public void ProjectionService_GetAllAsync_ReturnNull()
        {
            //Arrange
            IEnumerable<Projection> projections = null;
            Task<IEnumerable<Projection>> responseTask = Task.FromResult(projections);

            _mockProjectionsRepository = new Mock<IProjectionsRepository>();
            _mockProjectionsRepository.Setup(x => x.GetAll()).Returns(responseTask);
            ProjectionService projectionsController = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            //Act
            var resultAction = projectionsController.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void ProjectionService_CreateProjection_WithProjectionAtSameTime_ReturnErrorMessage()
        {
            //Arrange
            List<Projection> projectionsModelsList = new List<Projection>();
            projectionsModelsList.Add(_projection);
            string expectedMessage = "Cannot create new projection, there are projections at same time alredy.";

            _mockProjectionsRepository = new Mock<IProjectionsRepository>();
            _mockProjectionsRepository.Setup(x => x.GetByAuditoriumId(It.IsAny<int>())).Returns(projectionsModelsList);
            ProjectionService projectionsController = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            //Act
            var resultAction = projectionsController.CreateProjection(_projectionDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedMessage, resultAction.ErrorMessage);
            Assert.IsFalse(resultAction.IsSuccessful);
        }

        [TestMethod]
        public void ProjectionService_CreateProjection_InsertMockedNull_ReturnErrorMessage()
        {
            //Arrange
            List<Projection> projectionsModelsList = new List<Projection>();
            _projection = null;
            string expectedMessage = "Error occured while creating new projection, please try again.";

            _mockProjectionsRepository = new Mock<IProjectionsRepository>();
            _mockProjectionsRepository.Setup(x => x.GetByAuditoriumId(It.IsAny<int>())).Returns(projectionsModelsList);
            _mockProjectionsRepository.Setup(x => x.Insert(It.IsAny<Projection>())).Returns(_projection);
            ProjectionService projectionsController = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            //Act
            var resultAction = projectionsController.CreateProjection(_projectionDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedMessage, resultAction.ErrorMessage);
            Assert.IsFalse(resultAction.IsSuccessful);
        }

        [TestMethod]
        public void ProjectionService_CreateProjection_InsertMocked_ReturnProjection()
        {
            //Arrange
            List<Projection> projectionsModelsList = new List<Projection>();

            _mockProjectionsRepository = new Mock<IProjectionsRepository>();
            _mockProjectionsRepository.Setup(x => x.GetByAuditoriumId(It.IsAny<int>())).Returns(projectionsModelsList);
            _mockProjectionsRepository.Setup(x => x.Insert(It.IsAny<Projection>())).Returns(_projection);
            _mockProjectionsRepository.Setup(x => x.Save());
            ProjectionService projectionsController = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            //Act
            var resultAction = projectionsController.CreateProjection(_projectionDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(_projection.Id, resultAction.Projection.Id);
            Assert.IsNull(resultAction.ErrorMessage);
            Assert.IsTrue(resultAction.IsSuccessful);
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void Projectionervice_CreateProjection_When_Updating_Non_Existing_Movie()
        {
            // Arrange

            List<Projection> projectionsModelsList = new List<Projection>();

            _mockProjectionsRepository = new Mock<IProjectionsRepository>();
            _mockProjectionsRepository.Setup(x => x.Insert(It.IsAny<Projection>())).Throws(new DbUpdateException());
            _mockProjectionsRepository.Setup(x => x.Save());
            ProjectionService projectionsController = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            //Act
            var resultAction = projectionsController.CreateProjection(_projectionDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void ProjectionService_DeleteProjection_ReturnDeletedProjection()
        {
            // Arrange

            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            _mockProjectionsRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(_projection);
            _mockProjectionsRepository.Setup(x => x.Save());

            // Act
            var resultAction = projectionsService.DeleteProjection(_projection.Id).ConfigureAwait(false).GetAwaiter().GetResult();


            // Assert

            Assert.IsNotNull(resultAction);
            Assert.AreEqual(resultAction.Id, _projection.Id);
            Assert.IsInstanceOfType(resultAction, typeof(ProjectionDomainModel));

        }

        [TestMethod]
        public void ProjectionService_DeleteProjection_ReservationServiceReturnsNull_ReturnsNull()
        {

            // Arrange
            Task<IEnumerable<ReservationDomainModel>> reservationResponseTask = null;

            _mockReservationService = new Mock<IReservationService>();
            _mockReservationService.Setup(x => x.DeleteByProjectionId(It.IsAny<Guid>())).Returns(reservationResponseTask);

            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            _mockProjectionsRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(_projection);
            _mockProjectionsRepository.Setup(x => x.Save());

            // Act

            var resultAction = projectionsService.DeleteProjection(_projection.Id).ConfigureAwait(false).GetAwaiter().GetResult();


            // Assert

            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void ProjectionService_DeleteProjection_ProjectionRepositoryReturnsNull_ReturnsNull()
        {

            // Arrange

            Projection projection = null;

            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            _mockProjectionsRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(projection);
            _mockProjectionsRepository.Setup(x => x.Save());

            // Act

            var resultAction = projectionsService.DeleteProjection(_projection.Id).ConfigureAwait(false).GetAwaiter().GetResult();


            // Assert

            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void ProjectionService_UpdateProjection_ReturnUpdatedProjection()
        {
            // Arrange

            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            _mockProjectionsRepository.Setup(x => x.Update(It.IsAny<Projection>())).Returns(_projection);
            _mockProjectionsRepository.Setup(x => x.Save());

            // Act

            var resultAction = projectionsService.UpdateProjection(_projectionDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            // Assert

            Assert.IsNotNull(resultAction);
            Assert.AreEqual(resultAction.Id, _projectionDomainModel.Id);
            Assert.IsInstanceOfType(resultAction, typeof(ProjectionDomainModel));

        }

        [TestMethod]
        public void ProjectionService_UpdateProjection_ProjectionRepositoryReturnsNull_ReturnsNull()
        {

            //Arrange

            Projection projection = null;

            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            _mockProjectionsRepository.Setup(x => x.Update(It.IsAny<Projection>())).Returns(projection);
            _mockProjectionsRepository.Setup(x => x.Save());

            //Act

            var resultAction = projectionsService.UpdateProjection(_projectionDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert

            Assert.IsNull(resultAction);

        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void ProjectionService_UpdateProjection_UpdatingNonexistingProjection()
        {
            // Arrange

            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            _mockProjectionsRepository.Setup(x => x.Update(It.IsAny<Projection>())).Throws(new DbUpdateException());
            _mockProjectionsRepository.Setup(x => x.Save());

            // Act

            var resultAction = projectionsService.UpdateProjection(_projectionDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void ProjectionService_GetProjectionByIdAsync_ReturnsProjectionDomainModel()
        {
            //Arrange

            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            _mockProjectionsRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(_projection));
            _mockProjectionsRepository.Setup(x => x.Save());

            //Act

            var resultAction = projectionsService.GetProjectionByIdAsync(_projection.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert

            Assert.IsNotNull(resultAction);
            Assert.AreEqual(resultAction.Id, _projection.Id);
            Assert.IsInstanceOfType(resultAction, typeof(ProjectionDomainModel));
        }

        [TestMethod]
        public void ProjectionService_GetProjectionByIdAsync_IncorrectId_ReturnNull()
        {
            //Arrange
            Projection projection = null;

            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            _mockProjectionsRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult(projection));
            _mockProjectionsRepository.Setup(x => x.Save());

            //Act

            var resultAction = projectionsService.GetProjectionByIdAsync(_projection.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert

            Assert.IsNull(resultAction);
            Assert.IsNotInstanceOfType(resultAction, typeof(ProjectionDomainModel));
        }

        [TestMethod]
        public void ProjectionService_GetProjectionsByAuditoriumId_ReturnsProjectionDomainModelList()
        {
            // Arrange
            int expectedCount = 1;
            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            List<Projection> projectionsModelsList = new List<Projection>();
            projectionsModelsList.Add(_projection);

            _mockProjectionsRepository.Setup(x => x.GetByAuditoriumId(It.IsAny<int>())).Returns(projectionsModelsList);

            //Act

            var resultAction = projectionsService.GetProjectionsByAuditoriumId(_projection.AuditoriumId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();

            //Assert

            Assert.IsNotNull(resultAction);
            Assert.AreEqual(resultAction.Count, expectedCount);
            Assert.IsInstanceOfType(resultAction[0], typeof(ProjectionDomainModel));
            Assert.AreEqual(resultAction[0].Id, _projection.Id);
        }

        [TestMethod]
        public void ProjectionService_GetProjectionsByAuditoriumId_WrongId_ReturnEmptyList()
        {
            // Arrange
            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            List<Projection> projectionsModelsList = new List<Projection>();

            _mockProjectionsRepository.Setup(x => x.GetByAuditoriumId(It.IsAny<int>())).Returns(projectionsModelsList);

            //Act

            var resultAction = projectionsService.GetProjectionsByAuditoriumId(_projection.AuditoriumId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();

            //Assert

            Assert.IsNotNull(resultAction);
            Assert.AreEqual(resultAction.Count, 0);
            Assert.IsInstanceOfType(resultAction, typeof(List<ProjectionDomainModel>));
        }

        [TestMethod]
        public void ProjectionService_DeleteByAuditoriumId_ReturnsProjectionDomainModelList()
        {
            //Arrange
            int expectedCount = 1;
            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            List<Projection> projectionsModelsList = new List<Projection>();
            projectionsModelsList.Add(_projection);
            IEnumerable<Projection> projections = projectionsModelsList;

            List<ReservationDomainModel> reservationDomainModelsList = new List<ReservationDomainModel>();
            reservationDomainModelsList.Add(_reservationDomainModel);
            IEnumerable<ReservationDomainModel> reservations = reservationDomainModelsList;
            Task<IEnumerable<ReservationDomainModel>> reservationResponseTask = Task.FromResult(reservations);

            _mockProjectionsRepository.Setup(x => x.GetByAuditoriumId(It.IsAny<int>())).Returns(projections);
            _mockProjectionsRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(_projection);
            _mockReservationService.Setup(x => x.DeleteByProjectionId(It.IsAny<Guid>())).Returns(reservationResponseTask);

            //Act
            var resultAction = projectionsService.DeleteByAuditoriumId(_projection.AuditoriumId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();

            //Assert

            Assert.IsNotNull(resultAction);
            Assert.AreEqual(resultAction.Count, expectedCount);
            Assert.IsInstanceOfType(resultAction[0], typeof(ProjectionDomainModel));
            Assert.AreEqual(resultAction[0].Id, _projection.Id);
        }

        [TestMethod]
        public void ProjectionService_DeleteByAuditoriumId_ReservationServiceReturnsNull_ReturnsNull()
        {
            // Arrange

            List<Projection> projectionsModelsListForGet = new List<Projection>();
            projectionsModelsListForGet.Add(_projection);
            _mockProjectionsRepository.Setup(x => x.GetByAuditoriumId(It.IsAny<int>())).Returns(projectionsModelsListForGet);

            List<ReservationDomainModel> reservationModelsList = new List<ReservationDomainModel>();
            reservationModelsList.Add(_reservationDomainModel);
            IEnumerable<ReservationDomainModel> reservations = null;
            Task<IEnumerable<ReservationDomainModel>> reservationResponseTask = Task.FromResult(reservations);
            _mockReservationService.Setup(x => x.DeleteByProjectionId(It.IsAny<Guid>())).Returns(reservationResponseTask);

            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            List<Projection> projectionsModelsList = new List<Projection>();
            projectionsModelsList.Add(_projection);
            IEnumerable<Projection> projections = projectionsModelsList;
            Task<IEnumerable<Projection>> responseTask = Task.FromResult(projections);

            _mockProjectionsRepository.Setup(x => x.DeleteByAuditoriumId(It.IsAny<int>())).Returns(responseTask);

            //Act 
            var resultAction = projectionsService.DeleteByAuditoriumId(_projection.AuditoriumId).ConfigureAwait(false).GetAwaiter().GetResult();

            // Assert

            Assert.IsNull(resultAction);
        }
        [TestMethod]
        public void ProjectionService_DeleteByAuditoriumId_ProjectionRepositoryReturnsNull_ReturnsNull()
        {

            List<Projection> projectionsModelsList = new List<Projection>();
            projectionsModelsList.Add(_projection);
            IEnumerable<Projection> projections = projectionsModelsList;

            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            Projection projection = null;

            _mockProjectionsRepository.Setup(x => x.GetByAuditoriumId(It.IsAny<int>())).Returns(projections);
            _mockProjectionsRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns(projection);

            //Act
            var resultAction = projectionsService.DeleteByAuditoriumId(_projection.AuditoriumId).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);

        }

        [TestMethod]
        public void ProjectionService_DeleteByMovieId_ReturnsProjectionDomainModelList()
        {
            //Arrange

            int expectedCount = 1;
            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            List<Projection> projectionsModelsList = new List<Projection>();
            projectionsModelsList.Add(_projection);
            IEnumerable<Projection> projections = projectionsModelsList;
            Task<IEnumerable<Projection>> responseTask = Task.FromResult(projections);

            _mockProjectionsRepository.Setup(x => x.DeleteByMovieId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = projectionsService.DeleteByMovieId(_projection.MovieId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();

            //Assert

            Assert.IsNotNull(resultAction);
            Assert.AreEqual(resultAction.Count, expectedCount);
            Assert.IsInstanceOfType(resultAction[0], typeof(ProjectionDomainModel));
            Assert.AreEqual(resultAction[0].Id, _projection.Id);
        }

        [TestMethod]
        public void ProjectionService_DeleteByMovieId_ReservationServiceReturnsNull_ReturnsNull()
        {
            // Arrange

            List<Projection> projectionsModelsListForGet = new List<Projection>();
            projectionsModelsListForGet.Add(_projection);
            _mockProjectionsRepository.Setup(x => x.GetByMovieId(It.IsAny<Guid>())).Returns(projectionsModelsListForGet);

            List<ReservationDomainModel> reservationModelsList = new List<ReservationDomainModel>();
            reservationModelsList.Add(_reservationDomainModel);
            IEnumerable<ReservationDomainModel> reservations = null;
            Task<IEnumerable<ReservationDomainModel>> reservationResponseTask = Task.FromResult(reservations);
            _mockReservationService.Setup(x => x.DeleteByProjectionId(It.IsAny<Guid>())).Returns(reservationResponseTask);

            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            List<Projection> projectionsModelsList = new List<Projection>();
            projectionsModelsList.Add(_projection);
            IEnumerable<Projection> projections = projectionsModelsList;
            Task<IEnumerable<Projection>> responseTask = Task.FromResult(projections);

            _mockProjectionsRepository.Setup(x => x.DeleteByMovieId(It.IsAny<Guid>())).Returns(responseTask);

            //Act 
            var resultAction = projectionsService.DeleteByMovieId(_projection.MovieId).ConfigureAwait(false).GetAwaiter().GetResult();

            // Assert

            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void ProjectionService_DeleteByMovieId_ProjectionRepositoryReturnsNull_ReturnsNull()
        {

            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            IEnumerable<Projection> projections = null;
            Task<IEnumerable<Projection>> responseTask = Task.FromResult(projections);

            _mockProjectionsRepository.Setup(x => x.DeleteByMovieId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = projectionsService.DeleteByMovieId(_projection.MovieId).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);

        }

        [TestMethod]
        public void ProjectionService_DeleteByAuditoriumMovieId_ReturnsProjectionDomainModelList()
        {
            //Arrange

            int expectedCount = 1;
            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            List<Projection> projectionsModelsList = new List<Projection>();
            projectionsModelsList.Add(_projection);
            IEnumerable<Projection> projections = projectionsModelsList;
            Task<IEnumerable<Projection>> responseTask = Task.FromResult(projections);

            _mockProjectionsRepository.Setup(x => x.DeleteByAuditoriumIdMovieId(It.IsAny<int>(),It.IsAny<Guid>())).Returns(responseTask);
            _mockProjectionsRepository.Setup(x => x.Save());

            //Act
            var resultAction = projectionsService.DeleteByAuditoriumIdMovieId(_projection.AuditoriumId,_projection.MovieId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();

            //Assert

            Assert.IsNotNull(resultAction);
            Assert.AreEqual(resultAction.Count, expectedCount);
            Assert.IsInstanceOfType(resultAction[0], typeof(ProjectionDomainModel));
            Assert.AreEqual(resultAction[0].Id, _projection.Id);
        }

        [TestMethod]
        public void ProjectionService_DeleteByAuditoriumMovieId_ReservationServiceReturnsNull_ReturnsNull()
        {
            // Arrange

            List<Projection> projectionsModelsListForGet = new List<Projection>();
            projectionsModelsListForGet.Add(_projection);
            _mockProjectionsRepository.Setup(x => x.GetByAuditoriumIdMovieId(It.IsAny<int>(),It.IsAny<Guid>())).Returns(projectionsModelsListForGet);

            List<ReservationDomainModel> reservationModelsList = new List<ReservationDomainModel>();
            reservationModelsList.Add(_reservationDomainModel);
            IEnumerable<ReservationDomainModel> reservations = null;
            Task<IEnumerable<ReservationDomainModel>> reservationResponseTask = Task.FromResult(reservations);
            _mockReservationService.Setup(x => x.DeleteByProjectionId(It.IsAny<Guid>())).Returns(reservationResponseTask);

            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            List<Projection> projectionsModelsList = new List<Projection>();
            projectionsModelsList.Add(_projection);
            IEnumerable<Projection> projections = projectionsModelsList;
            Task<IEnumerable<Projection>> responseTask = Task.FromResult(projections);

            _mockProjectionsRepository.Setup(x => x.DeleteByAuditoriumIdMovieId(It.IsAny<int>(),It.IsAny<Guid>())).Returns(responseTask);

            //Act 
            var resultAction = projectionsService.DeleteByAuditoriumIdMovieId(_projection.AuditoriumId,_projection.MovieId).ConfigureAwait(false).GetAwaiter().GetResult();

            // Assert

            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void ProjectionService_DeleteByAuditoriumMovieId_ProjectionRepositoryReturnsNull_ReturnsNull()
        {

            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            IEnumerable<Projection> projections = null;
            Task<IEnumerable<Projection>> responseTask = Task.FromResult(projections);

            _mockProjectionsRepository.Setup(x => x.DeleteByAuditoriumIdMovieId(It.IsAny<int>(),It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = projectionsService.DeleteByAuditoriumIdMovieId(_projection.AuditoriumId,_projection.MovieId).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);

        }

        [TestMethod]
        public void ProjectionService_GetProjectionByMovieId_ReturnsProjectionDomainModel()
        {
            // Arrange
            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            List<Projection> projectionsModelsList = new List<Projection>();

            _mockProjectionsRepository.Setup(x => x.GetByMovieId(It.IsAny<Guid>())).Returns(projectionsModelsList);

            //Act

            var resultAction = projectionsService.GetProjectionByMovieId(_projection.MovieId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();

            //Assert

            Assert.IsNotNull(resultAction);
            Assert.AreEqual(resultAction.Count, 0);
            Assert.IsInstanceOfType(resultAction, typeof(List<ProjectionDomainModel>));
        }


        [TestMethod]
        public void ProjectionService_GetProjectionsByMovieId_WrongId_ReturnEmptyList()
        {
            // Arrange
            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            List<Projection> projectionsModelsList = new List<Projection>();

            _mockProjectionsRepository.Setup(x => x.GetByMovieId(It.IsAny<Guid>())).Returns(projectionsModelsList);

            //Act

            var resultAction = projectionsService.GetProjectionByMovieId(_projection.MovieId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();

            //Assert

            Assert.IsNotNull(resultAction);
            Assert.AreEqual(resultAction.Count, 0);
            Assert.IsInstanceOfType(resultAction, typeof(List<ProjectionDomainModel>));
        }

        [TestMethod]
        public void ProjectionService_GetProjectionByAuditoriumIdMovieId_ReturnsProjectionDomainModel()
        {
            // Arrange
            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            List<Projection> projectionsModelsList = new List<Projection>();

            _mockProjectionsRepository.Setup(x => x.GetByAuditoriumIdMovieId(It.IsAny<int>(),It.IsAny<Guid>())).Returns(projectionsModelsList);

            //Act

            var resultAction = projectionsService.GetProjectionByAuditoriumIdMovieId(_projection.AuditoriumId,_projection.MovieId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();

            //Assert

            Assert.IsNotNull(resultAction);
            Assert.AreEqual(resultAction.Count, 0);
            Assert.IsInstanceOfType(resultAction, typeof(List<ProjectionDomainModel>));
        }

        [TestMethod]
        public void ProjectionService_GetProjectionByAuditoriumIdMovieId_WrongId_ReturnEmptyList()
        {
            // Arrange
            ProjectionService projectionsService = new ProjectionService(_mockProjectionsRepository.Object, _mockReservationService.Object);

            List<Projection> projectionsModelsList = new List<Projection>();

            _mockProjectionsRepository.Setup(x => x.GetByAuditoriumIdMovieId(It.IsAny<int>(),It.IsAny<Guid>())).Returns(projectionsModelsList);

            //Act

            var resultAction = projectionsService.GetProjectionByAuditoriumIdMovieId(_projection.AuditoriumId,_projection.MovieId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();

            //Assert

            Assert.IsNotNull(resultAction);
            Assert.AreEqual(resultAction.Count, 0);
            Assert.IsInstanceOfType(resultAction, typeof(List<ProjectionDomainModel>));
        }

    }
}
