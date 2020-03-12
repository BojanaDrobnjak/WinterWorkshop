using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Services;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class MovieTagServiceTests
    {
        private Mock<IMovieTagsRepository> _mockMovieTagsRepostitory;
        private MovieTag _movieTag;
        private MovieTagsDomainModel _movieTagDomainModel;

        [TestInitialize]
        public void TestInitialize()
        {

            _movieTag = new MovieTag
            {
                MovieId = Guid.NewGuid(),
                Tagid = 1,
                Movie = new Movie(),
                Tag = new Tag()
            };

            _movieTagDomainModel = new MovieTagsDomainModel
            {
                MovieId = _movieTag.MovieId,
                TagId = 1,
                MovieTitle = "NazivFilma",
                TagName = "NazivTaga"
            };

            List<MovieTag> movieTagsList = new List<MovieTag>();

            movieTagsList.Add(_movieTag);
            IEnumerable<MovieTag> movieTags = movieTagsList;
            Task<IEnumerable<MovieTag>> responseTask = Task.FromResult(movieTags);

            _mockMovieTagsRepostitory = new Mock<IMovieTagsRepository>();

        }

        [TestMethod]
        public void MovieTagsService_GetAllAsync_ReturnsListOfAllMovieTags()
        {
            //Arrange

            List<MovieTag> movieTagsList = new List<MovieTag>();

            movieTagsList.Add(_movieTag);
            IEnumerable<MovieTag> movieTags = movieTagsList;
            Task<IEnumerable<MovieTag>> responseTask = Task.FromResult(movieTags);

            int expectedCount = 1;
            MovieTagService movieTagService = new MovieTagService(_mockMovieTagsRepostitory.Object);

            _mockMovieTagsRepostitory.Setup(x => x.GetAll()).Returns(responseTask);

            //Act

            var resultAction = movieTagService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult().ToList();

            //Assert

            Assert.IsNotNull(resultAction);
            Assert.AreEqual(resultAction.Count, expectedCount);
            Assert.IsInstanceOfType(resultAction[0], typeof(MovieTagsDomainModel));
            Assert.AreEqual(resultAction[0].MovieId, _movieTag.MovieId);
            Assert.AreEqual(resultAction[0].TagId, _movieTag.Tagid);

        }

        [TestMethod]
        public void MovieTagsService_GetAllAsync_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange

            IEnumerable<MovieTag> movieTags = null;
            Task<IEnumerable<MovieTag>> responseTask = Task.FromResult(movieTags);

            MovieTagService movieTagService = new MovieTagService(_mockMovieTagsRepostitory.Object);

            _mockMovieTagsRepostitory.Setup(x => x.GetAll()).Returns(responseTask);

            //Act

            var resultAction = movieTagService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert

            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void MovieTagsService_GetByTagId_ReturnsListOfAllMovieTags()
        {
            //Arrange

            List<MovieTag> movieTagsList = new List<MovieTag>();

            movieTagsList.Add(_movieTag);
            IEnumerable<MovieTag> movieTags = movieTagsList;
            Task<IEnumerable<MovieTag>> responseTask = Task.FromResult(movieTags);

            int expectedCount = 1;
            MovieTagService movieTagService = new MovieTagService(_mockMovieTagsRepostitory.Object);

            _mockMovieTagsRepostitory.Setup(x => x.GetByTagId(It.IsAny<int>())).Returns(responseTask);

            //Act

            var resultAction = movieTagService.GetByTagId(_movieTag.Tagid).ConfigureAwait(false).GetAwaiter().GetResult().ToList();

            //Assert

            Assert.IsNotNull(resultAction);
            Assert.AreEqual(resultAction.Count, expectedCount);
            Assert.IsInstanceOfType(resultAction[0], typeof(MovieTagsDomainModel));
            Assert.AreEqual(resultAction[0].MovieId, _movieTag.MovieId);
            Assert.AreEqual(resultAction[0].TagId, _movieTag.Tagid);

        }


        [TestMethod]
        public void MovieTagsService_GetByTagId_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange

            IEnumerable<MovieTag> movieTags = null;
            Task<IEnumerable<MovieTag>> responseTask = Task.FromResult(movieTags);

            MovieTagService movieTagService = new MovieTagService(_mockMovieTagsRepostitory.Object);

            _mockMovieTagsRepostitory.Setup(x => x.GetByTagId(It.IsAny<int>())).Returns(responseTask);

            //Act

            var resultAction = movieTagService.GetByTagId(_movieTag.Tagid).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert

            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void MovieTagsService_GetByMovieId_ReturnsListOfAllMovieTags()
        {
            //Arrange

            List<MovieTag> movieTagsList = new List<MovieTag>();

            movieTagsList.Add(_movieTag);
            IEnumerable<MovieTag> movieTags = movieTagsList;
            Task<IEnumerable<MovieTag>> responseTask = Task.FromResult(movieTags);

            int expectedCount = 1;
            MovieTagService movieTagService = new MovieTagService(_mockMovieTagsRepostitory.Object);

            _mockMovieTagsRepostitory.Setup(x => x.GetByMovieId(It.IsAny<Guid>())).Returns(responseTask);

            //Act

            var resultAction = movieTagService.GetByMovieId(_movieTag.MovieId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();

            //Assert

            Assert.IsNotNull(resultAction);
            Assert.AreEqual(resultAction.Count, expectedCount);
            Assert.IsInstanceOfType(resultAction[0], typeof(MovieTagsDomainModel));
            Assert.AreEqual(resultAction[0].MovieId, _movieTag.MovieId);
            Assert.AreEqual(resultAction[0].TagId, _movieTag.Tagid);

        }

        [TestMethod]
        public void MovieTagsService_GetByMovieId_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange

            IEnumerable<MovieTag> movieTags = null;
            Task<IEnumerable<MovieTag>> responseTask = Task.FromResult(movieTags);

            MovieTagService movieTagService = new MovieTagService(_mockMovieTagsRepostitory.Object);

            _mockMovieTagsRepostitory.Setup(x => x.GetByMovieId(It.IsAny<Guid>())).Returns(responseTask);

            //Act

            var resultAction = movieTagService.GetByMovieId(_movieTag.MovieId).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert

            Assert.IsNull(resultAction);
        }


        [TestMethod]
        public void MovieTagsService_GetByMovieIdTagId_ReturnsMovieTag()
        {
            //Arrange

            Task<MovieTag> movieTag = Task.FromResult(_movieTag);

            MovieTagService movieTagService = new MovieTagService(_mockMovieTagsRepostitory.Object);

            _mockMovieTagsRepostitory.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<int>())).Returns(movieTag);

            //Act

            var result = movieTagService.GetByMovieIdTagId(_movieTag.MovieId, _movieTag.Tagid).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MovieTagsDomainModel));
            Assert.AreEqual(result.MovieId, _movieTag.MovieId);
            Assert.AreEqual(result.TagId, _movieTag.Tagid);

        }


        [TestMethod]
        public void MovieTagsService_GetByMovieIdTagId_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange

            MovieTag movieTagNull = null;
            Task<MovieTag> movieTag = Task.FromResult(movieTagNull);

            MovieTagService movieTagService = new MovieTagService(_mockMovieTagsRepostitory.Object);

            _mockMovieTagsRepostitory.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<int>())).Returns(movieTag);

            //Act

            var result = movieTagService.GetByMovieIdTagId(_movieTag.MovieId, _movieTag.Tagid).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert

            Assert.IsNull(result);

        }

        [TestMethod]
        public void MovieTagsService_AddMovieTag_ReturnsInsertedMovieTag()
        {
            //Arrange

            MovieTagService movieTagService = new MovieTagService(_mockMovieTagsRepostitory.Object);

            _mockMovieTagsRepostitory.Setup(x => x.Insert(It.IsAny<MovieTag>())).Returns(_movieTag);

            //Act

            var result = movieTagService.AddMovieTag(_movieTagDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MovieTagsDomainModel));
            Assert.AreEqual(_movieTagDomainModel.MovieId, result.MovieId);
            Assert.AreEqual(_movieTagDomainModel.TagId, result.TagId);
        }

        [TestMethod]
        public void MovieTagsService_AddMovieTag_RepositoryReturnsNull_ReturnNull()
        {
            //Arrange

            MovieTag movieTagNull = null;

            MovieTagService movieTagService = new MovieTagService(_mockMovieTagsRepostitory.Object);

            _mockMovieTagsRepostitory.Setup(x => x.Insert(It.IsAny<MovieTag>())).Returns(movieTagNull);

            //Act

            var result = movieTagService.AddMovieTag(_movieTagDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert

            Assert.IsNull(result);
        }

        
        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void MovieTagsService_AddMovieTag_ThrowsDbUpdateException()
        {
            //Arrange

            MovieTagService movieTagService = new MovieTagService(_mockMovieTagsRepostitory.Object);

            _mockMovieTagsRepostitory.Setup(x => x.Insert(It.IsAny<MovieTag>())).Throws(new DbUpdateException());

            //Act

            var result = movieTagService.AddMovieTag(_movieTagDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void MovieTagsService_DeleteByTagId_ReturnsDeletedMovieTag()
        {
            //Arrange

            List<MovieTag> movieTagsList = new List<MovieTag>();
            movieTagsList.Add(_movieTag);
            IEnumerable<MovieTag> movieTags = movieTagsList;
            Task<IEnumerable<MovieTag>> responseTask = Task.FromResult(movieTags);


            int expectedCount = 1;
            MovieTagService movieTagService = new MovieTagService(_mockMovieTagsRepostitory.Object);

            _mockMovieTagsRepostitory.Setup(x => x.DeleteByTagId(It.IsAny<int>())).Returns(responseTask);

            //Act

            var result = movieTagService.DeleteByTagId(_movieTag.Tagid).ConfigureAwait(false).GetAwaiter().GetResult().ToList();

            //Assert

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, expectedCount);
            Assert.IsInstanceOfType(result[0], typeof(MovieTagsDomainModel));
            Assert.AreEqual(result[0].TagId, _movieTag.Tagid);
            Assert.AreEqual(result[0].MovieId, _movieTag.MovieId);
        }

        [TestMethod]
        public void MovieTagsService_DeleteByTagId_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            IEnumerable<MovieTag> movieTags = null;
            Task<IEnumerable<MovieTag>> responseTask = Task.FromResult(movieTags);

            MovieTagService movieTagService = new MovieTagService(_mockMovieTagsRepostitory.Object);
            _mockMovieTagsRepostitory.Setup(x => x.DeleteByTagId(It.IsAny<int>())).Returns(responseTask);

            //Act
            var result = movieTagService.DeleteByTagId(_movieTag.Tagid).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void MovieTagsService_DeleteByMovieId_ReturnsDeletedMovieTag()
        {
            //Arrange

            List<MovieTag> movieTagsList = new List<MovieTag>();
            movieTagsList.Add(_movieTag);
            IEnumerable<MovieTag> movieTags = movieTagsList;
            Task<IEnumerable<MovieTag>> responseTask = Task.FromResult(movieTags);


            int expectedCount = 1;
            MovieTagService movieTagService = new MovieTagService(_mockMovieTagsRepostitory.Object);

            _mockMovieTagsRepostitory.Setup(x => x.DeleteByMovieId(It.IsAny<Guid>())).Returns(responseTask);

            //Act

            var result = movieTagService.DeleteByMovieId(_movieTag.MovieId).ConfigureAwait(false).GetAwaiter().GetResult().ToList();

            //Assert

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, expectedCount);
            Assert.IsInstanceOfType(result[0], typeof(MovieTagsDomainModel));
            Assert.AreEqual(result[0].TagId, _movieTag.Tagid);
            Assert.AreEqual(result[0].MovieId, _movieTag.MovieId);
        }

        [TestMethod]
        public void MovieTagsService_DeleteByMovieId_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            IEnumerable<MovieTag> movieTags = null;
            Task<IEnumerable<MovieTag>> responseTask = Task.FromResult(movieTags);

            MovieTagService movieTagService = new MovieTagService(_mockMovieTagsRepostitory.Object);
            _mockMovieTagsRepostitory.Setup(x => x.DeleteByMovieId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var result = movieTagService.DeleteByMovieId(_movieTag.MovieId).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void MovieTagsService_DeleteByMovieIdTagId_ReturnsDeletedMovieTag()
        {
            //Arrange
            Task<MovieTag> movieTag = Task.FromResult(_movieTag);

            MovieTagService movieTagService = new MovieTagService(_mockMovieTagsRepostitory.Object);

            _mockMovieTagsRepostitory.Setup(x => x.DeleteById(It.IsAny<Guid>(), It.IsAny<int>())).Returns(movieTag);

            //Act

            var result = movieTagService.DeleteByMovieIdTagId(_movieTag.MovieId,_movieTag.Tagid).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MovieTagsDomainModel));
            Assert.AreEqual(result.TagId, _movieTag.Tagid);
            Assert.AreEqual(result.MovieId, _movieTag.MovieId);
        }

        [TestMethod]
        public void MovieTagsService_DeleteByMovieIdTagId_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            MovieTag movieTagNull = null;
            Task<MovieTag> movieTag = Task.FromResult(movieTagNull);

            MovieTagService movieTagService = new MovieTagService(_mockMovieTagsRepostitory.Object);

            _mockMovieTagsRepostitory.Setup(x => x.DeleteById(It.IsAny<Guid>(), It.IsAny<int>())).Returns(movieTag);

            //Act

            var result = movieTagService.DeleteByMovieIdTagId(_movieTag.MovieId, _movieTag.Tagid).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert

            Assert.IsNull(result);
        }

    }
}
