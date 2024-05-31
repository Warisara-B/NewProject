using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Plexus.Database;
using Plexus.Database.Enum;
using Plexus.Database.Enum.Academic;
using Plexus.Database.Model;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Academic.Curriculum;
using Plexus.Service.Exception;
using Plexus.Service.ViewModel.Term;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Service.src
{
    public class TermService : ITermService
    {
        private readonly IAsyncRepository<Student> _studentRepo;
        private readonly IAsyncRepository<StudyCourse> _studyCourseRepo;
        private readonly IAsyncRepository<CurriculumVersion> _curriculumVersionRepo;
        private readonly IAsyncRepository<Term> _termRepo;

        public TermService(IAsyncRepository<Student> studentRepo
            , IAsyncRepository<StudyCourse> studyCourseRepo
            , IAsyncRepository<CurriculumVersion> curriculumVersionRepo
            , IAsyncRepository<Term> termRepo)
        {
            _studentRepo = studentRepo;
            _studyCourseRepo = studyCourseRepo;
            _curriculumVersionRepo = curriculumVersionRepo;
            _termRepo = termRepo;
        }

        public List<TermViewModel> GetAllTerms(Guid studentId, LanguageCode language)
        {
            var std = _studentRepo.Query().FirstOrDefault(x => x.Id == studentId);

            if (std is null)
            {
                throw new TermException.NotFound();
            }

            var curriculumVers = _curriculumVersionRepo.Query().FirstOrDefault(x => x.Id == std.CurriculumVersionId);

            if (curriculumVers is null)
            {
                throw new TermException.NotFound();
            }

            var currentTerms = _termRepo.Query()
                .Where(x => x.CollegeCalendarType == curriculumVers.CollegeCalendarType
                && x.AcademicLevelId == curriculumVers.AcademicLevelId && x.IsCurrent)
                .Include(x => x.AcademicLevel)
                .ThenInclude(x => x.Localizations)
                .ToList();

            var stdRegis = _studyCourseRepo.Query().Include(x => x.Term)
                .Where(x => x.StudentId == studentId
                && x.Status == StudyCourseStatus.ACTIVE)
                .Select(x => x.TermId).ToList();

            var stdRegisTerms = _termRepo.Query().Where(x => stdRegis.Contains(x.Id))
                .Include(x => x.AcademicLevel)
                .ThenInclude(x => x.Localizations)
                .ToList();

            var results = MapTermToViewModel(currentTerms, stdRegisTerms, language);


            return results;
        }

        private static List<TermViewModel> MapTermToViewModel(List<Term>? currentTerms, List<Term>? stdRegisTerms, LanguageCode language)
        {
            var _currentTermView = (from data in currentTerms
                                    let _dataAcademicLevel = data.AcademicLevel
                                    let _locale_academic_level = data.AcademicLevel.Localizations.SingleOrDefault(x => x.Language == language)
                                    select new TermViewModel
                                    {
                                        Id = data.Id,
                                        Year = data.Year,
                                        Number = data.Number,
                                        TermType = data.Type,
                                        CollegeCalendarType = data.CollegeCalendarType,
                                        StartedAt = data.StartedAt,
                                        EndedAt = data.EndedAt,
                                        TotalWeeks = data.TotalWeeks,
                                        IsCurrent = data.IsCurrent,
                                        IsStudentRegistration = stdRegisTerms is not null && stdRegisTerms.Count > 0 ? stdRegisTerms.Any(x => x.Id == data.Id) : false,
                                        AcademicLevel = _dataAcademicLevel is not null ? new AcademicLevelObj
                                        {
                                            Id = _dataAcademicLevel.Id,
                                            FormalName = _locale_academic_level?.FormalName ?? _dataAcademicLevel.FormalName,
                                            Name = _locale_academic_level?.Name ?? _dataAcademicLevel.Name
                                        } : null
                                    }).ToList();

            var _RegisTermView = (from data in stdRegisTerms
                                  let _dataAcademicLevel = data.AcademicLevel
                                  let _locale_academic_level = data.AcademicLevel.Localizations.SingleOrDefault(x => x.Language == language)
                                  select new TermViewModel
                                  {
                                      Id = data.Id,
                                      Year = data.Year,
                                      Number = data.Number,
                                      TermType = data.Type,
                                      CollegeCalendarType = data.CollegeCalendarType,
                                      StartedAt = data.StartedAt,
                                      EndedAt = data.EndedAt,
                                      TotalWeeks = data.TotalWeeks,
                                      IsCurrent = data.IsCurrent,
                                      IsStudentRegistration = true,
                                      AcademicLevel = _dataAcademicLevel is not null ? new AcademicLevelObj
                                      {
                                          Id = _dataAcademicLevel.Id,
                                          FormalName = _locale_academic_level?.FormalName ?? _dataAcademicLevel.FormalName,
                                          Name = _locale_academic_level?.Name ?? _dataAcademicLevel.Name
                                      } : null
                                  }).ToList();

            var results = new List<TermViewModel>();
            results.AddRange(_currentTermView);
            results.AddRange(_RegisTermView);

            results = results.DistinctBy(x => x.Id)
                .OrderByDescending(x => x.IsCurrent)
                .ThenByDescending(x => x.EndedAt)
                .ThenByDescending(x => x.StartedAt)
                .ToList();

            return results;
        }
    }
}
