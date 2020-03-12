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
    public class MovieTagsController : Controller
    {
        private readonly IMovieTagService _movieTagService;

        public MovieTagsController(IMovieTagService movieTagService)
        {
            _movieTagService = movieTagService;
        }


        [HttpGet]
        [Route("getbytagid")]
        public async Task<ActionResult<IEnumerable<MovieTagsDomainModel>>> GetByTagIdAsync (int tagId)
        {

            IEnumerable<MovieTagsDomainModel> result = await _movieTagService.GetByTagId(tagId);

            if (result == null)
            {
                return NotFound(Messages.MOVIETAG_DOES_NOT_EXIST);
            }

            return Ok(result);

        }

        [HttpGet]
        [Route("getbymovieid")]
        public async Task<ActionResult<IEnumerable<MovieTagsDomainModel>>> GetByMovieIdAsync(Guid movieId)
        {

            IEnumerable<MovieTagsDomainModel> result;

            result = await _movieTagService.GetByMovieId(movieId);

            if (result == null)
            {
                return NotFound(Messages.MOVIETAG_DOES_NOT_EXIST);
            }

            return Ok(result);

        }

        [HttpGet]
        [Route("all")]
        [Authorize(Roles = "admin, superUser")]
        public async Task<ActionResult<IEnumerable<MovieTagsDomainModel>>> GetAll ()
        {
            IEnumerable<MovieTagsDomainModel> movieTagsDomainModels;

            movieTagsDomainModels = await _movieTagService.GetAllAsync();

            if (movieTagsDomainModels == null)
            {
                movieTagsDomainModels = new List<MovieTagsDomainModel>();
            }

            return Ok(movieTagsDomainModels);
        }



        [HttpPost]
        [Authorize(Roles = "admin, superUser")]

        public async Task<ActionResult> Post (Guid movieId, int tagId)
        {
            MovieTagsDomainModel domainModel = new MovieTagsDomainModel
            {
                MovieId = movieId,
                TagId = tagId
            };

            MovieTagsDomainModel createdMovieTag;

            try
            {
                createdMovieTag = await _movieTagService.AddMovieTag(domainModel);
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
            if (createdMovieTag == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MOVIETAG_CREATION_ERROR,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            return Created("movietags//", createdMovieTag);
        }

        [HttpDelete]
        [Authorize(Roles = "admin, superUser")]
        [Route("deletebytagid")]

        public async Task<ActionResult> DeleteByTagId(int tagId)
        {
            IEnumerable<MovieTagsDomainModel> deletedMovieTag;
            try
            {
                deletedMovieTag = await _movieTagService.DeleteByTagId(tagId);
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
            if (deletedMovieTag == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MOVIETAG_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            return Accepted(deletedMovieTag.ToList());
        }

        [HttpDelete]
        [Authorize(Roles = "admin, superUser")]
        [Route("deletebymovieid")]
        public async Task<ActionResult> DeleteByMovieId(Guid movieId)
        {
            IEnumerable<MovieTagsDomainModel> deletedMovieTag;
            try
            {
                deletedMovieTag = await _movieTagService.DeleteByMovieId(movieId);
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
            if (deletedMovieTag == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MOVIETAG_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            return Accepted(deletedMovieTag.ToList());
        }

        [HttpDelete]
        [Authorize(Roles = "admin, superUser")]
        [Route("deletebymovieidtagid")]
        public async Task<ActionResult> DeleteByMovieIdTagId(Guid movieId, int tagId)
        {
            MovieTagsDomainModel deletedMovieTag;
            try
            {
                deletedMovieTag = await _movieTagService.DeleteByMovieIdTagId(movieId,tagId);
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
            if (deletedMovieTag == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MOVIETAG_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            return Accepted(deletedMovieTag);
        }
    }
}