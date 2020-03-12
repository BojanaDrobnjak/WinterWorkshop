using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class SeatService : ISeatService
    {
        private readonly IReservationService _reservationService;
        private readonly ISeatsRepository _seatsRepository;

        public SeatService(ISeatsRepository seatsRepository, IReservationService reservationService)
        {
            _reservationService = reservationService;
            _seatsRepository = seatsRepository;
        }

        public async Task<IEnumerable<SeatDomainModel>> GetAllAsync()
        {
            var data = await _seatsRepository.GetAll();

            if (data == null)
            {
                return null;
            }

            List<SeatDomainModel> result = new List<SeatDomainModel>();
            SeatDomainModel model;
            foreach (var item in data)
            {
                model = new SeatDomainModel
                {
                    Id = item.Id,
                    AuditoriumId = item.AuditoriumId,
                    Number = item.Number,
                    Row = item.Row
                };
                result.Add(model);
            }

            return result;
        }

        public async Task<SeatDomainModel> AddSeat (SeatDomainModel newSeat)
        {
            Seat seatToCreate = new Seat()
            {
                AuditoriumId = newSeat.AuditoriumId,
                Number = newSeat.Number,
                Row = newSeat.Row
            };

            var data = _seatsRepository.Insert(seatToCreate);
            if (data == null)
            {
                return null;
            }

            _seatsRepository.Save();

            SeatDomainModel domainModel = new SeatDomainModel()
            {
                Id = data.Id,
                AuditoriumId = data.AuditoriumId,
                Number = data.Number,
                Row = data.Row
            };

            return domainModel;
        }

        public async Task<SeatDomainModel> DeleteSeat(Guid seatId)
        {
            var reservationData = await _reservationService.DeleteBySeatId(seatId);

            if (reservationData == null)
            {
                return null;
            }

            var data = _seatsRepository.Delete(seatId);

            if (data == null)
            {
                return null;
            }

            _seatsRepository.Save();

            SeatDomainModel domainModel = new SeatDomainModel
            {
                Id = data.Id,
                AuditoriumId = data.AuditoriumId,
                Number = data.Number,
                Row = data.Row

            };

            return domainModel;
        }

        public async Task<IEnumerable<SeatDomainModel>> GetSeatsByAuditoriumId(int id)
        {
            var data = _seatsRepository.GetByAuditoriumId(id);

            if (data == null)
            {
                return null;
            }

            List<SeatDomainModel> domainModelList = new List<SeatDomainModel>();

            foreach (Seat seat in data)
            {
                SeatDomainModel domainModel = new SeatDomainModel
                {
                    Id = seat.Id,
                    AuditoriumId = seat.AuditoriumId,
                    Number = seat.Number,
                    Row = seat.Row

                };

                domainModelList.Add(domainModel);
            }

            return domainModelList;
        }

        public async Task<SeatDomainModel> GetSeatByAuditoriumIdRowSeatnum(int auditoriumId, int rowNum, int seatNum)
        {
            var data = await _seatsRepository.GetByAuditoriumIdRowColumn(auditoriumId, rowNum, seatNum);

            if (data == null)
            {
                return null;
            }

            SeatDomainModel domainModel = new SeatDomainModel()
            {
                AuditoriumId = data.AuditoriumId,
                Id = data.Id,
                Number = data.Number,
                Row = data.Row
            };

            return domainModel;
        }

        public async Task<NumberOfSeatsModel> GetNumberOfSeats(int id)
        {
            var data = _seatsRepository.GetMaxValuesByAuditoriumId(id);

            if (data == null)
            {
                return null;
            }

                NumberOfSeatsModel domainModel = new NumberOfSeatsModel
                {
                    MaxNumber = data[0],
                    MaxRow = data[1]
                };

            return domainModel;
        }

        public async Task<IEnumerable<SeatDomainModel>> DeleteByAuditoriumId(int auditoriumId)
        {
            var seatModelsByAuditoriumId = _seatsRepository.GetByAuditoriumId(auditoriumId);
            if (seatModelsByAuditoriumId == null)
            {
                return null;
            }
            seatModelsByAuditoriumId.ToList();

            foreach (Seat seat in seatModelsByAuditoriumId)
            {
                var deletedReservations = await _reservationService.DeleteBySeatId(seat.Id);

                if (deletedReservations == null)
                {
                    return null;
                }
            }

            var data = await _seatsRepository.DeleteByAuditoriumId(auditoriumId);

            if (data == null)
            {
                return null;
            }

            _seatsRepository.Save();

            List<SeatDomainModel> domainModelList = new List<SeatDomainModel>();

            foreach (Seat seat in data)
            {
                SeatDomainModel domainModel = new SeatDomainModel
                {
                    Id = seat.Id,
                    AuditoriumId = seat.AuditoriumId,
                    Number = seat.Number,
                    Row = seat.Row
                };
                domainModelList.Add(domainModel);
            }
            return domainModelList;
        }


    }
}
