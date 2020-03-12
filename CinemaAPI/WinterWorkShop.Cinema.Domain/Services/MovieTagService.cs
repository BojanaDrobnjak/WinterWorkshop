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
    public class MovieTagService : IMovieTagService
    {
        private readonly IMovieTagsRepository _movieTagsRepository;

        public MovieTagService(IMovieTagsRepository tagsRepository)
        {
            _movieTagsRepository = tagsRepository;
        }


        public async Task<IEnumerable<MovieTagsDomainModel>> GetAllAsync()
        {
            var data = await _movieTagsRepository.GetAll();

            if (data == null)
            {
                return null;
            }

            List<MovieTagsDomainModel> result = new List<MovieTagsDomainModel>();
            MovieTagsDomainModel model;
            foreach (var item in data)
            {
                model = new MovieTagsDomainModel
                {
                    MovieId = item.MovieId,
                    TagId = item.Tagid
                };
                result.Add(model);
            }

            return result;
        }

        public async Task<IEnumerable<MovieTagsDomainModel>> GetByTagId(int tagId)
        {
            var data = await _movieTagsRepository.GetByTagId(tagId);

            if (data == null)
            {
                return null;
            }

            List<MovieTagsDomainModel> result = new List<MovieTagsDomainModel>();
            MovieTagsDomainModel domainModel;
            foreach (var item in data)
            {
                domainModel = new MovieTagsDomainModel
                {
                    MovieId = item.MovieId,
                    TagId = item.Tagid,
                    MovieTitle = item.Movie.Title,
                    TagName = item.Tag.Name
                };
                result.Add(domainModel);
            }
            return result;
        }

        public async Task<IEnumerable<MovieTagsDomainModel>> GetByMovieId(Guid movieId)
        {
            var data = await _movieTagsRepository.GetByMovieId(movieId);

            if (data == null)
            {
                return null;
            }

            List<MovieTagsDomainModel> result = new List<MovieTagsDomainModel>();
            MovieTagsDomainModel domainModel;
            foreach (var item in data)
            {
                domainModel = new MovieTagsDomainModel
                {
                    MovieId = item.MovieId,
                    TagId = item.Tagid,
                    TagName = item.Tag.Name,
                    MovieTitle = item.Movie.Title
                };
                result.Add(domainModel);
            }
            return result;
        }

        public async Task<MovieTagsDomainModel> GetByMovieIdTagId(Guid movieId, int tagId)
        {
            var movieTagsObject = await _movieTagsRepository.GetById(movieId,tagId);

            if (movieTagsObject == null)
            {
                return null;
            }

            MovieTagsDomainModel result = new MovieTagsDomainModel
            {
                MovieId = movieTagsObject.MovieId,
                TagId = movieTagsObject.Tagid
            };

            return result;
        }

        public async Task<MovieTagsDomainModel> AddMovieTag(MovieTagsDomainModel newMovieTag)
        {
            MovieTag movieTagToCreate = new MovieTag()
            {
                MovieId = newMovieTag.MovieId,
                Tagid = newMovieTag.TagId
            };

            var data = _movieTagsRepository.Insert(movieTagToCreate);

            if (data == null)
            {
                return null;
            }

            _movieTagsRepository.Save();

            MovieTagsDomainModel domainModel = new MovieTagsDomainModel()
            {
                MovieId = data.MovieId,
                TagId = data.Tagid
            };

            return domainModel;
        }

        public async Task<IEnumerable<MovieTagsDomainModel>> DeleteByTagId(int id)
        {
            var data = await _movieTagsRepository.DeleteByTagId(id);

            if (data == null)
            {
                return null;
            }

            _movieTagsRepository.Save();

            List<MovieTagsDomainModel> result = new List<MovieTagsDomainModel>();

            foreach (MovieTag movieTag in data)
            {
                MovieTagsDomainModel domainModel = new MovieTagsDomainModel
                {
                    MovieId = movieTag.MovieId,
                    TagId = movieTag.Tagid
                };

                result.Add(domainModel);
            }

            return result;
        }

        public async Task<IEnumerable<MovieTagsDomainModel>> DeleteByMovieId(Guid id)
        {
            var data = await _movieTagsRepository.DeleteByMovieId(id);

            if (data == null)
            {
                return null;
            }

            _movieTagsRepository.Save();

            List<MovieTagsDomainModel> result = new List<MovieTagsDomainModel>();

            foreach (MovieTag movieTag in data)
            {
                MovieTagsDomainModel domainModel = new MovieTagsDomainModel
                {
                    MovieId = movieTag.MovieId,
                    TagId = movieTag.Tagid
                };

                result.Add(domainModel);
            }

            return result;
        }

        public async Task<MovieTagsDomainModel> DeleteByMovieIdTagId(Guid movieId, int tagId)
        {
            var deletedMovieTag = await _movieTagsRepository.DeleteById(movieId,tagId);

            if (deletedMovieTag == null)
            {
                return null;
            }

            _movieTagsRepository.Save();

            MovieTagsDomainModel result = new MovieTagsDomainModel
            {
                MovieId = deletedMovieTag.MovieId,
                TagId = deletedMovieTag.Tagid
            };

            return result;
        }
        
    }
}
