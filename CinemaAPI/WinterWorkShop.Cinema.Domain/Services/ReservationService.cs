using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationsRepository _reservationRepository;

        public ReservationService(IReservationsRepository reservationRepository)

        {
            _reservationRepository = reservationRepository;
        }

        public async Task<IEnumerable<ReservationDomainModel>> GetAllReservations()
        {
            var data = await _reservationRepository.GetAll();

            if (data == null)
            {
                return null;
            }

            List<ReservationDomainModel> result = new List<ReservationDomainModel>();
            ReservationDomainModel model;
            foreach (var item in data)
            {
                model = new ReservationDomainModel
                {
                    ProjectionId = item.ProjectionId,
                    SeatId = item.SeatId,
                    UserId = item.UserId
                };
                result.Add(model);
            }

            return result;

        }

        public async Task<IEnumerable<ReservationDomainModel>> GetByProjectionId(Guid projectionId)
        {
            var data = await _reservationRepository.GetByProjectionId(projectionId);

            if (data == null)
            {
                return null;
            }

            List<ReservationDomainModel> domainModelList = new List<ReservationDomainModel>();

            foreach (Reservation reservation in data)
            {
                ReservationDomainModel domainModel = new ReservationDomainModel
                {

                    ProjectionId = reservation.ProjectionId,
                    SeatId = reservation.SeatId,
                    UserId = reservation.UserId

                };

                domainModelList.Add(domainModel);
            }
            return domainModelList;
        }

        public async Task<IEnumerable<ReservationDomainModel>> GetByUserId(Guid userId)
        {
            var data = await _reservationRepository.GetByUserId(userId);

            if (data == null)
            {
                return null;
            }

            List<ReservationDomainModel> domainModelList = new List<ReservationDomainModel>();

            foreach (Reservation reservation in data)
            {
                ReservationDomainModel domainModel = new ReservationDomainModel
                {

                    ProjectionId = reservation.ProjectionId,
                    SeatId = reservation.SeatId,
                    UserId = reservation.UserId

                };

                domainModelList.Add(domainModel);
            }
            return domainModelList;
        }

        public async Task<IEnumerable<ReservationDomainModel>> GetBySeatId(Guid seatId)
        {
            var data = await _reservationRepository.GetBySeatId(seatId);

            if (data == null)
            {
                return null;
            }

            List<ReservationDomainModel> domainModelList = new List<ReservationDomainModel>();

            foreach (Reservation reservation in data)
            {
                ReservationDomainModel domainModel = new ReservationDomainModel
                {

                    ProjectionId = reservation.ProjectionId,
                    SeatId = reservation.SeatId,
                    UserId = reservation.UserId

                };

                domainModelList.Add(domainModel);
            }
            return domainModelList;
        }

        public async Task<IEnumerable<ReservationDomainModel>> GetByProjectionIdUserId(Guid projectionid, Guid userId)
        {
            var data = await _reservationRepository.GetByProjectionIdUserId(projectionid, userId);

            if (data == null)
            {
                return null;
            }

            List<ReservationDomainModel> domainModelList = new List<ReservationDomainModel>();


            foreach (Reservation reservation in data)
            {
                ReservationDomainModel domainModel = new ReservationDomainModel
                {
                    ProjectionId = reservation.ProjectionId,
                    SeatId = reservation.SeatId,
                    UserId = reservation.UserId

                };

                domainModelList.Add(domainModel);
            }

            return domainModelList;

        }

        public async Task<IEnumerable<ReservationDomainModel>> GetBySeatIdUserId(Guid seatId, Guid userId)
        {
            var data = await _reservationRepository.GetBySeatIdUserId(seatId, userId);

            if (data == null)
            {
                return null;
            }

            List<ReservationDomainModel> domainModelList = new List<ReservationDomainModel>();


            foreach (Reservation reservation in data)
            {
                ReservationDomainModel domainModel = new ReservationDomainModel
                {
                    ProjectionId = reservation.ProjectionId,
                    SeatId = reservation.SeatId,
                    UserId = reservation.UserId

                };

                domainModelList.Add(domainModel);
            }

            return domainModelList;

        }

        public async Task<IEnumerable<ReservationDomainModel>> GetBySeatIdProjectionId(Guid seatId, Guid projectionId)
        {
            var data = await _reservationRepository.GetBySeatIdProjectionId(seatId, projectionId);

            if (data == null)
            {
                return null;
            }

            List<ReservationDomainModel> domainModelList = new List<ReservationDomainModel>();


            foreach (Reservation reservation in data)
            {
                ReservationDomainModel domainModel = new ReservationDomainModel
                {
                    ProjectionId = reservation.ProjectionId,
                    SeatId = reservation.SeatId,
                    UserId = reservation.UserId

                };

                domainModelList.Add(domainModel);
            }

            return domainModelList;

        }

        public async Task<ReservationDomainModel> GetById(Guid seatId, Guid projectionId, Guid userId)
        {
            var reservationObject = await _reservationRepository.GetById(seatId, projectionId, userId);

            if (reservationObject == null)
            {
                return null;
            }


            ReservationDomainModel result = new ReservationDomainModel
            {
                ProjectionId = reservationObject.ProjectionId,
                SeatId = reservationObject.SeatId,
                UserId = reservationObject.UserId
            };

            return result;

        }

        public async Task<IEnumerable<ReservationDomainModel>> DeleteByProjectionId(Guid projectionId)
        {
            var data = await _reservationRepository.DeleteByProjectionId(projectionId);

            if (data == null)
            {
                return null;
            }

            List<ReservationDomainModel> domainModelList = new List<ReservationDomainModel>();

            foreach (Reservation reservation in data)
            {
                ReservationDomainModel domainModel = new ReservationDomainModel
                {
                    ProjectionId = reservation.ProjectionId,
                    SeatId = reservation.SeatId,
                    UserId = reservation.UserId

                };

                domainModelList.Add(domainModel);
            }

            return domainModelList;

        }

        public async Task<IEnumerable<ReservationDomainModel>> DeleteByUserId(Guid userId)
        {
            var data = await _reservationRepository.DeleteByUserId(userId);

            if (data == null)
            {
                return null;
            }


            List<ReservationDomainModel> domainModelList = new List<ReservationDomainModel>();

            foreach (Reservation reservation in data)
            {
                ReservationDomainModel domainModel = new ReservationDomainModel
                {
                    ProjectionId = reservation.ProjectionId,
                    SeatId = reservation.SeatId,
                    UserId = reservation.UserId

                };

                domainModelList.Add(domainModel);
            }

            return domainModelList;

        }

        public async Task<IEnumerable<ReservationDomainModel>> DeleteBySeatId(Guid seatId)
        {
            var data = await _reservationRepository.DeleteBySeatId(seatId);

            if (data == null)
            {
                return null;
            }

            List<ReservationDomainModel> domainModelList = new List<ReservationDomainModel>();

            foreach (Reservation reservation in data)
            {
                ReservationDomainModel domainModel = new ReservationDomainModel
                {
                    ProjectionId = reservation.ProjectionId,
                    SeatId = reservation.SeatId,
                    UserId = reservation.UserId

                };

                domainModelList.Add(domainModel);
            }

            return domainModelList;

        }

        public async Task<IEnumerable<ReservationDomainModel>> DeleteByUserIdProjectionId(Guid userId, Guid projectionId)
        {
            var data = await _reservationRepository.DeleteByUserIdProjectionId(userId, projectionId);

            if (data == null)
            {
                return null;
            }

            _reservationRepository.Save();

            List<ReservationDomainModel> domainModelList = new List<ReservationDomainModel>();
            foreach (Reservation reservation in data)
            {
                ReservationDomainModel domainModel = new ReservationDomainModel
                {
                    ProjectionId = reservation.ProjectionId,
                    SeatId = reservation.SeatId,
                    UserId = reservation.UserId

                };

                domainModelList.Add(domainModel);
            }
            return domainModelList;
        }

        public async Task<IEnumerable<ReservationDomainModel>> DeleteBySeatIdProjectionId(Guid seatId, Guid projectionId)
        {
            var data = await _reservationRepository.DeleteBySeatIdProjectionId(seatId, projectionId);

            if (data == null)
            {
                return null;
            }

            List<ReservationDomainModel> domainModelList = new List<ReservationDomainModel>();
            foreach (Reservation reservation in data)
            {
                ReservationDomainModel domainModel = new ReservationDomainModel
                {
                    ProjectionId = reservation.ProjectionId,
                    SeatId = reservation.SeatId,
                    UserId = reservation.UserId

                };

                domainModelList.Add(domainModel);
            }
            return domainModelList;
        }

        public async Task<IEnumerable<ReservationDomainModel>> DeleteBySeatIdUserId(Guid seatId, Guid userId)
        {
            var data = await _reservationRepository.DeleteBySeatIdUserId(seatId, userId);

            if (data == null)
            {
                return null;
            }

            List<ReservationDomainModel> domainModelList = new List<ReservationDomainModel>();
            foreach (Reservation reservation in data)
            {
                ReservationDomainModel domainModel = new ReservationDomainModel
                {
                    ProjectionId = reservation.ProjectionId,
                    SeatId = reservation.SeatId,
                    UserId = reservation.UserId

                };

                domainModelList.Add(domainModel);
            }
            return domainModelList;
        }

        public async Task<ReservationDomainModel> DeleteById(Guid projectionId, Guid seatId, Guid userId)
        {
            var deletedReservation = await _reservationRepository.DeleteById(projectionId, seatId, userId);

            if (deletedReservation == null)
            {
                return null;
            }
            _reservationRepository.Save();

            ReservationDomainModel result = new ReservationDomainModel
            {
                ProjectionId = deletedReservation.ProjectionId,
                SeatId = deletedReservation.SeatId,
                UserId = deletedReservation.UserId
            };

            return result;

        }
        public async Task<CreateReservationResultModel> AddReservation(ReservationDomainModel newReservation)
        {
            Reservation reservationToCreate = new Reservation()
            {
                ProjectionId = newReservation.ProjectionId,
                SeatId = newReservation.SeatId,
                UserId = newReservation.UserId
            };

            var data = _reservationRepository.Insert(reservationToCreate);

            if (data == null)
            {
                return new CreateReservationResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.RESERVATION_CREATION_ERROR
                };
            }


            _reservationRepository.Save();

            CreateReservationResultModel result = new CreateReservationResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Reservation = newReservation,
            };

            return result;
        }
    }
}
