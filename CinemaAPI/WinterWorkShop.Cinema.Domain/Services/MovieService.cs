using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMoviesRepository _moviesRepository;
        private readonly IProjectionService _projectionService;
        private readonly IMovieTagService _movieTagService;

        public MovieService(IMoviesRepository moviesRepository, IProjectionService projectionService, IMovieTagService movieTagService)
        {
            _moviesRepository = moviesRepository;
            _projectionService = projectionService;
            _movieTagService = movieTagService;
        }

        public async Task<IEnumerable<MovieDomainModel>> GetAllMovies()
        {
            var data = await _moviesRepository.GetAll();

            if (data == null)
            {
                return null;
            }

            List<MovieDomainModel> result = new List<MovieDomainModel>();
            MovieDomainModel model;

            foreach (var item in data)
            {
                model = new MovieDomainModel
                {
                    Current = item.Current,
                    Id = item.Id,
                    Rating = item.Rating ?? 0,
                    Title = item.Title,
                    Year = item.Year,
                    BannerUrl = item.BannerUrl
                };
                result.Add(model);
            }

            return result;

        }

        public IEnumerable<MovieDomainModel> GetAllCurrentMovies()
        {
            var data = _moviesRepository.GetCurrentMovies();

            if (data == null)
            {
                return null;
            }

            List<MovieDomainModel> result = new List<MovieDomainModel>();
            MovieDomainModel model;
            foreach (Movie movie in data)
            {
                List<ProjectionDomainModel> listOfProjs = new List<ProjectionDomainModel>();

                if (movie.Projections != null)
                {
                    foreach (Projection projection in movie.Projections)
                    {
                        ProjectionDomainModel projModel = new ProjectionDomainModel
                        {
                            Id = projection.Id,
                            MovieId = projection.MovieId,
                            MovieTitle = projection.Movie.Title,
                            AuditoriumId = projection.AuditoriumId,
                            ProjectionTime = projection.DateTime,
                        };

                        listOfProjs.Add(projModel);
                    }
                }

                    model = new MovieDomainModel
                    {
                        Current = movie.Current,
                        Id = movie.Id,
                        Rating = movie.Rating ?? 0,
                        Title = movie.Title,
                        Year = movie.Year,
                        Projections = listOfProjs,
                        BannerUrl = movie.BannerUrl
                    };
                    result.Add(model);
            }

            return result;

        }


        public IEnumerable<MovieDomainModel> GetMoviesByYear(int movieYear)
        {
            var data = _moviesRepository.GetMoviesByYear(movieYear);

            if (data == null)
            {
                return null;
            }

            List<MovieDomainModel> result = new List<MovieDomainModel>();
            MovieDomainModel model;
            foreach (Movie movie in data)
            {
                List<ProjectionDomainModel> listOfProjs = new List<ProjectionDomainModel>();

                if (movie.Projections != null)
                {
                    foreach (Projection projection in movie.Projections)
                    {
                        ProjectionDomainModel projModel = new ProjectionDomainModel
                        {
                            Id = projection.Id,
                            MovieId = projection.MovieId,
                            MovieTitle = projection.Movie.Title,
                            AuditoriumId = projection.AuditoriumId,
                            ProjectionTime = projection.DateTime,
                        };

                        listOfProjs.Add(projModel);
                    }
                }

                model = new MovieDomainModel
                {
                    Current = movie.Current,
                    Id = movie.Id,
                    Rating = movie.Rating ?? 0,
                    Title = movie.Title,
                    Year = movie.Year,
                    Projections = listOfProjs
                };
                result.Add(model);
            }

            return result;

        }

        public IEnumerable<MovieDomainModel> GetTopTenMovies()
        {

            var data = _moviesRepository.GetTopTenMoviesByRating();

            if (data == null)
            {
                return null;
            }

            List<MovieDomainModel> result = new List<MovieDomainModel>();
            MovieDomainModel model;

               foreach (var item in data)
            {
                    model = new MovieDomainModel
                    {
                        Current = item.Current,
                        Id = item.Id,
                        Rating = item.Rating ?? 0,
                        Title = item.Title,
                        Year = item.Year
                    };
                    result.Add(model);
            }

            return result;

        }

        public async Task<MovieDomainModel> GetMovieByIdAsync(Guid id)
        {
            var data = await _moviesRepository.GetByIdAsync(id);

            if (data == null)
            {
                return null;
            }

            MovieDomainModel domainModel = new MovieDomainModel
            {
                Id = data.Id,
                Current = data.Current,
                Rating = data.Rating ?? 0,
                Title = data.Title,
                Year = data.Year
            };

            return domainModel;
        }



        public async Task<MovieDomainModel> AddMovie(MovieDomainModel newMovie)
        {
            Movie movieToCreate = new Movie()
            {
                Title = newMovie.Title,
                Current = newMovie.Current,
                Year = newMovie.Year,
                Rating = newMovie.Rating,
                BannerUrl = newMovie.BannerUrl
            };

            var data = _moviesRepository.Insert(movieToCreate);
            if (data == null)
            {
                return null;
            }

            _moviesRepository.Save();

            MovieDomainModel domainModel = new MovieDomainModel()
            {
                Id = data.Id,
                Title = data.Title,
                Current = data.Current,
                Year = data.Year,
                Rating = data.Rating ?? 0
            };

            return domainModel;
        }

        public async Task<MovieDomainModel> UpdateMovie(MovieDomainModel updateMovie) 
        {
            Movie movie = new Movie()
            {
                Id = updateMovie.Id,
                Title = updateMovie.Title,
                Current = updateMovie.Current,
                Year = updateMovie.Year,
                Rating = updateMovie.Rating
            };
            
            var data = _moviesRepository.Update(movie);

            if (data == null)
            {
                return null;
            }
            _moviesRepository.Save();

            MovieDomainModel domainModel = new MovieDomainModel()
            {
                Id = data.Id,
                Title = data.Title,
                Current = data.Current,
                Year = data.Year,
                Rating = data.Rating ?? 0
            };

            return domainModel;
        }

        public async Task<MovieDomainModel> DeleteMovie(Guid movieId)
        {

            var deletedMovieTags = await _movieTagService.DeleteByMovieId(movieId);

            if (deletedMovieTags == null)
            {
                return null;
            }

            var deletedProjectionsData = await _projectionService.DeleteByMovieId(movieId);

            if (deletedProjectionsData == null)
            {
                return null;
            }

            var deletedMovieData = _moviesRepository.Delete(movieId);

            if (deletedMovieData == null)
            {
                return null;
            }

            _moviesRepository.Save();

            MovieDomainModel domainModel = new MovieDomainModel
            {
                Id = deletedMovieData.Id,
                Title = deletedMovieData.Title,
                Current = deletedMovieData.Current,
                Year = deletedMovieData.Year,
                Rating = deletedMovieData.Rating ?? 0

            };

            return domainModel;
        }
    }
}
