using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<UserDomainModel>>> GetAsync()
        {
            IEnumerable<UserDomainModel> userDomainModels;

            userDomainModels = await _userService.GetAllAsync();

            if (userDomainModels == null)
            {
                userDomainModels = new List<UserDomainModel>();
            }

            return Ok(userDomainModels);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            UserDomainModel deletedUser;
            try
            {
                deletedUser = await _userService.DeleteUserById(id);
            }
            catch (ArgumentNullException a)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.USER_NOT_FOUND,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);
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


            if (deletedUser == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.USER_NOT_FOUND,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            return Accepted("tags//" + deletedUser.Id, deletedUser);
        }

        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult> Post([FromBody] CreateUserModel newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var sameName = await _userService.GetUserByUserName(newUser.UserName);
            if (sameName != null)
            {
                {
                    ErrorResponseModel errorResponse = new ErrorResponseModel
                    {
                        ErrorMessage = Messages.USER_SAME_USERNAME,
                        StatusCode = System.Net.HttpStatusCode.BadRequest
                    };

                    return BadRequest(errorResponse);
                }
            }

            UserDomainModel domainModel = new UserDomainModel
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                UserName = newUser.UserName,
                IsAdmin = false,
                IsSuperUser = false,
                IsUser = true,
                BonusPoints = 0
            };

            UserDomainModel createdUser;

            try
            {
                createdUser = await _userService.AddUser(domainModel);
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

            if (createdUser == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.USER_CREATION_ERROR,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }
            return Created("tags//" + createdUser.Id, createdUser);
        }

        /// <summary>
        /// Gets User by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<UserDomainModel>> GetbyIdAsync(Guid id)
        {
            UserDomainModel model;

            model = await _userService.GetUserByIdAsync(id);

            if (model == null)
            {
                return NotFound(Messages.USER_NOT_FOUND);
            }

            return Ok(model);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody]CreateUserModel movieModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserDomainModel userToUpdate;

            userToUpdate = await _userService.GetUserByIdAsync(id);

            if (userToUpdate == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.USER_NOT_FOUND,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            userToUpdate.UserName = movieModel.UserName;
            userToUpdate.FirstName = movieModel.FirstName;
            userToUpdate.LastName = movieModel.LastName;

            UserDomainModel userDomainModel;
            try
            {
                userDomainModel = await _userService.UpdateUser(userToUpdate);
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

            return Accepted("movies//" + userDomainModel.Id, userDomainModel);

        }

        // <summary>
        /// Gets User by UserName
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("byusername/{username}")]
        public async Task<ActionResult<UserDomainModel>> GetbyUserNameAsync(string username)
        {
            UserDomainModel model;

            model = await _userService.GetUserByUserName(username);

            if (model == null)
            {
                return NotFound(Messages.USER_NOT_FOUND);
            }

            return Ok(model);
        }
    }
}
