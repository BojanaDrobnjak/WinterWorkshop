using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatsController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        /// <summary>
        /// Gets all seats
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<SeatDomainModel>>> GetAsync()
        {
            IEnumerable<SeatDomainModel> seatDomainModels;

            seatDomainModels = await _seatService.GetAllAsync();

            if (seatDomainModels == null)
            {
                seatDomainModels = new List<SeatDomainModel>();
            }

            return Ok(seatDomainModels);
        }

        [HttpGet]
        [Route("byauditoriumid/{auditoriumId}")]
        public async Task<ActionResult<IEnumerable<SeatDomainModel>>> GetByAuditoriumId(int auditoriumId)
        {
            IEnumerable<SeatDomainModel> seatDomainModels;

            seatDomainModels = await _seatService.GetSeatsByAuditoriumId(auditoriumId);

            if (seatDomainModels == null)
            {
                seatDomainModels = new List<SeatDomainModel>();
            }

            return Ok(seatDomainModels);
        }

        [HttpGet]
        [Route("numberofseats/{auditoriumId}")]
        public async Task<ActionResult<NumberOfSeatsModel>> GetNumberOfSeats(int auditoriumId)
        {
            NumberOfSeatsModel seatDomainModel;

            seatDomainModel = await _seatService.GetNumberOfSeats(auditoriumId);

            if (seatDomainModel == null)
            {
                 return NotFound(Messages.AUDITORIUM_DOES_NOT_EXIST);
            }

            return Ok(seatDomainModel);
        }
    }
}
