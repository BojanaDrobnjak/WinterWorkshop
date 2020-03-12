using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface IMovieTagService
    {
        Task<IEnumerable<MovieTagsDomainModel>> GetAllAsync();
        Task<IEnumerable<MovieTagsDomainModel>> GetByTagId(int tagId);
        Task<IEnumerable<MovieTagsDomainModel>> GetByMovieId(Guid movieId);
        Task<MovieTagsDomainModel> GetByMovieIdTagId(Guid movieId, int tagId);

        Task<MovieTagsDomainModel> AddMovieTag(MovieTagsDomainModel newMovieTag);

        Task<IEnumerable<MovieTagsDomainModel>> DeleteByTagId(int id);
        Task<IEnumerable<MovieTagsDomainModel>> DeleteByMovieId(Guid id);
        Task<MovieTagsDomainModel> DeleteByMovieIdTagId(Guid movieId, int tagId);
    }
}
