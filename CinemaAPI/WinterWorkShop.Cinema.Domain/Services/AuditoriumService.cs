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
    public class AuditoriumService : IAuditoriumService
    {
        private readonly IAuditoriumsRepository _auditoriumsRepository;
        private readonly IProjectionService _projectionService;
        private readonly ISeatService _seatService;

        public AuditoriumService(IAuditoriumsRepository auditoriumsRepository,IProjectionService projectionService, ISeatService seatService)
        {
            _auditoriumsRepository = auditoriumsRepository;
            _projectionService = projectionService;
            _seatService = seatService;
        }

        public async Task<AuditoriumDomainModel> GetByIdAsync (int id)
        {
            var data = await _auditoriumsRepository.GetByIdAsync(id);
            if (data == null)
            {
                return null;
            }

            List<SeatDomainModel> domainModelList = new List<SeatDomainModel>();
            if (data.Seats != null)
            {
                foreach (Seat seat in data.Seats)
                {

                    SeatDomainModel domainModel = new SeatDomainModel()
                    {
                        Id = seat.Id,
                        AuditoriumId = seat.AuditoriumId,
                        Number = seat.Number,
                        Row = seat.Row
                    };
                    domainModelList.Add(domainModel);
                }
            }

            AuditoriumDomainModel result = new AuditoriumDomainModel()
            {
                Id = data.Id,
                CinemaId = data.CinemaId,
                Name = data.Name,
                SeatsList = domainModelList
            };

            return result;
        }
        public async Task<IEnumerable<AuditoriumDomainModel>> GetAllAsync()
        {
            var data = await _auditoriumsRepository.GetAll();

            if (data == null)
            {
                return null;
            }

            List<AuditoriumDomainModel> result = new List<AuditoriumDomainModel>();
            AuditoriumDomainModel model;
            foreach (var item in data)
            {
                model = new AuditoriumDomainModel
                {
                    Id = item.Id,
                    CinemaId = item.CinemaId,
                    Name = item.Name
                };
                result.Add(model);
            }

            return result;
        }

        public async Task<IEnumerable<AuditoriumDomainModel>> GetByCinemaId(int cinemaId)
        {
            var data = await _auditoriumsRepository.GetByCinemaId(cinemaId);

            if (data == null)
            {
                return null;
            }

            List<AuditoriumDomainModel> result = new List<AuditoriumDomainModel>();
            AuditoriumDomainModel model;
            foreach (var item in data)
            {
                model = new AuditoriumDomainModel
                {
                    Id = item.Id,
                    CinemaId = item.CinemaId,
                    Name = item.Name
                };
                result.Add(model);
            }

            return result;
        }

        public async Task<AuditoriumDomainModel> DeleteAuditorium(int auditoriumId)
        {
            var deletedProjections = await _projectionService.DeleteByAuditoriumId(auditoriumId);
            if (deletedProjections == null)
            {
                return null;
            }

            var deletedSeats = await _seatService.DeleteByAuditoriumId(auditoriumId);
            if (deletedSeats == null)
            {
                return null;
            }

            var deletedAuditorium = _auditoriumsRepository.Delete(auditoriumId);
            if (deletedAuditorium == null)
            {
                return null;
            }


            _auditoriumsRepository.Save();

            AuditoriumDomainModel result = new AuditoriumDomainModel
            {
                CinemaId = deletedAuditorium.CinemaId,
                Id = deletedAuditorium.Id,
                Name = deletedAuditorium.Name,
                SeatsList = deletedSeats.ToList()
            };

            return result;

        }

        public async Task<IEnumerable<AuditoriumDomainModel>> DeleteAuditoriumsByCinemaId(int cinemaId)
        {
            var auditoriumsToBeDeleted = (await _auditoriumsRepository.GetByCinemaId(cinemaId));

            if (auditoriumsToBeDeleted == null)
            {
                return null;
            }
            auditoriumsToBeDeleted.ToList();

            List<AuditoriumDomainModel> deletedAuditoriums = new List<AuditoriumDomainModel>();

            foreach(Auditorium auditorium in auditoriumsToBeDeleted)
            {
                var deletedProjection = await _projectionService.DeleteByAuditoriumId(auditorium.Id);
                if (deletedProjection == null)
                {
                    return null;
                }

                var deletedSeats = await _seatService.DeleteByAuditoriumId(auditorium.Id);
                if (deletedSeats == null)
                {
                    return null;
                }

                var deletedAuditorium = _auditoriumsRepository.Delete(auditorium.Id);
                if (deletedAuditorium == null)
                {
                    return null;
                }

                AuditoriumDomainModel domainModel = new AuditoriumDomainModel
                {
                    CinemaId = deletedAuditorium.CinemaId,
                    Id = deletedAuditorium.Id,
                    Name = deletedAuditorium.Name,
                    SeatsList = deletedSeats.ToList()
                };

                deletedAuditoriums.Add(domainModel);
            }

            _auditoriumsRepository.Save();

            return deletedAuditoriums;

        }

        public async Task<CreateAuditoriumResultModel> CreateAuditorium(AuditoriumDomainModel domainModel, int numberOfRows, int numberOfSeats)
        {

            var auditorium = await _auditoriumsRepository.GetByAuditName(domainModel.Name, domainModel.CinemaId);
            var sameAuditoriumName = auditorium.ToList();
            if (sameAuditoriumName != null && sameAuditoriumName.Count > 0)
            {
                return new CreateAuditoriumResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.AUDITORIUM_SAME_NAME
                };
            }

            Auditorium newAuditorium = new Auditorium
            {
                Name = domainModel.Name,
                CinemaId = domainModel.CinemaId,
            };

            newAuditorium.Seats = new List<Seat>();

            for (int i = 1; i <= numberOfRows; i++)
            {
                for (int j = 1; j <= numberOfSeats; j++)
                {
                    Seat newSeat = new Seat()
                    {
                        Row = i,
                        Number = j
                    };

                    newAuditorium.Seats.Add(newSeat);
                }
            }

            Auditorium insertedAuditorium = _auditoriumsRepository.Insert(newAuditorium);
            if (insertedAuditorium == null)
            {
                return new CreateAuditoriumResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.AUDITORIUM_CREATION_ERROR
                };
            }

            _auditoriumsRepository.Save();

            CreateAuditoriumResultModel resultModel = new CreateAuditoriumResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Auditorium = new AuditoriumDomainModel
                {
                    Id = insertedAuditorium.Id,
                    Name = insertedAuditorium.Name,
                    CinemaId = insertedAuditorium.CinemaId,
                    SeatsList = new List<SeatDomainModel>()
                }
            };

            foreach (var item in insertedAuditorium.Seats)
            {
                resultModel.Auditorium.SeatsList.Add(new SeatDomainModel
                {
                    AuditoriumId = insertedAuditorium.Id,
                    Id = item.Id,
                    Number = item.Number,
                    Row = item.Row
                });
            }

            return resultModel;
        }

        public async Task<AuditoriumDomainModel> UpdateAuditorium (AuditoriumDomainModel updatedAuditorium, int numberOfRows, int numberOfColumns)

        {
            var originalAuditoriumSeats = await _seatService.GetSeatsByAuditoriumId(updatedAuditorium.Id);
            
            if(originalAuditoriumSeats == null)
            {
                return null;
            }

            // Proveravanje trenutnog stanja auditorijuma
            int maxRow = 1;
            int maxColumn = 1;

            void setMax(IEnumerable<SeatDomainModel> audit)
            {
                foreach (SeatDomainModel seat in audit)
                {
                    if (seat.Row > maxRow)
                        maxRow = seat.Row;
                    if (seat.Number > maxColumn)
                        maxColumn = seat.Number;
                }
            }

            void setMaxRow(IEnumerable<SeatDomainModel> audit)
            {
                foreach (SeatDomainModel seat in audit)
                {
                    maxRow = 1;
                    if (seat.Row > maxRow)
                        maxRow = seat.Row;
                }
            }

            setMax(originalAuditoriumSeats);

            // Uspostavljanje parametara za izmenu stanja sedista 
            updatedAuditorium.SeatsList = originalAuditoriumSeats.ToList();

            int rowDelta = numberOfRows - maxRow; // parametar koji definise kako se menja broj kolona moze biti pozitivan i negativan
            int columnDelta = numberOfColumns - maxColumn; // parametar koji definise kako se menja broj redova moze biti pozitivan i negativan
            

            if (rowDelta != 0 || columnDelta !=0)
            {
                int i = 0, j = 0; // brojaci
                // ako treba da se dodaju redovi
                if (rowDelta > 0)
                {
                    for ( i = maxRow+1; i<=maxRow+rowDelta; i++ )
                    {
                        for (j = 1; j<=maxColumn; j++)
                        {

                           
                            SeatDomainModel newSeat = new SeatDomainModel()
                            {
                                AuditoriumId = updatedAuditorium.Id,
                                Row = i,
                                Number = j
                            };

                            // sedista se dodaju u objekat koji ce se updejtovati
                            updatedAuditorium.SeatsList.Add(newSeat);
                        }
                    }
                        setMaxRow(updatedAuditorium.SeatsList);
                }
                // ako treba da se izbrisu redovi
                else if (rowDelta < 0)
                {
                    for ( i = maxRow; i>maxRow+rowDelta; i--)
                    {
                        for (j = 1; j<=maxColumn; j++)
                        {
                            // sedista se brisu iz baze
                            var seatToDelete = await _seatService.GetSeatByAuditoriumIdRowSeatnum(updatedAuditorium.Id, i, j);
                            if (seatToDelete == null)
                            {
                                continue;
                            }
                            // ovo je neophodno jer navodno ne moze da se radi .remove preko kopije objekta nego mora da bude 'isti tip objekta'
                            var seatListObjectToRemove = updatedAuditorium.SeatsList.Where(x => x.Id == seatToDelete.Id).FirstOrDefault();
                            updatedAuditorium.SeatsList.Remove(seatListObjectToRemove);

                            var result = await _seatService.DeleteSeat(seatToDelete.Id);
                            if (result == null)
                            {
                                return null;
                            }

                        }
                    }

                    setMaxRow(updatedAuditorium.SeatsList);
                }


                if (columnDelta > 0)
                {
                    for ( i = 1; i <= maxRow; i++)
                    {
                        for (j = maxColumn+1; j <= maxColumn+columnDelta; j++)
                        {
                            SeatDomainModel newSeat = new SeatDomainModel()
                            {
                                AuditoriumId = updatedAuditorium.Id,
                                Row = i,
                                Number = j
                            };

                            // sedista se dodaju u objekat koji ce se updejtovati
                            updatedAuditorium.SeatsList.Add(newSeat);
                        }
                    }
                }
                else if (columnDelta < 0)
                {
                    for (i = 1; i <= maxRow; i++)
                    {
                        for (j = maxColumn; j > maxColumn+columnDelta; j--)
                        {
                            // ovde skidanje sa liste ide prvo jer sedista koja su samo dodata na listu a nisu u bazu takodje treba da se obrisu
                            var seatListObjectToRemove = updatedAuditorium.SeatsList.Where(x => x.AuditoriumId == updatedAuditorium.Id && x.Row == i && x.Number == j).FirstOrDefault();
                            updatedAuditorium.SeatsList.Remove(seatListObjectToRemove);

                            // sedista se brisu iz baze
                            var seatToDelete = await _seatService.GetSeatByAuditoriumIdRowSeatnum(updatedAuditorium.Id, i, j);
                            if (seatToDelete == null)
                            {
                                continue;
                            }

                            var result = _seatService.DeleteSeat(seatToDelete.Id);
                            if (result == null)
                            {
                                return null;
                            }
                        }
                    }

                    foreach (SeatDomainModel seat in updatedAuditorium.SeatsList)
                    {
                        if (seat.Row > maxRow)
                            maxRow = seat.Row;
                    };
                }
            }


            // SeatDomainModel se prevode u SeatList da bi se Auditorium objekat uneo u bazu
            List<Seat> auditoriumSeatList = new List<Seat>();
            foreach(SeatDomainModel domainModel in updatedAuditorium.SeatsList)
            {
                Seat seat = new Seat()
                {
                    Id = domainModel.Id,
                    AuditoriumId = domainModel.AuditoriumId,
                    Row = domainModel.Row,
                    Number = domainModel.Number
                };
                auditoriumSeatList.Add(seat);
            }


            // Kreira se updejtovani objekat za unos u bazu
            Auditorium auditorium = new Auditorium()
            {
                CinemaId = updatedAuditorium.CinemaId,
                Id = updatedAuditorium.Id,
                Name = updatedAuditorium.Name,
                Seats = auditoriumSeatList
            };

            // Updejtovani auditorijum se vraca u bazu
            var data = _auditoriumsRepository.Update(auditorium);
            if (data == null)
            {
                return null;
            }
            _auditoriumsRepository.Save();


            // Prevodjenje vracenog Auditorijuma u AuditoriumDomainModel i return
            List<SeatDomainModel> resultSeatList = new List<SeatDomainModel>();
            foreach (Seat seat in data.Seats)
            {
                SeatDomainModel seatDomainModel = new SeatDomainModel()
                {
                    Id = seat.Id,
                    AuditoriumId = seat.AuditoriumId,
                    Row = seat.Row,
                    Number = seat.Number
                };
                resultSeatList.Add(seatDomainModel);
            }

            AuditoriumDomainModel returnResult = new AuditoriumDomainModel()
            {
                Id = data.Id,
                CinemaId = data.CinemaId,
                Name = data.Name,
                SeatsList = resultSeatList
            };

            return returnResult;

        }
    }
}
