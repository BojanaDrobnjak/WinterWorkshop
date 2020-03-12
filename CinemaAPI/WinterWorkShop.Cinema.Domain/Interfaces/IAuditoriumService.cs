using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface IAuditoriumService
    {
        Task<IEnumerable<AuditoriumDomainModel>> GetAllAsync();
        Task<AuditoriumDomainModel> DeleteAuditorium(int id);
        Task<IEnumerable<AuditoriumDomainModel>> GetByCinemaId(int cinemaId);
        Task<IEnumerable<AuditoriumDomainModel>> DeleteAuditoriumsByCinemaId(int cinemaId);
        Task<AuditoriumDomainModel> GetByIdAsync(int id);
        Task<AuditoriumDomainModel> UpdateAuditorium(AuditoriumDomainModel updatedAuditorium, int numberOfRows, int numberOfColumns);
        Task<CreateAuditoriumResultModel> CreateAuditorium(AuditoriumDomainModel domainModel, int numberOfRows, int numberOfSeats);
        
    }
}
