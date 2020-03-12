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
    public class TagService : ITagService
    {
        private readonly ITagsRepository _tagsRepository;
        private readonly IMovieTagService _movieTagService;
        public TagService(ITagsRepository tagsRepository, IMovieTagService movieTagService)
        {
            _tagsRepository = tagsRepository;
            _movieTagService = movieTagService;
        }


        public async Task<IEnumerable<TagsDomainModel>> GetAllAsync ()
        {
            var data = await _tagsRepository.GetAll();

            if (data == null)
            {
                return null;
            }

            List<TagsDomainModel> result = new List<TagsDomainModel>();
            TagsDomainModel model;
            foreach (var item in data)
            {
                model = new TagsDomainModel
                {
                    Id = item.Id,
                    Name = item.Name
                };
                result.Add(model);
            }

            return result;

        }

        public async Task<TagsDomainModel> AddTag (TagsDomainModel newTag)
        {
            Tag tagToCreate = new Tag()
            {
                Name = newTag.Name
            };

            var data = _tagsRepository.Insert(tagToCreate);

            if (data == null)
            {
                return null;
            }

            _tagsRepository.Save();

            TagsDomainModel domainModel = new TagsDomainModel()
            {
                Id = data.Id,
                Name = data.Name
            };

            return domainModel;
        }

        public async Task<TagsDomainModel> GetTagByIdAsync (int id)
        {
            var data = await _tagsRepository.GetByIdAsync(id);

            if (data == null)
            {
                return null;
            }

            TagsDomainModel domainModel = new TagsDomainModel
            {
                Id = data.Id,
                Name = data.Name
            };

            return domainModel;
        }

        public async Task<TagsDomainModel> DeleteTag(int tagId)
        {
            var movieTagData = await _movieTagService.DeleteByTagId(tagId);

            var data = _tagsRepository.Delete(tagId);

            if (data == null)
            {
                return null;
            }

            _tagsRepository.Save();

            TagsDomainModel domainModel = new TagsDomainModel
            {
                Id = data.Id,
                Name = data.Name
            };

            return domainModel;
        }

        public async Task<TagsDomainModel> GetTagByTagName(string tagName)
        {
            var data = await _tagsRepository.GetTagByTagName(tagName);

            if (data == null)
            {
                return null;
            }

            TagsDomainModel domainModel = new TagsDomainModel
            {
                Id = data.Id,
                Name = data.Name
            };

            return domainModel;
        }

    }
}
