using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface IProjectionService
    {
        Task<IEnumerable<ProjectionDomainModel>> GetAllAsync();
        Task<ProjectionDomainModel> GetProjectionByIdAsync(Guid id);
        Task<IEnumerable<ProjectionDomainModel>> GetProjectionsByAuditoriumId(int id);
        Task<IEnumerable<ProjectionDomainModel>> GetProjectionByMovieId(Guid id);
        Task<IEnumerable<ProjectionDomainModel>> GetProjectionsByReservationIds(IEnumerable<ReservationDomainModel> reservationDomainModels);
        Task<ProjectionDomainModel> GetFullProjectionByIdAsync(Guid id);
        Task<ProjectionDomainModel> DeleteProjection(Guid id);
        Task<IEnumerable<ProjectionDomainModel>> DeleteByMovieId(Guid movieId);
        Task<IEnumerable<ProjectionDomainModel>> DeleteByAuditoriumId(int auditoriumId);
        Task<IEnumerable<ProjectionDomainModel>> DeleteByAuditoriumIdMovieId(int auditoriumId, Guid movieId);

        Task<ProjectionDomainModel> UpdateProjection(ProjectionDomainModel dataToUpdate);

        Task<CreateProjectionResultModel> CreateProjection(ProjectionDomainModel domainModel);
    }
}
