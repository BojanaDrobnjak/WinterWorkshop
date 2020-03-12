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
    public class TagsController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly IMovieTagService _movieTagService;

        public TagsController(ITagService tagService, IMovieTagService movieTagService)
        {
            _tagService = tagService;
            _movieTagService = movieTagService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<TagsDomainModel>> GetByIdAsync (int id)
        {
            TagsDomainModel tag;

            tag = await _tagService.GetTagByIdAsync(id);

            if (tag == null)
            {
                return NotFound(Messages.TAG_DOES_NOT_EXIST);
            }
            return Ok(tag);
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<TagsDomainModel>>> GetAllTags()
        {
            IEnumerable<TagsDomainModel> tagDomainModel;

            tagDomainModel = await _tagService.GetAllAsync();

            if (tagDomainModel == null)
            {
                tagDomainModel = new List<TagsDomainModel>();
            }

            return Ok(tagDomainModel);
        }

        [Authorize(Roles = "admin, superUser")]
        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult>Post (string name)
        {
            if (name == null)
            {
                return BadRequest("Invalid tag name. ");
            }

            TagsDomainModel domainModel = new TagsDomainModel
            {
                Name = name
            };

        TagsDomainModel createdTag;

            try
            {
                createdTag = await _tagService.AddTag(domainModel);
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

            if (createdTag == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.TAG_CREATION_ERROR,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }
            return Created("tags//" + createdTag.Id, createdTag);
        }

        [Authorize(Roles = "admin, superUser")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            TagsDomainModel deletedTag;
            try
            {
                deletedTag = await _tagService.DeleteTag(id);
            }
            catch(ArgumentNullException a)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.TAG_DOES_NOT_EXIST,
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
            

            if (deletedTag == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.TAG_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            return Accepted("tags//" + deletedTag.Id, deletedTag);
        }

        [Authorize]
        [HttpGet]
        [Route("getbymovieid/{movieId}")]
        public async Task<ActionResult<IEnumerable<TagsDomainModel>>> GetTagListByMovieId (Guid movieId)
        {
            var movieTagList = await _movieTagService.GetByMovieId(movieId);


            List<int> tagIdList = new List<int>();
            foreach(MovieTagsDomainModel domainModel in movieTagList)
            {
                tagIdList.Add(domainModel.TagId);
            }


            List<TagsDomainModel> result = new List<TagsDomainModel>();
            TagsDomainModel tagsDomainModel;
            foreach(int id in tagIdList)
            {
                tagsDomainModel = await _tagService.GetTagByIdAsync(id);
                result.Add(tagsDomainModel);
            }

            return Ok(result);
        }

    }
}