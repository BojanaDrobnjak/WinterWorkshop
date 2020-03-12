using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectionsController : ControllerBase
    {
        private readonly IProjectionService _projectionService;
        private readonly IMovieService _movieService;
        private readonly IAuditoriumService _auditoriumService;

        public ProjectionsController(IProjectionService projectionService, IMovieService movieService, IAuditoriumService auditoriumService)
        {
            _auditoriumService = auditoriumService;
            _projectionService = projectionService;
            _movieService = movieService;
        }

        /// <summary>
        /// Gets all projections
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "admin, superUser")]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<ProjectionDomainModel>>> GetAsync()
        {
            IEnumerable<ProjectionDomainModel> projectionDomainModels;
           
             projectionDomainModels = await _projectionService.GetAllAsync();            

            if (projectionDomainModels == null)
            {
                projectionDomainModels = new List<ProjectionDomainModel>();
            }

            return Ok(projectionDomainModels);
        }

        /// <summary>
        /// Adds a new projection
        /// </summary>
        /// <param name="projectionModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "admin, superUser")]
        [Route("")]
        public async Task<ActionResult<ProjectionDomainModel>> PostAsync(CreateProjectionModel projectionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (projectionModel.ProjectionTime < DateTime.Now)
            {
                ModelState.AddModelError(nameof(projectionModel.ProjectionTime), Messages.PROJECTION_IN_PAST);
                return BadRequest(ModelState);
            }

            ProjectionDomainModel domainModel = new ProjectionDomainModel
            {
                AuditoriumId = projectionModel.AuditoriumId,
                MovieId = projectionModel.MovieId,
                ProjectionTime = projectionModel.ProjectionTime
            };

            CreateProjectionResultModel createProjectionResultModel;

            try
            {
                createProjectionResultModel = await _projectionService.CreateProjection(domainModel);
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

            if (!createProjectionResultModel.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = createProjectionResultModel.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);                
            }

            return Created("projections//" + createProjectionResultModel.Projection.Id, createProjectionResultModel.Projection);
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        [Route("{id}")]


        public async Task<ActionResult> Delete (Guid id)
        {
            ProjectionDomainModel deletedProjection;

            try
            {
                deletedProjection = await _projectionService.DeleteProjection(id);
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

            if (deletedProjection == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.PROJECTION_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            return Accepted("movies//" + deletedProjection.Id, deletedProjection);
        }


        [Authorize(Roles = "admin")]
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody]ProjectionModel projectionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProjectionDomainModel projectionToUpdate;

            projectionToUpdate = await _projectionService.GetProjectionByIdAsync(id);

            if (projectionToUpdate == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.PROJECTION_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            projectionToUpdate.MovieId = projectionModel.MovieId;
            projectionToUpdate.AuditoriumId = projectionModel.AuditoriumId;
            projectionToUpdate.ProjectionTime = projectionModel.ProjectionTime;

            ProjectionDomainModel projectionDomainModel;
            try
            {
                projectionDomainModel = await _projectionService.UpdateProjection(projectionToUpdate);
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

            return Accepted("movies//" + projectionDomainModel.Id, projectionDomainModel);

        }

        [HttpGet]
        [Route("bymovieid/{id}")]
        public async Task<ActionResult<IEnumerable<ProjectionDomainModel>>> GetProjectionsByMovieId(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IEnumerable<ProjectionDomainModel> projectionDomainModels;
            projectionDomainModels = await _projectionService.GetProjectionByMovieId(id);


            if (projectionDomainModels == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.TAG_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };
            }

            return Ok(projectionDomainModels);
        }

        [HttpGet]
        [Route("byprojectionid/{id}")]
        public async Task<ActionResult<ProjectionDomainModel>> GetProjectionsById(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ProjectionDomainModel projectionDomainModels;
            projectionDomainModels = await _projectionService.GetProjectionByIdAsync(id);


            if (projectionDomainModels == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.PROJECTION_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };
            }

            return Ok(projectionDomainModels);
        }

        [HttpGet]
        [Route("filter")]
        public async Task<ActionResult<IEnumerable<ProjectionDomainModel>>> projectionsByFilters (int cinemaId, int auditoriumId, Guid movieId, DateTime dateTime)
        {

            ProjectionFilterModel filterModel = new ProjectionFilterModel()
            {
                CinemaId = cinemaId,
                AuditoriumId = auditoriumId,
                MovieId = movieId,
                DateTime = dateTime
            };

            if ( filterModel.CinemaId > 0)
            {
                var auditoriumsByCinemaId = await _auditoriumService.GetByCinemaId(filterModel.CinemaId);
                if (auditoriumsByCinemaId == null)
                {
                    ErrorResponseModel errorResponse = new ErrorResponseModel
                    {
                        ErrorMessage = Messages.CINEMA_DOES_NOT_EXIST,
                        StatusCode = System.Net.HttpStatusCode.InternalServerError
                    };
                }

                List<ProjectionDomainModel> projectionsByCinemaId = new List<ProjectionDomainModel>();
                foreach(AuditoriumDomainModel auditoriumDomainModel in auditoriumsByCinemaId)
                {
                    var projections = await _projectionService.GetProjectionsByAuditoriumId(auditoriumDomainModel.Id);
                    projectionsByCinemaId.AddRange(projections);
                }

                if (filterModel.AuditoriumId > 0)
                {
                    List<ProjectionDomainModel> projectionsByAuditoriumId = new List<ProjectionDomainModel>();
                    foreach(ProjectionDomainModel projectionDomainModel in projectionsByCinemaId)
                    {
                        if (projectionDomainModel.AuditoriumId == filterModel.AuditoriumId)
                        {
                            projectionsByAuditoriumId.Add(projectionDomainModel);
                        }
                    }



                    if (filterModel.MovieId != null && filterModel.MovieId != Guid.Empty)
                    {
                        List<ProjectionDomainModel> projectionsByMovieId = new List<ProjectionDomainModel>();
                        foreach(ProjectionDomainModel projectionDomainModel in projectionsByAuditoriumId)
                        {
                            if (projectionDomainModel.MovieId == filterModel.MovieId)
                            {
                                projectionsByMovieId.Add(projectionDomainModel);
                            }
                        }

                        if (filterModel.DateTime != DateTime.MinValue)
                        {
                            List<ProjectionDomainModel> projectionsByDate = new List<ProjectionDomainModel>();
                            foreach (ProjectionDomainModel projectionDomainModel in projectionsByMovieId)
                            {
                                if (projectionDomainModel.ProjectionTime.Date == filterModel.DateTime.Date)
                                {
                                    projectionsByDate.Add(projectionDomainModel);
                                }
                            }
                            return Ok(projectionsByDate);
                        }

                        return Ok(projectionsByMovieId);
                    }

                    return Ok(projectionsByAuditoriumId);
                }

                return Ok(projectionsByCinemaId);
                
            }
            else
            {
                    var result = _movieService.GetAllCurrentMovies();
                    if (result == null)
                    {
                        return NotFound(Messages.PROJECTION_GET_ALL_PROJECTIONS_ERROR);
                    }

                    return Ok(result);
            }
        }
    }
}