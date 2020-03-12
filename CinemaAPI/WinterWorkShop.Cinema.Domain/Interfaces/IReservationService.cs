using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface IReservationService
    {
        Task<IEnumerable<ReservationDomainModel>> GetAllReservations();
        Task<IEnumerable<ReservationDomainModel>> GetByProjectionId(Guid projectionId);
        Task<IEnumerable<ReservationDomainModel>> GetByUserId(Guid userId);
        Task<IEnumerable<ReservationDomainModel>> GetBySeatId(Guid seatId);
        Task<IEnumerable<ReservationDomainModel>> GetByProjectionIdUserId(Guid projectionid, Guid userId);
        Task<IEnumerable<ReservationDomainModel>> GetBySeatIdUserId(Guid seatId, Guid userId);
        Task<IEnumerable<ReservationDomainModel>> GetBySeatIdProjectionId(Guid seatId, Guid projectionId);
        Task<ReservationDomainModel> GetById(Guid seatId, Guid projectionId, Guid userId);

        Task<IEnumerable<ReservationDomainModel>> DeleteByProjectionId(Guid projectionId);
        Task<IEnumerable<ReservationDomainModel>> DeleteByUserId(Guid userId);
        Task<IEnumerable<ReservationDomainModel>> DeleteBySeatId(Guid seatId);
        Task<IEnumerable<ReservationDomainModel>> DeleteByUserIdProjectionId(Guid userId, Guid projectionId);
        Task<IEnumerable<ReservationDomainModel>> DeleteBySeatIdProjectionId(Guid seatId, Guid projectionId);
        Task<IEnumerable<ReservationDomainModel>> DeleteBySeatIdUserId(Guid seatId, Guid userId);
        Task<ReservationDomainModel> DeleteById(Guid projectionId, Guid seatId, Guid userId);

        Task<CreateReservationResultModel> AddReservation(ReservationDomainModel newReservation);
    }
}
