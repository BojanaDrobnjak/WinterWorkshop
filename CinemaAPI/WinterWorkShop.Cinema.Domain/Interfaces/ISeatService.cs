using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface ISeatService
    {
        Task<IEnumerable<SeatDomainModel>> GetAllAsync();

        Task<SeatDomainModel> DeleteSeat(Guid id);

        Task<IEnumerable<SeatDomainModel>> GetSeatsByAuditoriumId(int id);

        Task<IEnumerable<SeatDomainModel>> DeleteByAuditoriumId(int auditoriumId);

        Task<SeatDomainModel> AddSeat(SeatDomainModel newSeat);

        Task<SeatDomainModel> GetSeatByAuditoriumIdRowSeatnum(int auditoriumId, int rowNum, int seatNum);
        Task<NumberOfSeatsModel> GetNumberOfSeats(int id);
    }
}
