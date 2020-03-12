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
    public class MovieTagsControllerTests
    {
        private Mock<IMovieTagService> _mockMovieTagService;
        private MovieTagsDomainModel _movieTagDomainModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _movieTagDomainModel = new MovieTagsDomainModel
            {
                MovieId = Guid.NewGuid(),
                TagId = 1,
                MovieTitle = "NazivFilma",
                TagName = "NazivTaga"
            };

            List<MovieTagsDomainModel> movieTagsList = new List<MovieTagsDomainModel>();

            movieTagsList.Add(_movieTagDomainModel);
            IEnumerable<MovieTagsDomainModel> movieTags = movieTagsList;
            Task<IEnumerable<MovieTagsDomainModel>> responseTask = Task.FromResult(movieTags);

            _mockMovieTagService = new Mock<IMovieTagService>();
        }

        [TestMethod]
        public void MovieTagsController_GetAll_ReturnAllMovieTags()
        {
            //Arrange
            List<MovieTagsDomainModel> movieTagsList = new List<MovieTagsDomainModel>();
            movieTagsList.Add(_movieTagDomainModel);
            IEnumerable<MovieTagsDomainModel> movieTags = movieTagsList;
            Task<IEnumerable<MovieTagsDomainModel>> responseTask = Task.FromResult(movieTags);
            int expectedCount = 1;
            int expectedStatusCode = 200;

            MovieTagsController movieTagsController = new MovieTagsController(_mockMovieTagService.Object);

            _mockMovieTagService.Setup(x => x.GetAllAsync()).Returns(responseTask);

            //Act
            var result = movieTagsController.GetAll().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var movieTagDomainModelResultList = (List<MovieTagsDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(movieTagDomainModelResultList.Count, expectedCount);
            Assert.IsInstanceOfType(movieTagDomainModelResultList[0], typeof(MovieTagsDomainModel));
            Assert.AreEqual(movieTagDomainModelResultList[0].MovieId, _movieTagDomainModel.MovieId);
            Assert.AreEqual(movieTagDomainModelResultList[0].TagId, _movieTagDomainModel.TagId);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void MovieTagsController_GetAll_MovieTagServiceReturnsNull_ReturnEmptyList()
        {
            //Arrange
            IEnumerable<MovieTagsDomainModel> movieTags = null;
            Task<IEnumerable<MovieTagsDomainModel>> responseTask = Task.FromResult(movieTags);
            int expectedCount = 0;
            int expectedStatusCode = 200;

            MovieTagsController movieTagsController = new MovieTagsController(_mockMovieTagService.Object);

            _mockMovieTagService.Setup(x => x.GetAllAsync()).Returns(responseTask);

            //Act
            var result = movieTagsController.GetAll().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var movieTagDomainModelResultList = (List<MovieTagsDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(movieTagDomainModelResultList.Count, expectedCount);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);

        }

        [TestMethod]
        public void MovieTagsController_GetByMovieIdAsync_ReturnMovieTagsByMovieId()
        {
            //Arrange
            List<MovieTagsDomainModel> movieTagsList = new List<MovieTagsDomainModel>();
            movieTagsList.Add(_movieTagDomainModel);
            IEnumerable<MovieTagsDomainModel> movieTags = movieTagsList;
            Task<IEnumerable<MovieTagsDomainModel>> responseTask = Task.FromResult(movieTags);
            int expectedCount = 1;
            int expectedStatusCode = 200;

            MovieTagsController movieTagsController = new MovieTagsController(_mockMovieTagService.Object);

            _mockMovieTagService.Setup(x => x.GetByMovieId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var result = movieTagsController.GetByMovieIdAsync(_movieTagDomainModel.MovieId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var movieTagDomainModelResultList = (List<MovieTagsDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(movieTagDomainModelResultList.Count, expectedCount);
            Assert.IsInstanceOfType(movieTagDomainModelResultList[0], typeof(MovieTagsDomainModel));
            Assert.AreEqual(movieTagDomainModelResultList[0].MovieId, _movieTagDomainModel.MovieId);
            Assert.AreEqual(movieTagDomainModelResultList[0].TagId, _movieTagDomainModel.TagId);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void MovieTagsController_GetByMovieIdAsync_ServiceReturnsNull_Returns404()
        {
            //Arrange
            IEnumerable<MovieTagsDomainModel> movieTags = null;
            Task<IEnumerable<MovieTagsDomainModel>> responseTask = Task.FromResult(movieTags);
            int expectedStatusCode = 404;
            string expectedMessage = Messages.MOVIETAG_DOES_NOT_EXIST;

            MovieTagsController movieTagsController = new MovieTagsController(_mockMovieTagService.Object);

            _mockMovieTagService.Setup(x => x.GetByMovieId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var result = movieTagsController.GetByMovieIdAsync(_movieTagDomainModel.MovieId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultMessage = (NotFoundObjectResult)result;

            //Assert
            Assert.IsNotNull(resultMessage);
            Assert.AreEqual(resultMessage.Value.ToString(), expectedMessage);
            Assert.AreEqual(expectedStatusCode, resultMessage.StatusCode);
        }

        [TestMethod]
        public void MovieTagsController_GetByTagIdAsync_ReturnMovieTagsByMovieId()
        {
            //Arrange
            List<MovieTagsDomainModel> movieTagsList = new List<MovieTagsDomainModel>();
            movieTagsList.Add(_movieTagDomainModel);
            IEnumerable<MovieTagsDomainModel> movieTags = movieTagsList;
            Task<IEnumerable<MovieTagsDomainModel>> responseTask = Task.FromResult(movieTags);
            int expectedCount = 1;
            int expectedStatusCode = 200;

            MovieTagsController movieTagsController = new MovieTagsController(_mockMovieTagService.Object);

            _mockMovieTagService.Setup(x => x.GetByTagId(It.IsAny<int>())).Returns(responseTask);

            //Act
            var result = movieTagsController.GetByTagIdAsync(_movieTagDomainModel.TagId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var movieTagDomainModelResultList = (List<MovieTagsDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(movieTagDomainModelResultList.Count, expectedCount);
            Assert.IsInstanceOfType(movieTagDomainModelResultList[0], typeof(MovieTagsDomainModel));
            Assert.AreEqual(movieTagDomainModelResultList[0].MovieId, _movieTagDomainModel.MovieId);
            Assert.AreEqual(movieTagDomainModelResultList[0].TagId, _movieTagDomainModel.TagId);
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void MovieTagsController_GetByTagIdAsync_ServiceReturnsNull_Returns404()
        {
            //Arrange
            IEnumerable<MovieTagsDomainModel> movieTags = null;
            Task<IEnumerable<MovieTagsDomainModel>> responseTask = Task.FromResult(movieTags);
            int expectedStatusCode = 404;
            string expectedMessage = Messages.MOVIETAG_DOES_NOT_EXIST;

            MovieTagsController movieTagsController = new MovieTagsController(_mockMovieTagService.Object);

            _mockMovieTagService.Setup(x => x.GetByTagId(It.IsAny<int>())).Returns(responseTask);

            //Act
            var result = movieTagsController.GetByTagIdAsync(_movieTagDomainModel.TagId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultMessage = (NotFoundObjectResult)result;

            //Assert
            Assert.IsNotNull(resultMessage);
            Assert.AreEqual(resultMessage.Value.ToString(), expectedMessage);
            Assert.AreEqual(expectedStatusCode, resultMessage.StatusCode);
        }

        [TestMethod]
        public void MovieTagsController_Post_ReturnsInsertedMovieTag()
        {
            //Arrange
            Task<MovieTagsDomainModel> movieTagDomainModel = Task.FromResult(_movieTagDomainModel);
            int expectedStatusCode = 201;

            MovieTagsController movieTagsController = new MovieTagsController(_mockMovieTagService.Object);

            _mockMovieTagService.Setup(x => x.AddMovieTag(It.IsAny<MovieTagsDomainModel>())).Returns(movieTagDomainModel);

            //Act

            var result = movieTagsController.Post(_movieTagDomainModel.MovieId, _movieTagDomainModel.TagId).ConfigureAwait(false).GetAwaiter().GetResult();
            var resultMessage = ((CreatedResult)result).Value;
            var resultModel = (MovieTagsDomainModel)resultMessage;

            //Assert
            Assert.IsNotNull(resultMessage);
            Assert.AreEqual(expectedStatusCode, ((CreatedResult)result).StatusCode);
            Assert.IsInstanceOfType(resultModel, typeof(MovieTagsDomainModel));
            Assert.AreEqual(resultModel.MovieId, _movieTagDomainModel.MovieId);
        }

        [TestMethod]
        public void MovieTagsController_Post_ThrowsDbUpdateException()
        {
            //Arrange
            string expectedMessage = "Inner exception error message.";
            int expectedStatusCode = 400;
            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);

            MovieTagsController movieTagsController = new MovieTagsController(_mockMovieTagService.Object);

            _mockMovieTagService.Setup(x => x.AddMovieTag(It.IsAny<MovieTagsDomainModel>())).Throws(dbUpdateException);

            //Act

            var result = movieTagsController.Post(_movieTagDomainModel.MovieId, _movieTagDomainModel.TagId).ConfigureAwait(false).GetAwaiter().GetResult();
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
        public void MovieTagsController_MovieTagServiceReturnsNull()
        {
            //Arrange
            MovieTagsDomainModel nullDomainModel = null;
            Task<MovieTagsDomainModel> nullTask = Task.FromResult(nullDomainModel);

            string expectedMessage = Messages.MOVIETAG_DOES_NOT_EXIST;
            int expectedStatusCode = 500;

            ErrorResponseModel errorResponseModel = new ErrorResponseModel();
            errorResponseModel.ErrorMessage = expectedMessage;
            errorResponseModel.StatusCode = System.Net.HttpStatusCode.InternalServerError;

            MovieTagsController movieTagsController = new MovieTagsController(_mockMovieTagService.Object);

            _mockMovieTagService.Setup(x => x.AddMovieTag(It.IsAny<MovieTagsDomainModel>())).Returns(nullTask);

            //Act
            var result = movieTagsController.Post(_movieTagDomainModel.MovieId, _movieTagDomainModel.TagId).ConfigureAwait(false).GetAwaiter().GetResult();
            ObjectResult viewResult = (ObjectResult)result;
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(viewResult.Value.ToString(), errorResponseModel.ToString());
            Assert.AreEqual(viewResult.StatusCode, expectedStatusCode);
        }


        [TestMethod]
        public void MovieTagsController_DeleteByTagId_ReturnsDeletedMovieTags()
        {
            //Arrange

            //Arrange
            List<MovieTagsDomainModel> movieTagsList = new List<MovieTagsDomainModel>();
            movieTagsList.Add(_movieTagDomainModel);
            IEnumerable<MovieTagsDomainModel> movieTags = movieTagsList;
            Task<IEnumerable<MovieTagsDomainModel>> responseTask = Task.FromResult(movieTags);
            int expectedCount = 1;
            int expectedStatusCode = 202;

            MovieTagsController movieTagsController = new MovieTagsController(_mockMovieTagService.Object);

            _mockMovieTagService.Setup(x => x.DeleteByTagId(It.IsAny<int>())).Returns(responseTask);

            //Act

            var result = movieTagsController.DeleteByTagId(_movieTagDomainModel.TagId).ConfigureAwait(false).GetAwaiter().GetResult();
            var resultMessage = ((AcceptedResult)result).Value;
            var resultModel = (IEnumerable<MovieTagsDomainModel>)resultMessage;

            //Assert
            Assert.IsNotNull(resultMessage);
            Assert.AreEqual(expectedStatusCode, ((AcceptedResult)result).StatusCode);
            Assert.IsInstanceOfType(resultModel.ToList()[0], typeof(MovieTagsDomainModel));
        }


        [TestMethod]
        public void MovieTagsController_DeleteByTagId_ThrowsDbUpdateException()
        {
            //Arrange
            string expectedMessage = "Inner exception error message.";
            int expectedStatusCode = 400;
            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);

            MovieTagsController movieTagsController = new MovieTagsController(_mockMovieTagService.Object);

            _mockMovieTagService.Setup(x => x.DeleteByTagId(It.IsAny<int>())).Throws(dbUpdateException);

            //Act

            var result = movieTagsController.DeleteByTagId(_movieTagDomainModel.TagId).ConfigureAwait(false).GetAwaiter().GetResult();
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
        public void MovieTagsController_DeleteByTagId_ServiceReturnsNull()
        {
            //Arrange
            IEnumerable<MovieTagsDomainModel> movieTags = null;
            Task<IEnumerable<MovieTagsDomainModel>> responseTask = Task.FromResult(movieTags);

            string expectedMessage = Messages.MOVIETAG_DOES_NOT_EXIST;
            int expectedStatusCode = 500;

            ErrorResponseModel errorResponseModel = new ErrorResponseModel();
            errorResponseModel.ErrorMessage = expectedMessage;
            errorResponseModel.StatusCode = System.Net.HttpStatusCode.InternalServerError;

            MovieTagsController movieTagsController = new MovieTagsController(_mockMovieTagService.Object);

            _mockMovieTagService.Setup(x => x.DeleteByTagId(It.IsAny<int>())).Returns(responseTask);

            //Act
            var result = movieTagsController.DeleteByTagId(_movieTagDomainModel.TagId).ConfigureAwait(false).GetAwaiter().GetResult();
            ObjectResult viewResult = (ObjectResult)result;
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(viewResult.Value.ToString(), errorResponseModel.ToString());
            Assert.AreEqual(viewResult.StatusCode, expectedStatusCode);
        }

        [TestMethod]
        public void MovieTagsController_DeleteByMovieId_ReturnsDeletedMovieTags()
        {
            //Arrange

            //Arrange
            List<MovieTagsDomainModel> movieTagsList = new List<MovieTagsDomainModel>();
            movieTagsList.Add(_movieTagDomainModel);
            IEnumerable<MovieTagsDomainModel> movieTags = movieTagsList;
            Task<IEnumerable<MovieTagsDomainModel>> responseTask = Task.FromResult(movieTags);
            int expectedCount = 1;
            int expectedStatusCode = 202;

            MovieTagsController movieTagsController = new MovieTagsController(_mockMovieTagService.Object);

            _mockMovieTagService.Setup(x => x.DeleteByMovieId(It.IsAny<Guid>())).Returns(responseTask);

            //Act

            var result = movieTagsController.DeleteByMovieId(_movieTagDomainModel.MovieId).ConfigureAwait(false).GetAwaiter().GetResult();
            var resultMessage = ((AcceptedResult)result).Value;
            var resultModel = (IEnumerable<MovieTagsDomainModel>)resultMessage;

            //Assert
            Assert.IsNotNull(resultMessage);
            Assert.AreEqual(expectedStatusCode, ((AcceptedResult)result).StatusCode);
            Assert.IsInstanceOfType(resultModel.ToList()[0], typeof(MovieTagsDomainModel));
        }


        [TestMethod]
        public void MovieTagsController_DeleteByMovieId_ThrowsDbUpdateException()
        {
            //Arrange
            string expectedMessage = "Inner exception error message.";
            int expectedStatusCode = 400;
            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);

            MovieTagsController movieTagsController = new MovieTagsController(_mockMovieTagService.Object);

            _mockMovieTagService.Setup(x => x.DeleteByMovieId(It.IsAny<Guid>())).Throws(dbUpdateException);

            //Act

            var result = movieTagsController.DeleteByMovieId(_movieTagDomainModel.MovieId).ConfigureAwait(false).GetAwaiter().GetResult();
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
        public void MovieTagsController_DeleteByMovieId_ServiceReturnsNull()
        {
            //Arrange
            IEnumerable<MovieTagsDomainModel> movieTags = null;
            Task<IEnumerable<MovieTagsDomainModel>> responseTask = Task.FromResult(movieTags);

            string expectedMessage = Messages.MOVIETAG_DOES_NOT_EXIST;
            int expectedStatusCode = 500;

            ErrorResponseModel errorResponseModel = new ErrorResponseModel();
            errorResponseModel.ErrorMessage = expectedMessage;
            errorResponseModel.StatusCode = System.Net.HttpStatusCode.InternalServerError;

            MovieTagsController movieTagsController = new MovieTagsController(_mockMovieTagService.Object);

            _mockMovieTagService.Setup(x => x.DeleteByMovieId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var result = movieTagsController.DeleteByMovieId(_movieTagDomainModel.MovieId).ConfigureAwait(false).GetAwaiter().GetResult();
            ObjectResult viewResult = (ObjectResult)result;
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(viewResult.Value.ToString(), errorResponseModel.ToString());
            Assert.AreEqual(viewResult.StatusCode, expectedStatusCode);
        }

        [TestMethod]
        public void MovieTagsController_DeleteByMovieIdTagId_ReturnsDeletedMovieTags()
        {
            //Arrange
            Task<MovieTagsDomainModel> responseTask = Task.FromResult(_movieTagDomainModel);
            int expectedStatusCode = 202;

            MovieTagsController movieTagsController = new MovieTagsController(_mockMovieTagService.Object);

            _mockMovieTagService.Setup(x => x.DeleteByMovieIdTagId(It.IsAny<Guid>(),It.IsAny<int>())).Returns(responseTask);

            //Act

            var result = movieTagsController.DeleteByMovieIdTagId(_movieTagDomainModel.MovieId,_movieTagDomainModel.TagId).ConfigureAwait(false).GetAwaiter().GetResult();
            var resultMessage = ((AcceptedResult)result).Value;
            var resultModel = (MovieTagsDomainModel)resultMessage;

            //Assert
            Assert.IsNotNull(resultMessage);
            Assert.AreEqual(expectedStatusCode, ((AcceptedResult)result).StatusCode);
            Assert.IsInstanceOfType(resultModel, typeof(MovieTagsDomainModel));
            Assert.AreEqual(resultModel.TagId, _movieTagDomainModel.TagId);
            Assert.AreEqual(resultModel.MovieId, _movieTagDomainModel.MovieId);
        }

        [TestMethod]
        public void MovieTagsController_DeleteByMovieIdTagId_ThrowsDbUpdateException()
        {
            //Arrange
            string expectedMessage = "Inner exception error message.";
            int expectedStatusCode = 400;
            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);

            MovieTagsController movieTagsController = new MovieTagsController(_mockMovieTagService.Object);

            _mockMovieTagService.Setup(x => x.DeleteByMovieIdTagId(It.IsAny<Guid>(),It.IsAny<int>())).Throws(dbUpdateException);

            //Act

            var result = movieTagsController.DeleteByMovieIdTagId(_movieTagDomainModel.MovieId,_movieTagDomainModel.TagId).ConfigureAwait(false).GetAwaiter().GetResult();
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
        public void MovieTagsController_DeleteByMovieIdTagId_ServiceReturnsNull()
        {
            //Arrange
            MovieTagsDomainModel movieTags = null;
            Task<MovieTagsDomainModel> responseTask = Task.FromResult(movieTags);

            string expectedMessage = Messages.MOVIETAG_DOES_NOT_EXIST;
            int expectedStatusCode = 500;

            ErrorResponseModel errorResponseModel = new ErrorResponseModel();
            errorResponseModel.ErrorMessage = expectedMessage;
            errorResponseModel.StatusCode = System.Net.HttpStatusCode.InternalServerError;

            MovieTagsController movieTagsController = new MovieTagsController(_mockMovieTagService.Object);

            _mockMovieTagService.Setup(x => x.DeleteByMovieIdTagId(It.IsAny<Guid>(),It.IsAny<int>())).Returns(responseTask);

            //Act
            var result = movieTagsController.DeleteByMovieIdTagId(_movieTagDomainModel.MovieId,_movieTagDomainModel.TagId).ConfigureAwait(false).GetAwaiter().GetResult();
            ObjectResult viewResult = (ObjectResult)result;
            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(viewResult.Value.ToString(), errorResponseModel.ToString());
            Assert.AreEqual(viewResult.StatusCode, expectedStatusCode);
        }

    }
}
