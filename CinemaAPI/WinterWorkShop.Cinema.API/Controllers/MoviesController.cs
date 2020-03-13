using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        private readonly ITagService _tagService;

        private readonly IMovieTagService _movieTagService;

        private readonly IProjectionService _projectionsService;

        public MoviesController(IMovieService movieService, ITagService tagService, IMovieTagService movieTagService, IProjectionService projectionService)
        {
            _movieService = movieService;
            _tagService = tagService;
            _movieTagService = movieTagService;
            _projectionsService = projectionService;
        }

        /// <summary>
        /// Gets Movie by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<MovieDomainModel>> GetAsync(Guid id)
        {
            MovieDomainModel movie;

            movie = await _movieService.GetMovieByIdAsync(id);

            if (movie == null)
            {
                return NotFound(Messages.MOVIE_DOES_NOT_EXIST);
            }

            return Ok(movie);
        }

        [HttpGet]
        [Route("byauditoriumid/{id}")]
        public async Task<ActionResult<IEnumerable<MovieDomainModel>>> GetMovieListByAuditoriumId(int id)
        {
            var projectionData = await _projectionsService.GetProjectionsByAuditoriumId(id);
            if (projectionData.Count() == 0)
            {
                return NotFound(Messages.AUDITORIUM_DOES_NOT_EXIST + " or the selected auditorium has no scheduled projections");
            }

            List<MovieDomainModel> movieList = new List<MovieDomainModel>();
            foreach (ProjectionDomainModel projectionDomainModel in projectionData)
            {
                var movieData = await _movieService.GetMovieByIdAsync(projectionDomainModel.MovieId);
                if (movieData == null)
                {
                    return NotFound(Messages.MOVIE_DOES_NOT_EXIST);
                }
                movieList.Add(movieData);
            }

            return Ok(movieList);
        }

        /// <summary>
        /// Gets all current movies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        [Authorize(Roles = "admin, superUser")]
        public async Task<ActionResult<IEnumerable<MovieDomainModel>>> GetAllAsync()
        {
            IEnumerable<MovieDomainModel> movieDomainModels;

            movieDomainModels = await _movieService.GetAllMovies();

            if (movieDomainModels == null)
            {
                movieDomainModels = new List<MovieDomainModel>();
            }

            return Ok(movieDomainModels);
        }


        [HttpGet]
        [Route("current")]
        public async Task<ActionResult<IEnumerable<MovieDomainModel>>> GetAllCurrentAsync()
        {
            IEnumerable<MovieDomainModel> movieDomainModels;

            movieDomainModels = _movieService.GetAllCurrentMovies();

            if (movieDomainModels == null)
            {
                movieDomainModels = new List<MovieDomainModel>();
            }

            return Ok(movieDomainModels);
        }

        [HttpGet]
        [Route("currentMoviesAndProjections")]
        public async Task<ActionResult<IEnumerable<MovieDomainModel>>> GetAllCurrentMoviesAndProjections()
        {
            IEnumerable<MovieDomainModel> movieWithProjectionsModel;

            movieWithProjectionsModel = _movieService.GetAllCurrentMovies();

            if (movieWithProjectionsModel == null)
            {
                movieWithProjectionsModel = new List<MovieDomainModel>();
            }

            return Ok(movieWithProjectionsModel);
        }


        /// <summary>
        /// Adds a new movie
        /// </summary>
        /// <param name="movieModel"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin, superUser")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]CreateMovieWithTagsModel movieModel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            MovieDomainModel domainModel = new MovieDomainModel
            {
                Current = movieModel.Current,
                Rating = movieModel.Rating,
                Title = movieModel.Title,
                Year = movieModel.Year,
                BannerUrl = movieModel.BannerUrl
            };

            List<TagsDomainModel> tagList = new List<TagsDomainModel>();
            try
            {
                foreach (string tag in movieModel.Tags)
                {
                    TagsDomainModel tagsDomainModel = new TagsDomainModel
                    {
                        Name = tag
                    };

                    TagsDomainModel createdTag = await _tagService.AddTag(tagsDomainModel);
                    if (createdTag != null)
                    {
                        tagList.Add(createdTag);
                    }
                }
            }
            catch (DbUpdateException e)
            { Console.WriteLine(e.Message); }

            MovieDomainModel createMovie;

            try
            {
                createMovie = await _movieService.AddMovie(domainModel);
            }
            catch (DbUpdateException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = e.InnerException.Message ?? e.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            if (createMovie == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MOVIE_CREATION_ERROR,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            foreach (TagsDomainModel tag in tagList)
            {
                MovieTagsDomainModel movieTadDomainModel = new MovieTagsDomainModel
                {
                    MovieId = createMovie.Id,
                    TagId = tag.Id
                };

                await _movieTagService.AddMovieTag(movieTadDomainModel);
            }
            return Created("movies//" + createMovie.Id, createMovie);
        }

        /// <summary>
        /// Updates a movie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="movieModel"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin, superUser")]
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody]MovieModel movieModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MovieDomainModel movieToUpdate;

            movieToUpdate = await _movieService.GetMovieByIdAsync(id);

            if (movieToUpdate == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            movieToUpdate.Title = movieModel.Title;
            movieToUpdate.Current = movieModel.Current;
            movieToUpdate.Year = movieModel.Year;
            movieToUpdate.Rating = movieModel.Rating;

            MovieDomainModel movieDomainModel;
            try
            {
                movieDomainModel = await _movieService.UpdateMovie(movieToUpdate);
            }
            catch (DbUpdateException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = e.InnerException.Message ?? e.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Accepted("movies//" + movieDomainModel.Id, movieDomainModel);

        }

        /// <summary>
        /// Updates a current field in movie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="movieModel"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin, superUser")]
        [HttpPut]
        [Route("changecurrent/{id}")]
        public async Task<ActionResult> PutCurrent(Guid id)
        {

            MovieDomainModel movieToUpdate;

            movieToUpdate = await _movieService.GetMovieByIdAsync(id);
            var movieWithProjections = await _projectionsService.GetProjectionByMovieId(id);

            if (movieToUpdate == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            if (movieWithProjections != null && movieWithProjections.Count() != 0)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST_OR_HAVE_PROJECTIONS,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);
            }

            movieToUpdate.Title = movieToUpdate.Title;
            if (movieToUpdate.Current == true)
            {
                movieToUpdate.Current = false;
            }
            else
            {
                movieToUpdate.Current = true;
            }
            movieToUpdate.Year = movieToUpdate.Year;
            movieToUpdate.Rating = movieToUpdate.Rating;
            movieToUpdate.BannerUrl = movieToUpdate.BannerUrl;

            MovieDomainModel movieDomainModel;
            try
            {
                movieDomainModel = await _movieService.UpdateMovie(movieToUpdate);
            }
            catch (DbUpdateException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = e.InnerException.Message ?? e.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Accepted("movies//" + movieDomainModel.Id, movieDomainModel);

        }

        /// <summary>
        /// Delete a movie by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "admin, superUser")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            MovieDomainModel deletedMovie;
            try
            {
                deletedMovie = await _movieService.DeleteMovie(id);
            }
            catch (DbUpdateException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = e.InnerException.Message ?? e.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            if (deletedMovie == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            return Accepted("movies//" + deletedMovie.Id, deletedMovie);
        }

        [HttpGet]
        [Route("top")]
        public async Task<ActionResult<IEnumerable<MovieDomainModel>>> GetTopTenMovies()
        {
            IEnumerable<MovieDomainModel> movieDomainModels;

            movieDomainModels = _movieService.GetTopTenMovies();

            if (movieDomainModels == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MOVIE_GET_ALL_MOVIES_ERROR,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            return Ok(movieDomainModels);
        }

        [HttpGet]
        [Route("TopByYear/{year}")]
        public async Task<ActionResult<IEnumerable<MovieDomainModel>>> GetMovieListByYear(int year)
        {
            var movieData = _movieService.GetMoviesByYear(year);
            if (movieData == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MOVIE_GET_ALL_MOVIES_ERROR,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            return Ok(movieData);
        }


        [HttpGet]
        [Route("bytag/{tagName}")]
        public async Task<ActionResult<IEnumerable<MovieDomainModel>>> GetMovieListByTagName(string tagName)
        {
            // Prvo se poziva metoda koja preuzima tag objekat po nazivu taga
            TagsDomainModel tagData = await _tagService.GetTagByTagName(tagName);


            if (tagData == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.TAG_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };
                return BadRequest(errorResponse);
            }

            // Nakon toga se poziva Metoda kojoj se prosledjuje tagID, a vraca list MovieTag objekata sa tim tagID
            var movieTagList = await _movieTagService.GetByTagId(tagData.Id);

            // Ovo je lista koja ce sadrzati movieIDeve svakog MovieTag objekta sa zeljenim TagIDem
            List<Guid> movieIdList = new List<Guid>();
            foreach (MovieTagsDomainModel domainModel in movieTagList)
            {
                movieIdList.Add(domainModel.MovieId);

            }

            // Sada se poziva GetMovie metoda za svaki MovieId u listi i dodaje se na listu filmova koji se vracaju kao rezultat
            List<MovieDomainModel> result = new List<MovieDomainModel>();
            MovieDomainModel movieDomainModel;
            foreach (Guid movieId in movieIdList)
            {
                movieDomainModel = await _movieService.GetMovieByIdAsync(movieId);
                result.Add(movieDomainModel);
            }

            return Ok(result);
        }


    }
}
