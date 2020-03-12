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
    public class MovieServicesTests
    {
        private Mock<IMoviesRepository> _mockMoviesRepository;
        private Movie _movie;
        private MovieDomainModel _movieDomainModel;
        private Mock<IMovieTagService> _mockMovieTagService;
        private Mock<IProjectionService> _mockProjectionService;

        [TestInitialize]
        public void TestInitialize()
        {
            _movie = new Movie
            {
                Id = Guid.NewGuid(),
                Current = true,
                Rating = 5,
                Title = "NazivFilma",
                Year = 1999
            };

            _movieDomainModel = new MovieDomainModel
            {
                Id = _movie.Id,
                Current = _movie.Current,
                Rating = 5,
                Title = _movie.Title,
                Year = _movie.Year
            };

            List<Movie> movieList = new List<Movie>();

            movieList.Add(_movie);
            IEnumerable<Movie> movies = movieList;
            Task<IEnumerable<Movie>> responseTask = Task.FromResult(movies);

            _mockMoviesRepository = new Mock<IMoviesRepository>();
            _mockMovieTagService = new Mock<IMovieTagService>();
            _mockProjectionService = new Mock<IProjectionService>();
            

        }



        [TestMethod]
        public void MovieService_GetAll_ReturnsListOfMovies()
        {
            // Arrange

            List<Movie> movieList = new List<Movie>();

            movieList.Add(_movie);
            IEnumerable<Movie> movies = movieList;
            Task<IEnumerable<Movie>> responseTask = Task.FromResult(movies);

            int expectedCount = 1;
            MovieService movieService = new MovieService(_mockMoviesRepository.Object, _mockProjectionService.Object, _mockMovieTagService.Object);
            _mockMoviesRepository.Setup(x => x.GetAll()).Returns(responseTask);

            //Act
            var result = movieService.GetAllMovies().ConfigureAwait(false).GetAwaiter().GetResult().ToList();


            //Assert

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, expectedCount);
            Assert.IsInstanceOfType(result[0], typeof(MovieDomainModel));
            Assert.AreEqual(result[0].Id, _movie.Id);
        }


        [TestMethod]
        public void MovieService_GetAll_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange

            IEnumerable<Movie> nullMovie = null;
            Task<IEnumerable<Movie>> responseTask = Task.FromResult(nullMovie);

            MovieService movieService = new MovieService(_mockMoviesRepository.Object, _mockProjectionService.Object, _mockMovieTagService.Object);
            _mockMoviesRepository.Setup(x => x.GetAll()).Returns(responseTask);

            //Act

            var result = movieService.GetAllMovies().ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void MovieService_GetAllCurrentMovies_ReturnsAllMovies_WhereCurrentISTRUE()
        {
            //Arrange

            List<Movie> movieList = new List<Movie>();

            movieList.Add(_movie);
            IEnumerable<Movie> movies = movieList;

            int expectedCount = 1;
            MovieService movieService = new MovieService(_mockMoviesRepository.Object, _mockProjectionService.Object, _mockMovieTagService.Object);
            _mockMoviesRepository.Setup(x => x.GetCurrentMovies()).Returns(movies);

            //Act

            var result = movieService.GetAllCurrentMovies().ToList();


            //Assert

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, expectedCount);
            Assert.IsInstanceOfType(result[0], typeof(MovieDomainModel));
            Assert.AreEqual(result[0].Id, _movie.Id);
            Assert.AreEqual(result[0].Current, _movie.Current);
        }

        [TestMethod]
        public void MovieService_GetAllCurrentMovies_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange

            List<Movie> movieList = null;
            IEnumerable<Movie> movies = movieList;

            MovieService movieService = new MovieService(_mockMoviesRepository.Object, _mockProjectionService.Object, _mockMovieTagService.Object);
            _mockMoviesRepository.Setup(x => x.GetCurrentMovies()).Returns(movies);

            //Act
            var result = movieService.GetAllCurrentMovies();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void MovieService_GetTopTenMovies_ReturnsTopTenMoviesWithHighestRating()
        {
            // Arrange

            List<Movie> movieList = new List<Movie>();

            movieList.Add(_movie);
            IEnumerable<Movie> movies = movieList;

            int expectedCount = 1;
            MovieService movieService = new MovieService(_mockMoviesRepository.Object, _mockProjectionService.Object, _mockMovieTagService.Object);
            _mockMoviesRepository.Setup(x => x.GetTopTenMoviesByRating()).Returns(movieList);

            //Act
            var result = movieService.GetTopTenMovies().ToList();


            //Assert

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, expectedCount);
            Assert.IsInstanceOfType(result[0], typeof(MovieDomainModel));
            Assert.AreEqual(result[0].Id, _movie.Id);
        }

        [TestMethod]
        public void MovieService_GetTopTenMovies_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange

            List<Movie> movieList = null;
            IEnumerable<Movie> movies = movieList;

            MovieService movieService = new MovieService(_mockMoviesRepository.Object, _mockProjectionService.Object, _mockMovieTagService.Object);
            _mockMoviesRepository.Setup(x => x.GetTopTenMoviesByRating()).Returns(movies);

            //Act
            var result = movieService.GetTopTenMovies();

            //Assert
            Assert.IsNull(result);

        }

        [TestMethod]
        public void MovieService_GetMovieByIdAsync_ReturnsMovieDomainModel()
        {
            //Arrange
            Task<Movie> movie = Task.FromResult(_movie);

            MovieService movieService = new MovieService(_mockMoviesRepository.Object, _mockProjectionService.Object, _mockMovieTagService.Object);
            _mockMoviesRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(movie);

            //Act
            var result = movieService.GetMovieByIdAsync(_movie.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MovieDomainModel));
            Assert.AreEqual(result.Id, _movie.Id);
        }

        [TestMethod]
        public void MovieService_GetMovieByIdAsync_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            Movie nullMovie = null;
            Task<Movie> movie = Task.FromResult(nullMovie);

            MovieService movieService = new MovieService(_mockMoviesRepository.Object, _mockProjectionService.Object, _mockMovieTagService.Object);
            _mockMoviesRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(movie);

            //Act
            var result = movieService.GetMovieByIdAsync(_movie.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void MovieService_AddMovie_ReturnsAddedMovie()
        {
            //Arrange

            MovieService movieService = new MovieService(_mockMoviesRepository.Object, _mockProjectionService.Object, _mockMovieTagService.Object);
            _mockMoviesRepository.Setup(x => x.Insert(It.IsAny<Movie>())).Returns(_movie);
            _mockMoviesRepository.Setup(x => x.Save());

            //Act
            var result = movieService.AddMovie(_movieDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MovieDomainModel));
            Assert.AreEqual(result.Id, _movie.Id);
        }

        [TestMethod]
        public void MovieService_AddMovie_RepositoryReturnsNull_ReturnsNull()
        {
            // Arrange
            Movie nullMovie = null;

            MovieService movieService = new MovieService(_mockMoviesRepository.Object, _mockProjectionService.Object, _mockMovieTagService.Object);
            _mockMoviesRepository.Setup(x => x.Insert(It.IsAny<Movie>())).Returns(nullMovie);
            _mockMoviesRepository.Setup(x => x.Save());

            //Act
            var result = movieService.AddMovie(_movieDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);

        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException))]
        public void MovieService_AddMovie_ThrowsDbUpdateException()
        {
            //Arrange
            MovieService movieService = new MovieService(_mockMoviesRepository.Object, _mockProjectionService.Object, _mockMovieTagService.Object);
            _mockMoviesRepository.Setup(x => x.Insert(It.IsAny<Movie>())).Throws(new DbUpdateException());

            //Act
            var result = movieService.AddMovie(_movieDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void MovieService_UpdateMovie_ReturnsUpdateMovie()
        {
            //Arrange
            Movie movie = new Movie
            {
                Id = _movie.Id,
                Current = _movie.Current,
                Rating = _movie.Rating,
                Year = _movie.Year,
                Title = "PromenjenTitle"
            };
            MovieService movieService = new MovieService(_mockMoviesRepository.Object, _mockProjectionService.Object, _mockMovieTagService.Object);
            _mockMoviesRepository.Setup(x => x.Update(It.IsAny<Movie>())).Returns(movie);

            //Act
            var result = movieService.UpdateMovie(_movieDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MovieDomainModel));
            Assert.AreEqual(result.Id, _movie.Id);
            Assert.AreNotEqual(result.Title, _movie.Title);
        }

        [TestMethod]
        public void MovieService_UpdateMovie_RepositoryReturnsNull_ReturnsNull()
        {
            //Arrange
            Movie movie = null;

            MovieService movieService = new MovieService(_mockMoviesRepository.Object, _mockProjectionService.Object, _mockMovieTagService.Object);
            _mockMoviesRepository.Setup(x => x.Update(It.IsAny<Movie>())).Returns(movie);

            //Act
            var result = movieService.UpdateMovie(_movieDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void MovieService_DeleteMovie_ReturnsDeletedMovie()
        {
            //Arrange

            MovieService movieService = new MovieService(_mockMoviesRepository.Object, _mockProjectionService.Object, _mockMovieTagService.Object);
            _mockMoviesRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(_movie);

            //Act
            var result = movieService.DeleteMovie(_movie.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(MovieDomainModel));
            Assert.AreEqual(result.Id, _movie.Id);

        }

        [TestMethod]
        public void MovieServce_DeleteMovie_RepositoryReturnsNull()
        {
            //Arrange
            Movie nullMovie = null;

            MovieService movieService = new MovieService(_mockMoviesRepository.Object, _mockProjectionService.Object, _mockMovieTagService.Object);
            _mockMoviesRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(nullMovie);

            //Act
            var result = movieService.DeleteMovie(_movie.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(result);
        }
    }
}
