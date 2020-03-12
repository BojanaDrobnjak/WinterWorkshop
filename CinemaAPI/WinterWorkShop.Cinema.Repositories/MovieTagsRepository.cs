using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface IMovieTagsRepository
    {
        MovieTag Insert(MovieTag obj);
        void Save();
        MovieTag Update(MovieTag obj);

        Task<IEnumerable<MovieTag>> GetAll();
        Task<IEnumerable<MovieTag>> GetByMovieId(Guid movieId);
        Task<IEnumerable<MovieTag>> GetByTagId(int tagId);
        Task<MovieTag> GetById(Guid movieId, int tagId);

        Task<IEnumerable<MovieTag>> DeleteByMovieId(Guid movieId);
        Task<IEnumerable<MovieTag>> DeleteByTagId(int tagId);
        Task<MovieTag> DeleteById(Guid movieId, int tagId);
    }
    public class MovieTagsRepository : IMovieTagsRepository
    {
        private CinemaContext _cinemaContext;

        public MovieTagsRepository (CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }
        public async Task<IEnumerable<MovieTag>> DeleteByMovieId(Guid movieId)
        {
            IEnumerable<MovieTag> existing = _cinemaContext.MovieTags.Where(x => x.MovieId == movieId);
            List<MovieTag> result = new List<MovieTag>();
            foreach (MovieTag movieTag in existing)
            {
                var data = _cinemaContext.Remove(movieTag);
                result.Add(data.Entity);
            }

            return result;
        }

        public async Task<IEnumerable<MovieTag>> DeleteByTagId(int tagId)
        {
            IEnumerable<MovieTag> existing = _cinemaContext.MovieTags.Where(x => x.Tagid == tagId);
            List<MovieTag> result = new List<MovieTag>();
            foreach (MovieTag movieTag in existing)
            {
                var data = _cinemaContext.Remove(movieTag);
                result.Add(data.Entity);
            }

            return result;
        }

        public async Task<MovieTag> DeleteById(Guid movieId, int tagId)
        {
            var existing = await _cinemaContext.MovieTags.Where(x => x.Tagid == tagId && x.MovieId == movieId).FirstOrDefaultAsync();

            var result = _cinemaContext.MovieTags.Remove(existing);

            return result.Entity;
        }

        public async Task<IEnumerable<MovieTag>> GetAll()
        {
            var data = await _cinemaContext.MovieTags.ToListAsync();

            return data;
        }

        public async Task<MovieTag> GetById(Guid movieId, int tagId)
        {
            var data = _cinemaContext.MovieTags.Where(x => x.MovieId == movieId && x.Tagid == tagId).FirstOrDefault();

            return data;
        }


        public MovieTag Insert(MovieTag obj)
        {
            return _cinemaContext.MovieTags.Add(obj).Entity;

        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public MovieTag Update(MovieTag obj)
        {
            var updatedEntry = _cinemaContext.MovieTags.Attach(obj);
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry.Entity;
        }

        public async Task<IEnumerable<MovieTag>> GetByMovieId(Guid movieId)
        {
            var tagdata = _cinemaContext.MovieTags.Where(x => x.MovieId == movieId).Include(x => x.Tag).Include(x => x.Movie);
            return tagdata;
        }

        public async Task<IEnumerable<MovieTag>> GetByTagId(int tagId)
        {
            var data = _cinemaContext.MovieTags.Where(x => x.Tagid == tagId).Include(x => x.Movie).Include(x => x.Tag);

            return data;

        }
    }
}
