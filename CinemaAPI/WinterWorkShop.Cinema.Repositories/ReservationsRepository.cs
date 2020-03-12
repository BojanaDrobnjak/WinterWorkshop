using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;

namespace WinterWorkShop.Cinema.Repositories
{

    public interface IReservationsRepository
    {
        Reservation Insert(Reservation obj);

        public void Save();

        Reservation Update(Reservation obj);

        Task<IEnumerable<Reservation>> GetAll();
        Task<IEnumerable<Reservation>> GetByUserId(Guid id);
        Task<IEnumerable<Reservation>> GetByProjectionId(Guid id);
        Task<IEnumerable<Reservation>> GetBySeatId(Guid id);
        Task<IEnumerable<Reservation>> GetByProjectionIdUserId(Guid projectionId, Guid userId);
        Task<IEnumerable<Reservation>> GetBySeatIdUserId(Guid seatId, Guid userId);
        Task<IEnumerable<Reservation>> GetBySeatIdProjectionId(Guid seatId, Guid projectionId);
        Task<Reservation> GetById(Guid seatId, Guid projectionId, Guid userId);



        Task<Reservation> DeleteById(Guid projectionId, Guid seatId, Guid userId);
        Task<IEnumerable<Reservation>> DeleteByUserIdProjectionId(Guid userId, Guid projectionId);
        Task<IEnumerable<Reservation>> DeleteBySeatIdProjectionId(Guid seatId, Guid projectionId);
        Task<IEnumerable<Reservation>> DeleteBySeatIdUserId(Guid seatId, Guid userId);
        Task<IEnumerable<Reservation>> DeleteByProjectionId(Guid projectionId);
        Task<IEnumerable<Reservation>> DeleteByUserId(Guid userId);
        Task<IEnumerable<Reservation>> DeleteBySeatId(Guid seatId);
    }
    public class ReservationsRepository : IReservationsRepository
    {
        private readonly CinemaContext _cinemaContext;
        public ReservationsRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }


        public async Task<Reservation> DeleteById(Guid projectionId, Guid seatId, Guid userId)
        {
            var existing = await _cinemaContext.Reservations.Where(x => x.UserId == userId).Where(y => y.ProjectionId == projectionId).Where(z => z.SeatId == seatId).FirstOrDefaultAsync();
            
            var result = _cinemaContext.Reservations.Remove(existing);

            return result.Entity;
        }

        public async Task<IEnumerable<Reservation>> DeleteByUserIdProjectionId(Guid userId, Guid projectionId)
        {
            var existing = _cinemaContext.Reservations.Where(x => x.UserId == userId).Where(y => y.ProjectionId == projectionId);
            List<Reservation> result = new List<Reservation> { };

            foreach (Reservation reservation in existing)
            {
                var value = _cinemaContext.Reservations.Remove(reservation).Entity;
                result.Add(value);
            }

            return result;
        }

        public async Task<IEnumerable<Reservation>> DeleteBySeatIdProjectionId(Guid seatId, Guid projectionId)
        {
            var existing = _cinemaContext.Reservations.Where(x => x.SeatId == seatId).Where(y => y.ProjectionId == projectionId);
            List<Reservation> result = new List<Reservation> { };

            foreach (Reservation reservation in existing)
            {
                var value = _cinemaContext.Reservations.Remove(reservation).Entity;
                result.Add(value);
            }

            return result;
        }

        public async Task<IEnumerable<Reservation>> DeleteBySeatIdUserId(Guid seatId, Guid userId)
        {
            var existing = _cinemaContext.Reservations.Where(x => x.SeatId == seatId).Where(y => y.UserId == userId);
            List<Reservation> result = new List<Reservation> { };

            foreach (Reservation reservation in existing)
            {
                var value = _cinemaContext.Reservations.Remove(reservation).Entity;
                result.Add(value);
            }

            return result;
        }

        public async Task<IEnumerable<Reservation>> DeleteByUserId(Guid userId)
        {
            var existing = _cinemaContext.Reservations.Where(x => x.UserId == userId);
            List<Reservation> result = new List<Reservation> { };

            foreach (Reservation reservation in existing)
            {
                var value = _cinemaContext.Reservations.Remove(reservation).Entity;
                result.Add(value);
            }

            return result;
        }

        public async Task<IEnumerable<Reservation>> DeleteByProjectionId (Guid projectionId)
        {
            var existing = _cinemaContext.Reservations.Where(x => x.ProjectionId == projectionId);
            List<Reservation> result = new List<Reservation> { };

            foreach (Reservation reservation in existing)
            {
                var value = _cinemaContext.Reservations.Remove(reservation).Entity;
                result.Add(value);
            }

            return result;
        }

        public async Task<IEnumerable<Reservation>> DeleteBySeatId(Guid seatId)
        {
            var existing = _cinemaContext.Reservations.Where(x => x.SeatId == seatId);
            List<Reservation> result = new List<Reservation> { };

            foreach (Reservation reservation in existing)
            {
                var value = _cinemaContext.Reservations.Remove(reservation).Entity;
                result.Add(value);
            }

            return result;
        }


        public async Task<IEnumerable<Reservation>> GetAll()
        {
            var data = await _cinemaContext.Reservations.ToListAsync();

            return data;
        }

        public async Task<IEnumerable<Reservation>> GetByProjectionId(Guid id)
        {
            var data = _cinemaContext.Reservations.Where(x => x.ProjectionId == id);

            return data;
        }

        public async Task<IEnumerable<Reservation>> GetByUserId(Guid id)
        {
            var data = _cinemaContext.Reservations.Where(x => x.UserId == id);

            return data;
        }

        public async Task<IEnumerable<Reservation>> GetBySeatId(Guid id)
        {
            var data = _cinemaContext.Reservations.Where(x => x.SeatId == id);

            return data;
        }

        public async Task<IEnumerable<Reservation>> GetByProjectionIdUserId(Guid projectionId, Guid userId)
        {
            var data = _cinemaContext.Reservations.Where(x => x.ProjectionId == projectionId).Where(y => y.UserId == userId);

            return data;
        }

        public async Task<IEnumerable<Reservation>> GetBySeatIdUserId(Guid seatId, Guid userId)
        {
            var data = _cinemaContext.Reservations.Where(x => x.SeatId == seatId).Where(y => y.UserId == userId);

            return data;
        }

        public async Task<IEnumerable<Reservation>> GetBySeatIdProjectionId(Guid seatId, Guid projectionId)
        {
            var data = _cinemaContext.Reservations.Where(x => x.SeatId == seatId).Where(y => y.ProjectionId == projectionId);

            return data;
        }

        public async Task<Reservation> GetById(Guid seatId, Guid projectionId, Guid userId)
        {
            var data = await _cinemaContext.Reservations.Where(x => x.SeatId == seatId).Where(y => y.ProjectionId == projectionId).Where(z => z.UserId == userId).FirstOrDefaultAsync();

            return data;
        }

        public Reservation Insert(Reservation obj)
        {
            var data = _cinemaContext.Reservations.Add(obj).Entity;

            return data;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public Reservation Update(Reservation obj)
        {
            var updatedEntry = _cinemaContext.Reservations.Attach(obj).Entity;
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry;
        }
    }
}
