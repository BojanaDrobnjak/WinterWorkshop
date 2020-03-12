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
    public class ProjectionService : IProjectionService
    {
        private readonly IProjectionsRepository _projectionsRepository;
        private readonly IReservationService _reservationService;
  
        public ProjectionService(IProjectionsRepository projectionsRepository, IReservationService reservationService)
        {
            _projectionsRepository = projectionsRepository;
            _reservationService = reservationService;
        }

        public async Task<IEnumerable<ProjectionDomainModel>> GetAllAsync()
        {
            var data = await _projectionsRepository.GetAll();

            if (data == null)
            {
                return null;
            }

            List<ProjectionDomainModel> result = new List<ProjectionDomainModel>();
            ProjectionDomainModel model;
            foreach (var item in data)
            {
                model = new ProjectionDomainModel
                {
                    Id = item.Id,
                    MovieId = item.MovieId,
                    AuditoriumId = item.AuditoriumId,
                    ProjectionTime = item.DateTime,
                    MovieTitle = item.Movie.Title,
                    AuditoriumName = item.Auditorium.Name
                };
                result.Add(model);
            }

            return result;
        }

        public async Task<CreateProjectionResultModel> CreateProjection(ProjectionDomainModel domainModel)
        {
            int projectionTime = 3;

            var projectionsAtSameTime = _projectionsRepository.GetByAuditoriumId(domainModel.AuditoriumId)
                .Where(x => x.DateTime < domainModel.ProjectionTime.AddHours(projectionTime) && x.DateTime > domainModel.ProjectionTime.AddHours(-projectionTime))
                .ToList();

            if (projectionsAtSameTime != null && projectionsAtSameTime.Count > 0)
            {
                return new CreateProjectionResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PROJECTIONS_AT_SAME_TIME
                };
            }

            var newProjection = new Projection
            {
                MovieId = domainModel.MovieId,
                AuditoriumId = domainModel.AuditoriumId,
                DateTime = domainModel.ProjectionTime
            };

            var insertedProjection = _projectionsRepository.Insert(newProjection);

            if (insertedProjection == null)
            {
                return new CreateProjectionResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PROJECTION_CREATION_ERROR
                };
            }

            _projectionsRepository.Save();
            CreateProjectionResultModel result = new CreateProjectionResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Projection = new ProjectionDomainModel
                {
                    Id = insertedProjection.Id,
                    AuditoriumId = insertedProjection.AuditoriumId,
                    MovieId = insertedProjection.MovieId,
                    ProjectionTime = insertedProjection.DateTime
                }
            };

            return result;
        }

        public async Task<ProjectionDomainModel> DeleteProjection(Guid id)
        {
            var reservationData = _reservationService.DeleteByProjectionId(id);

            if (reservationData == null)
            {
                return null;
            }

            var data = _projectionsRepository.Delete(id);

            if (data == null)
            {
                return null;
            }

            _projectionsRepository.Save();

            ProjectionDomainModel domainModel = new ProjectionDomainModel
            {
                Id = data.Id,
                MovieId = data.MovieId,
                AuditoriumId = data.AuditoriumId,
                ProjectionTime = data.DateTime

            };

            return domainModel;
        }

        public async Task<ProjectionDomainModel> UpdateProjection(ProjectionDomainModel dataToUpdate)
        {
            Projection projection = new Projection()
            {
                Id = dataToUpdate.Id,
                AuditoriumId = dataToUpdate.AuditoriumId,
                MovieId = dataToUpdate.MovieId,
                DateTime = dataToUpdate.ProjectionTime
            };

            var data = _projectionsRepository.Update(projection);

            if (data == null)
            {
                return null;
            }

            _projectionsRepository.Save();

            ProjectionDomainModel domainModel = new ProjectionDomainModel()
            {
                Id = data.Id,
                AuditoriumId = data.AuditoriumId,
                MovieId = data.MovieId,
                ProjectionTime = data.DateTime
            };

            return domainModel;
        }

        public async Task<IEnumerable<ProjectionDomainModel>> GetProjectionsByReservationIds(IEnumerable<ReservationDomainModel> reservationDomainModels)
        {
            List<ProjectionDomainModel> result = new List<ProjectionDomainModel>();

            foreach (ReservationDomainModel reservation in reservationDomainModels)
            {
                ProjectionDomainModel projection = await GetFullProjectionByIdAsync(reservation.ProjectionId);
                result.Add(projection);
            }
            return result;
        }


        public async Task<ProjectionDomainModel> GetFullProjectionByIdAsync(Guid id)
        {
            var data = await _projectionsRepository.GetByIdAsync(id);

            if (data == null)
            {
                return null;
            }
            //_movieService
           // MovieDomainModel movie = await _movieService.GetMovieByIdAsync(data.MovieId);
            //Movie movie = nadjiPreko servisa
            // Auditorium name = nadji preko servisa  
            ProjectionDomainModel domainModel = new ProjectionDomainModel
            {
                Id = data.Id,
                AuditoriumId = data.AuditoriumId,
                // AuditoriumName = data.Auditorium.Name,
                MovieId = data.MovieId,
                ProjectionTime = data.DateTime,
              //   MovieTitle = movie.Title,
              //  MovieYear = movie.Year,
              //   MovieRating = movie.Rating
            };

            return domainModel;
        }

        public async Task<ProjectionDomainModel> GetProjectionByIdAsync(Guid id)
        {
            var data = await _projectionsRepository.GetByIdAsync(id);

            if (data == null)
            {
                return null;
            }
           
            ProjectionDomainModel domainModel = new ProjectionDomainModel
            {
                Id = data.Id,
                AuditoriumId = data.AuditoriumId,
                MovieId = data.MovieId,
                ProjectionTime = data.DateTime    
            };

            return domainModel;
        }

        public async Task<IEnumerable<ProjectionDomainModel>> GetProjectionsByAuditoriumId(int id)
        {
            var data = _projectionsRepository.GetByAuditoriumId(id);

            if (data == null)
            {
                return null;
            }

            List<ProjectionDomainModel> domainModelList = new List<ProjectionDomainModel>();
            foreach (Projection projection in data)
            {
                ProjectionDomainModel domainModel = new ProjectionDomainModel
                {
                    Id = projection.Id,
                    AuditoriumId = projection.AuditoriumId,
                    MovieId = projection.MovieId,
                    ProjectionTime = projection.DateTime,
                    AuditoriumName = projection.Auditorium.Name,
                    MovieTitle = projection.Movie.Title,
                    MovieRating = projection.Movie.Rating,
                    MovieYear = projection.Movie.Year
                };

                domainModelList.Add(domainModel);
            }

            return domainModelList;
        }


        public async Task<IEnumerable<ProjectionDomainModel>> DeleteByAuditoriumId(int auditoriumId)
        {
            var projectionModelsByAuditoriumId = _projectionsRepository.GetByAuditoriumId(auditoriumId).ToList();

            foreach (Projection projection in projectionModelsByAuditoriumId)
            {
                var deletedReservations = await _reservationService.DeleteByProjectionId(projection.Id);

                if (deletedReservations == null)
                {
                    return null;
                }
            }

            var deletedProjections = await _projectionsRepository.DeleteByAuditoriumId(auditoriumId);

            if (deletedProjections == null)
            {
                return null;
            }

            _projectionsRepository.Save();

            List<ProjectionDomainModel> domainModelList = new List<ProjectionDomainModel>();

            foreach (Projection projection in deletedProjections)
            {
                ProjectionDomainModel domainModel = new ProjectionDomainModel
                {
                    Id = projection.Id,
                    AuditoriumId = projection.AuditoriumId,
                    MovieId = projection.MovieId,
                    ProjectionTime = projection.DateTime
                };
                domainModelList.Add(domainModel);
            }
            return domainModelList;
        }

        public async Task<IEnumerable<ProjectionDomainModel>> DeleteByMovieId(Guid movieId)
        {
            var projectionModelsByMovieId = _projectionsRepository.GetByMovieId(movieId).ToList();

            foreach (Projection projection in projectionModelsByMovieId)
            {
                var deletedReservations = await _reservationService.DeleteByProjectionId(projection.Id);

                if (deletedReservations == null)
                {
                    return null;
                }
            }

            var deletedProjections = await _projectionsRepository.DeleteByMovieId(movieId);

            if (deletedProjections == null)
            {
                return null;
            }

            _projectionsRepository.Save();

            List<ProjectionDomainModel> domainModelList = new List<ProjectionDomainModel>();

            foreach (Projection projection in deletedProjections)
            {
                ProjectionDomainModel domainModel = new ProjectionDomainModel
                {
                    Id = projection.Id,
                    AuditoriumId = projection.AuditoriumId,
                    MovieId = projection.MovieId,
                    ProjectionTime = projection.DateTime
                };
                domainModelList.Add(domainModel);
            }
            return domainModelList;
        }

        public async Task<IEnumerable<ProjectionDomainModel>> DeleteByAuditoriumIdMovieId(int auditoriumId, Guid movieId)
        {
            var projectionModelsByMovieId = _projectionsRepository.GetByAuditoriumIdMovieId(auditoriumId, movieId).ToList();

            foreach (Projection projection in projectionModelsByMovieId)
            {
                var deletedReservations = await _reservationService.DeleteByProjectionId(projection.Id);

                if (deletedReservations == null)
                {
                    return null;
                }
            }

            var deletedProjections = await _projectionsRepository.DeleteByAuditoriumIdMovieId(auditoriumId, movieId);

            if (deletedProjections == null)
            {
                return null;
            }

            _projectionsRepository.Save();

            List<ProjectionDomainModel> domainModelList = new List<ProjectionDomainModel>();

            foreach (Projection projection in deletedProjections)
            {
                ProjectionDomainModel domainModel = new ProjectionDomainModel
                {
                    Id = projection.Id,
                    AuditoriumId = projection.AuditoriumId,
                    MovieId = projection.MovieId,
                    ProjectionTime = projection.DateTime
                };
                domainModelList.Add(domainModel);
            }
            return domainModelList;
        }

        public async Task<IEnumerable<ProjectionDomainModel>> GetProjectionByMovieId(Guid id)
        {
            var data = _projectionsRepository.GetByMovieId(id).ToList();

            if (data == null)
            {
                return null;
            }
            List<ProjectionDomainModel> domainModelList = new List<ProjectionDomainModel>();

            foreach (Projection projection in data)
            {
                ProjectionDomainModel domainModel = new ProjectionDomainModel
                {
                    Id = projection.Id,
                    AuditoriumId = projection.AuditoriumId,
                    MovieId = projection.MovieId,
                    ProjectionTime = projection.DateTime
                };

                domainModelList.Add(domainModel);
            }

            return domainModelList;
        }

        public async Task<IEnumerable<ProjectionDomainModel>> GetProjectionByAuditoriumIdMovieId(int auditoriumId, Guid movieId)
        {
            var data = _projectionsRepository.GetByAuditoriumIdMovieId(auditoriumId, movieId);

            if (data == null)
            {
                return null;
            }
            List<ProjectionDomainModel> domainModelList = new List<ProjectionDomainModel>();

            foreach (Projection projection in data)
            {
                ProjectionDomainModel domainModel = new ProjectionDomainModel
                {
                    Id = projection.Id,
                    AuditoriumId = projection.AuditoriumId,
                    MovieId = projection.MovieId,
                    ProjectionTime = projection.DateTime,
                    AuditoriumName = projection.Auditorium.Name
                };

                domainModelList.Add(domainModel);
            }

            return domainModelList;
        }

    }
}
