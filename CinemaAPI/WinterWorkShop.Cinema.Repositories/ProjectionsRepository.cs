using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface IProjectionsRepository : IRepository<Projection> 
    {
        IEnumerable<Projection> GetByAuditoriumId(int salaId);
        IEnumerable<Projection> GetByMovieId(Guid movieId);
        Task<IEnumerable<Projection>> DeleteByAuditoriumId(int auditoriumId);
        Task<IEnumerable<Projection>> DeleteByMovieId(Guid movieId);
        Task<IEnumerable<Projection>> DeleteByAuditoriumIdMovieId(int auditoriumId, Guid movieId);
        IEnumerable<Projection> GetByAuditoriumIdMovieId(int auditoriumId, Guid movieId);
    }

    public class ProjectionsRepository : IProjectionsRepository
    {
        private CinemaContext _cinemaContext;

        public ProjectionsRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public Projection Delete(object id)
        {
            Projection existing = _cinemaContext.Projections.Find(id);
            var result = _cinemaContext.Projections.Remove(existing).Entity;

            return result;
        }


        public async Task<IEnumerable<Projection>> GetAll()
        {
            var data = await _cinemaContext.Projections.Include(x => x.Movie).Include(x => x.Auditorium).ToListAsync();
            
            return data;           
        }

        public async Task<Projection> GetByIdAsync(object id)
        {
             return await _cinemaContext.Projections.FindAsync(id);
        }

        public IEnumerable<Projection> GetByAuditoriumId(int auditoriumId)
        {
            var projectionsData = _cinemaContext.Projections.Where(x => x.AuditoriumId == auditoriumId).Include(x => x.Movie).Include(x => x.Auditorium);

            return projectionsData;
        }

        public IEnumerable<Projection> GetByAuditoriumIdMovieId(int auditoriumId, Guid movieId)
        {
            var projectionsData = _cinemaContext.Projections.Where(x => x.AuditoriumId == auditoriumId && x.MovieId == movieId).Include(x => x.Movie);

            return projectionsData;
        }

        public async Task<IEnumerable<Projection>> DeleteByAuditoriumId(int auditoriumId)
        {
            IEnumerable<Projection> projectionList = _cinemaContext.Projections.Where(x => x.AuditoriumId == auditoriumId);
            List<Projection> result = new List<Projection>();
            foreach (Projection projection in projectionList)
            {
                var data = _cinemaContext.Remove(projection);
                result.Add(data.Entity);
            }

            return result;
        }

        public async Task<IEnumerable<Projection>> DeleteByAuditoriumIdMovieId(int auditoriumId, Guid movieId)
        {
            IEnumerable<Projection> projectionList = _cinemaContext.Projections.Where(x => x.AuditoriumId == auditoriumId && x.MovieId == movieId);
            List<Projection> result = new List<Projection>();
            foreach (Projection projection in projectionList)
            {
                var data = _cinemaContext.Remove(projection);
                result.Add(data.Entity);
            }

            return result;
        }

        public async Task<IEnumerable<Projection>> DeleteByMovieId(Guid movieId)
        {
            IEnumerable<Projection> projectionList = _cinemaContext.Projections.Where(x => x.MovieId == movieId);
            List<Projection> result = new List<Projection>();
            foreach (Projection projection in projectionList)
            {
                var data = _cinemaContext.Remove(projection);
                result.Add(data.Entity);
            }

            return result;
        }

        public Projection Insert(Projection obj)
        {
            var data = _cinemaContext.Projections.Add(obj).Entity;

            return data;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public Projection Update(Projection obj)
        {
            var updatedEntry = _cinemaContext.Projections.Attach(obj).Entity;
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry;
        }

        public IEnumerable<Projection> GetByMovieId(Guid movieId)
        {
            var projectionsData = _cinemaContext.Projections.Where(x => x.MovieId == movieId).Include(x => x.Auditorium);

            return projectionsData;
        }
    }
}
