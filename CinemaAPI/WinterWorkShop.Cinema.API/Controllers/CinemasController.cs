using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CinemasController : ControllerBase
    {
        private readonly ICinemaService _cinemaService;

        public CinemasController(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
        }

        /// <summary>
        /// Gets all cinemas
        /// </summary>
        /// <returns>List of cinemas</returns>
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<CinemaDomainModel>>> GetAsync()
        {
            IEnumerable<CinemaDomainModel> cinemaDomainModels;

            cinemaDomainModels = await _cinemaService.GetAllAsync();

            if (cinemaDomainModels == null)
            {
                cinemaDomainModels = new List<CinemaDomainModel>();
            }

            return Ok(cinemaDomainModels);
        }


        // Metoda ce napraviti bioskop sa auditorijumom samo ako su joj prosledjeni svi podaci za auditorijum( Ime Auditorijuma, broj redova, i broj kolona)
        // ukoliko bar jedan podatak nije prosledjen, napravice samo bioskop sa datim imenom bioskopa i ignorisace podatke za auditorijum
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody]CinemaModel cinemaModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CinemaDomainModel cinemaWithoutAuditorium = new CinemaDomainModel()
            {
                Name = cinemaModel.Name
            };
            CreateCinemaWithAuditoriumModel cinemaWithAuditoriumModel = new CreateCinemaWithAuditoriumModel
            {
               AuditoriumName = cinemaModel.auditName,
               CinemaName = cinemaModel.Name,
               NumberOfRows = cinemaModel.seatRows,
               NumberOfColumns = cinemaModel.numberOfSeats
            };

            CreateCinemaResultModel createCinema;

            try
            {
                if (cinemaModel.auditName != null && cinemaModel.numberOfSeats > 0 && cinemaModel.seatRows > 0)
                {
                    createCinema = await _cinemaService.AddCinemaWithAuditorium(cinemaWithAuditoriumModel);
                }
                else
                {
                    createCinema = await _cinemaService.AddCinema(cinemaWithoutAuditorium);
                }
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

            if (createCinema.IsSuccessful != true)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = createCinema.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Created("cinemas//" + createCinema.Cinema.Id, createCinema.Cinema);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<CinemaDomainModel>> GetAsync(int id)
        {
            CinemaDomainModel cinema;

            cinema = await _cinemaService.GetCinemaByIdAsync(id);

            if (cinema == null)
            {
                return NotFound(Messages.CINEMA_DOES_NOT_EXIST);
            }

            return Ok(cinema);
        }

        [HttpDelete]
        [Route("{cinemaId}")]
        public async Task<ActionResult> Delete (int cinemaId)
        {
            CinemaDomainModel deletedCinema;

            try
            {
                deletedCinema = await _cinemaService.DeleteCinema(cinemaId);
            }
            catch(DbUpdateException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = e.InnerException.Message ?? e.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);
            }
            catch(ArgumentNullException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.CINEMA_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            if (deletedCinema == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.CINEMA_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            return Accepted("Cinema//" + deletedCinema.Id, deletedCinema);
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]CinemaModel cinemaModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CinemaDomainModel cinemaToUpdate;

            cinemaToUpdate = await _cinemaService.GetCinemaByIdAsync(id);

            if (cinemaToUpdate == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.CINEMA_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            cinemaToUpdate.Name = cinemaModel.Name;

            CinemaDomainModel cinemaDomainModel;
            try
            {
                cinemaDomainModel = await _cinemaService.UpdateCinema(cinemaToUpdate);
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

            return Accepted("cinemas//" + cinemaDomainModel.Id, cinemaDomainModel);

        }
    }
}
