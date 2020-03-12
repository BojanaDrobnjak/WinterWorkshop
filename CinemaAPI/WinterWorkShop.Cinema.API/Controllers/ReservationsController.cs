using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]

    public class ReservationsController : ControllerBase
    {

        private readonly IReservationService _reservationService;
        private readonly IUserService _userService;
        private readonly IProjectionService _projectionService;

        public ReservationsController(IReservationService reservationService, IUserService userService, IProjectionService projectionService)
        {
            _reservationService = reservationService;
            _userService = userService;
            _projectionService = projectionService;
        }

        [HttpGet]
        [Route("get/all")]
        [Authorize(Roles = "admin, superUser")]
        public async Task<ActionResult<IEnumerable<ReservationDomainModel>>> GetAllReservations()
        {
            IEnumerable<ReservationDomainModel> reservationDomainModels;

            reservationDomainModels = await _reservationService.GetAllReservations();

            if (reservationDomainModels == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.RESERVATION_GET_ALL_ERROR,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }


            return Ok(reservationDomainModels);

        }

        [HttpGet]
        [Route("getbyprojectionid/{id}")]
        [Authorize]

        public async Task<ActionResult<IEnumerable<ReservationDomainModel>>> GetAllByProjectionId(Guid id)
        {
            IEnumerable<ReservationDomainModel> reservationDomainModels;

            reservationDomainModels = await _reservationService.GetByProjectionId(id);

            //if (reservationDomainModels.Count() == 0)
            //{
            //    ErrorResponseModel errorResponse = new ErrorResponseModel
            //    {
            //        ErrorMessage = Messages.RESERVATION_GET_ALL_BY_PROJECTIONID_ERROR,
            //        StatusCode = System.Net.HttpStatusCode.InternalServerError
            //    };

            //    return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            //}

            return Ok(reservationDomainModels);
        }

        [HttpGet]
        [Route("get/projectioniduserid")]
        [Authorize]

        public async Task<ActionResult<IEnumerable<ReservationDomainModel>>> GetByProjectionIdUserId(Guid projectionId, Guid userId)
        {
            IEnumerable<ReservationDomainModel> reservationDomainModels;

            reservationDomainModels = await _reservationService.GetByProjectionIdUserId(projectionId, userId);
            
            if (reservationDomainModels.Count() == 0)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.RESERVATION_GET_ALL_BY_PROJECTIONID_USERID_ERROR,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            return Ok(reservationDomainModels);
        }

        [HttpGet]
        [Route("byuserid/{userId}")]
        [Authorize]

        public async Task<ActionResult<IEnumerable<ReservationDomainModel>>> GetByUserId(Guid userId)
        {
            IEnumerable<ReservationDomainModel> reservationDomainModels;

            reservationDomainModels = await _reservationService.GetByUserId(userId);

            if (reservationDomainModels.Count() == 0)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.RESERVATION_GET_ALL_BY_USERID_ERROR,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            return Ok(reservationDomainModels);
        }

        [HttpGet]
        [Route("list-byuserid/{userId}")]
        [Authorize]

        public async Task<ActionResult<IEnumerable<ReservationDomainModel>>> GetListByUserId(Guid userId)
        {
            IEnumerable<ReservationDomainModel> reservationDomainModels;

            reservationDomainModels = await _reservationService.GetByUserId(userId);

            if (reservationDomainModels.Count() == 0)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.RESERVATION_GET_ALL_BY_USERID_ERROR,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }else
            {
                IEnumerable<ProjectionDomainModel> projections =  await _projectionService.GetProjectionsByReservationIds(reservationDomainModels);
                return Ok(projections);
            }
        }

        [HttpDelete]
        [Route("delete/useridprojectionid")]
        [Authorize]

        public async Task<ActionResult<IEnumerable<ReservationDomainModel>>> DeleteByUserIdProjectionId(Guid userId, Guid projectionId)
        {
            IEnumerable<ReservationDomainModel> deletedReservations;
            try
            {
                deletedReservations = await _reservationService.DeleteByUserIdProjectionId(userId, projectionId);
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
            if (deletedReservations.Count() == 0)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.RESERVATION_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            return Accepted(deletedReservations);

        }


        [HttpDelete]
        [Route("delete/projectionid")]
        [Authorize]

        public async Task<ActionResult<IEnumerable<ReservationDomainModel>>> DeleteByProjectionId(Guid projectionId)
        {
            IEnumerable<ReservationDomainModel> deletedReservations;
            try
            {
                deletedReservations = await _reservationService.DeleteByProjectionId(projectionId);
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
            if (deletedReservations.Count() == 0)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.RESERVATION_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            return Accepted(deletedReservations);

        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<ReservationDomainModel>> PostAsync ([FromBody]CreateReservationModel reservationModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<ReservationDomainModel> reservationResultList = new List<ReservationDomainModel>();
            int bonusPointsToAdd = 0;
            foreach (Guid seatId in reservationModel.SeatIds)
            {
                bonusPointsToAdd++;
                ReservationDomainModel domainModel = new ReservationDomainModel
                {
                    ProjectionId = reservationModel.ProjectionId,
                    SeatId = seatId,
                    UserId = reservationModel.UserId
                };

                CreateReservationResultModel createReservationResultModel;
                try
                {
                    createReservationResultModel = await _reservationService.AddReservation(domainModel);
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

                if (!createReservationResultModel.IsSuccessful)
                {
                    ErrorResponseModel errorResponse = new ErrorResponseModel
                    {
                        ErrorMessage = createReservationResultModel.ErrorMessage,
                        StatusCode = System.Net.HttpStatusCode.BadRequest
                    };

                    return BadRequest(errorResponse);
                }
                reservationResultList.Add(createReservationResultModel.Reservation);

            }
            try
            {
                var bonusPointsResult = _userService.AddBonusPointsByUserId(reservationModel.UserId, bonusPointsToAdd);

                if (bonusPointsResult == -1 || bonusPointsToAdd < 0)
                {
                    ErrorResponseModel errorResponse = new ErrorResponseModel
                    {
                        ErrorMessage = Messages.USER_BONUSPOINTS_ERROR,
                        StatusCode = System.Net.HttpStatusCode.BadRequest
                    };

                    return BadRequest(errorResponse);
                }
            }
            catch(DbUpdateException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.USER_BONUSPOINTS_ERROR,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }
            return Created("reservations//", reservationResultList);
        }
    }
}
