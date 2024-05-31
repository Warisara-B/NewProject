using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Plexus.Database;
using Plexus.Database.Enum;
using Plexus.Database.Enum.Academic.Section;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Academic.Section;
using Plexus.Entity.DTO;
using Plexus.Entity.DTO.Academic.Curriculum;
using Plexus.Entity.DTO.Academic.Section;
using Plexus.Entity.Exception;
using SectionModel = Plexus.Database.Model.Academic.Section.Section;

namespace Plexus.Entity.Provider.src.Academic.Section
{
    public class SectionSeatProvider : ISectionSeatProvider
    {
        private readonly DatabaseContext _dbContext;
        
        public SectionSeatProvider(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public SectionSeatDTO GetById(Guid id)
        {
            var sectionSeat = _dbContext.SectionSeats.AsNoTracking()
                                                     .SingleOrDefault(x => x.Id == id);

            if (sectionSeat is null)
            {
                throw new SectionException.SeatNotFound(id);
            }

            var response = MapModelToDTO(sectionSeat);

            return response;
        }

        public IEnumerable<SectionSeatDTO> GetById(IEnumerable<Guid> ids)
        {
            if (ids is null || !ids.Any())
            {
                return Enumerable.Empty<SectionSeatDTO>();
            }

            var sectionSeats = _dbContext.SectionSeats.AsNoTracking()
                                                      .Where(x => ids.Contains(x.Id))
                                                      .ToList();

            var response = (from seat in sectionSeats
                            orderby seat.Name
                            select MapModelToDTO(seat))
                           .ToList();

            return response;
        }

        public IEnumerable<SectionSeatDTO> GetBySectionId(IEnumerable<Guid> sectionIds)
        {
            var sectionSeats = _dbContext.SectionSeats.AsNoTracking()
                                                      .Where(x => sectionIds.Contains(x.SectionId))
                                                      .ToList();

            var response = (from seat in sectionSeats
                            orderby seat.Name
                            select MapModelToDTO(seat))
                           .ToList();

            return response;
        }
        
        public (IEnumerable<SectionSeat>, List<SectionSeatUsage>) MapCreateDTOToModel(SectionModel section, IEnumerable<CreateSectionSeatDTO> requests, IEnumerable<SectionSeat> defaultSeats, string requester)
        {
            var createdResponses = new List<SectionSeat>();

            var usages = new List<SectionSeatUsage>();

            foreach (var request in requests)
            {
                var model = new SectionSeat
                {
                    Section = section,
                    Name = request.Name,
                    Type = request.Type,
                    Conditions = request.Conditions is null ? null
                                                            : JsonConvert.SerializeObject(request.Conditions),
                    TotalSeat = request.TotalSeat,
                    Remark = request.Remark,
                    SeatUsed = 0,
                    MasterSeat = section.ParentSectionId.HasValue ? defaultSeats.SingleOrDefault(x => x.SectionId == section.ParentSectionId.Value)
                                                                  : null,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = requester,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = requester
                };

                createdResponses.Add(model);

                foreach (var defaultSeat in defaultSeats)
                {
                    defaultSeat.SeatUsed += request.TotalSeat;
                    
                    var deductSeat = new SectionSeatUsage
                    {
                        Seat = defaultSeat,
                        Section = defaultSeat.Section,
                        Amount = request.TotalSeat * -1,
                        ReferenceSeat = model,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = requester
                    };

                    usages.Add(deductSeat);
                }

                var reserveSeat = new SectionSeatUsage
                {
                    Seat = model,
                    Section = section,
                    Amount = request.TotalSeat,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = requester
                };

                usages.Add(reserveSeat);
            }
            
            return (createdResponses, usages);
        }

        public (List<CreateSectionSeatDTO>, List<SectionSeatDTO>) Upsert(IEnumerable<UpsertSectionSeatDTO>? requests, int remaingingSeat)
        {
            var updatedResponses = new List<SectionSeatDTO>();

            var createdResponses = new List<CreateSectionSeatDTO>();

            if (requests is null || !requests.Any())
            {
                return (createdResponses, updatedResponses);
            }

            // UPDATE SECTION SEATS
            var updatedRequests = requests.Where(x => x.Id.HasValue)
                                          .ToList();

            foreach (var request in updatedRequests)
            {
                var sectionSeat = _dbContext.SectionSeats.AsNoTracking()
                                                         .SingleOrDefault(x => x.Id == request.Id!.Value);

                if (sectionSeat is null)
                {
                    throw new SectionException.SeatNotFound(request.Id!.Value);
                }

                var dto = MapModelToDTO(sectionSeat);

                if (request.TotalSeat < sectionSeat.SeatUsed)
                {
                    throw new SectionException.NotAllowUpdateSeatLessThanAlreadyUsed(sectionSeat.SeatUsed);
                }

                dto.Name = request.Name;
                dto.TotalSeat = request.TotalSeat;
                dto.Conditions = request.Conditions is null ? null
                                                            : (from condition in request.Conditions
                                                               select new SectionConditionDTO
                                                               {
                                                                   FacultyId = condition.FacultyId,
                                                                   DepartmentId = condition.DepartmentId,
                                                                   CurriculumId = condition.CurriculumId,
                                                                   CurriculumVersionId = condition.CurriculumVersionId,
                                                                   Batches = condition.Batches is null ? null : (from batch in condition.Batches
                                                                                                                 select batch)
                                                                                                                .ToList(),
                                                                   Codes = condition.Codes is null ? null
                                                                                                   : (from code in condition.Codes
                                                                                                      select code)
                                                                                                     .ToList()
                                                               })
                                                              .ToList();
                dto.Remark = request.Remark;

                if (request.Type != SeatType.RESERVED)
                {
                    continue;
                }

                var seatDiff = request.TotalSeat - sectionSeat.TotalSeat;
                dto.UsageAmount = GenerateUsage(seatDiff, remaingingSeat);

                updatedResponses.Add(dto);
            }

            var createdRequests = requests.Where(x => !x.Id.HasValue)
                                          .ToList();

            foreach (var request in createdRequests)
            {
                var dto = new CreateSectionSeatDTO
                {
                    Name = request.Name,
                    Type = request.Type,
                    Conditions = request.Conditions is null ? null
                                                            : (from condition in request.Conditions
                                                               select new SectionConditionDTO
                                                               {
                                                                   FacultyId = condition.FacultyId,
                                                                   DepartmentId = condition.DepartmentId,
                                                                   CurriculumId = condition.CurriculumId,
                                                                   CurriculumVersionId = condition.CurriculumVersionId,
                                                                   Batches = condition.Batches is null ? null : (from batch in condition.Batches
                                                                                                                 select batch)
                                                                                                                .ToList(),
                                                                   Codes = condition.Codes is null ? null
                                                                                                   : (from code in condition.Codes
                                                                                                      select code)
                                                                                                     .ToList()
                                                               })
                                                              .ToList(),
                    TotalSeat = request.TotalSeat,
                    Remark = request.Remark
                };

                if (request.Type != SeatType.RESERVED)
                {
                    continue;
                }

                var usage = GenerateUsage(request.TotalSeat, remaingingSeat);

                createdResponses.Add(dto);
            }

            return (createdResponses, updatedResponses);
        }

        public (IEnumerable<SectionSeat>, List<SectionSeatUsage>) MapUpdateDTOToModel(Guid sectionId, IEnumerable<SectionSeatDTO> requests, IEnumerable<SectionSeat> defaultSeats, string requester)
        {
            var usages = new List<SectionSeatUsage>();

            var sectionSeats = _dbContext.SectionSeats.Where(x => x.SectionId == sectionId
                                                                  && x.Type != SeatType.DEFAULT)
                                                      .ToList();

            foreach (var request in requests)
            {
                var sectionSeat = sectionSeats.SingleOrDefault(x => x.Id == request.Id);
                
                if (sectionSeat is null)
                {
                    throw new SectionException.SeatNotFound(request.Id);
                }

                sectionSeat.Name = request.Name;
                sectionSeat.TotalSeat = request.TotalSeat;
                sectionSeat.Conditions = request.Conditions is null ? null
                                                                    : JsonConvert.SerializeObject(request.Conditions);
                sectionSeat.Remark = request.Remark;
                sectionSeat.UpdatedAt = DateTime.UtcNow;

                if (!request.UsageAmount.HasValue)
                {
                    continue;
                }

                foreach (var defaultSeat in defaultSeats)
                {
                    defaultSeat.SeatUsed += request.UsageAmount.Value;

                    var adjustUsage = new SectionSeatUsage
                    {
                        SeatId = defaultSeat.Id,
                        Amount = request.UsageAmount.Value * -1,
                        SectionId = defaultSeat.SectionId,
                        ReferenceSeatId = sectionSeat.Id,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = requester
                    };

                    usages.Add(adjustUsage);
                }

                var reserveSeat = new SectionSeatUsage
                {
                    SeatId = sectionSeat.Id,
                    SectionId = sectionSeat.SectionId,
                    Amount = request.UsageAmount.Value,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = requester
                };

                usages.Add(reserveSeat);
            }

            return (sectionSeats, usages);
        }

        public IEnumerable<SectionSeatUsage> CutSeat(StudyCourse studyCourse, IEnumerable<SectionSeat> defaultSeats, Guid sectionSeatId, int amount, RegistrationChannel channel, string requester)
        {
            var sectionSeat = _dbContext.SectionSeats.SingleOrDefault(x => x.Id == sectionSeatId);

            if (sectionSeat is null)
            {
                throw new SectionException.SeatNotFound(sectionSeatId);
            }

            sectionSeat.SeatUsed += amount;

            if (channel == RegistrationChannel.STUDENT 
                && sectionSeat.TotalSeat - sectionSeat.SeatUsed < 0)
            {
                throw new RegistrationException.NoAvailableSeat(sectionSeat.SectionId);
            }

            var usages = new List<SectionSeatUsage>();

            // NORMAL USAGE
            var adjustUsage = new SectionSeatUsage
            {
                SeatId = sectionSeatId,
                Amount = amount,
                SectionId = sectionSeat.SectionId,
                StudyCourse = studyCourse,
                StudentId = studyCourse.StudentId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = requester
            };

            usages.Add(adjustUsage);
            
            if (defaultSeats is null || !defaultSeats.Any())
            {
                return usages;
            }

            // PARENT DEFAULT USAGES
            if (sectionSeat.Type == SeatType.DEFAULT)
            {
                defaultSeats = defaultSeats.Where(x => x.Id != sectionSeatId);
            }

            foreach (var defaultSeat in defaultSeats)
            {
                defaultSeat.SeatUsed += amount;

                if (channel == RegistrationChannel.STUDENT 
                    && defaultSeat.TotalSeat - defaultSeat.SeatUsed < 0)
                {
                    throw new RegistrationException.NoAvailableSeat(defaultSeat.SectionId);
                }

                var defaultUsage = new SectionSeatUsage
                {
                    SeatId = defaultSeat.Id,
                    Amount = amount,
                    SectionId = defaultSeat.SectionId,
                    StudyCourse = studyCourse,
                    StudentId = studyCourse.StudentId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = requester
                };

                usages.Add(defaultUsage);
            }

            return usages;
        }
    	
        public IEnumerable<SectionSeatUsage> ReturnSeats(Guid studyCourseId, string requester)
        {
            var usages = _dbContext.SectionSeatUsages.AsNoTracking()
                                                     .Where(x => x.StudyCourseId.HasValue
                                                                 && x.StudyCourseId == studyCourseId)
                                                     .ToList();

            var reverseUsages = (from usage in usages
                                 select new SectionSeatUsage
                                 {
                                     SectionId = usage.SectionId,
                                     StudentId = usage.StudentId,
                                     SeatId = usage.SeatId,
                                     Amount = usage.Amount * -1,
                                     ReferenceSeatId = usage.ReferenceSeatId,
                                     StudyCourseId = usage.StudyCourseId,
                                     CreatedAt = DateTime.UtcNow,
                                     CreatedBy = requester
                                 })
                                .ToList();

            return reverseUsages;
        }

        public static SectionSeatDTO MapModelToDTO(SectionSeat model)
        {
            var response = new SectionSeatDTO
            {
                Id = model.Id,
                SectionId = model.SectionId,
                Name = model.Name,
                Type = model.Type,
                MasterSeatId = model.MasterSeatId,
                Remark = model.Remark,
                Conditions = string.IsNullOrEmpty(model.Conditions) ? Enumerable.Empty<SectionConditionDTO>()
                                                                    : JsonConvert.DeserializeObject<IEnumerable<SectionConditionDTO>>(model.Conditions),
                TotalSeat = model.TotalSeat,
                SeatUsed = model.SeatUsed,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };

            return response;
        }

        private static int? GenerateUsage(int amount, int remainingSeat)
        {
            if (amount > 0 && remainingSeat < amount)
            {
                throw new SectionException.NotEnoughRemainingSeat(remainingSeat);
            }

            if (amount != 0)
            {
                remainingSeat -= amount;

                return amount;
            }

            return null;
        }
    }
}