using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Plexus.Database.Enum.Academic.Curriculum;
using Plexus.Database.Model;
using Plexus.Database.Model.Academic;
using Plexus.Database.Model.Academic.Advising;
using Plexus.Database.Model.Academic.Curriculum;
using Plexus.Database.Model.Academic.Faculty;
using Plexus.Database.Model.Academic.Section;
using Plexus.Database.Model.Announcement;
using Plexus.Database.Model.Facility;
using Plexus.Database.Model.Facility.Reservation;
using Plexus.Database.Model.Localization;
using Plexus.Database.Model.Localization.Academic;
using Plexus.Database.Model.Localization.Academic.Curriculum;
using Plexus.Database.Model.Localization.Academic.Faculty;
using Plexus.Database.Model.Localization.Announcement;
using Plexus.Database.Model.Localization.Facility;
using Plexus.Database.Model.Localization.Payment;
using Plexus.Database.Model.Localization.Research;
using Plexus.Database.Model.Notification;
using Plexus.Database.Model.Payment;
using Plexus.Database.Model.Payment.Scholarship;
using Plexus.Database.Model.Registration;
using Plexus.Database.Model.Research;

namespace Plexus.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() { }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        #region MASTER DATA

        public DbSet<AcademicLevel> AcademicLevels { get; set; }
        public DbSet<AcademicLevelLocalization> AcademicLevelLocalizations { get; set; }

        public DbSet<AcademicProgram> AcademicPrograms { get; set; }
        public DbSet<AcademicProgramLocalization> AcademicProgramLocalizations { get; set; }

        public DbSet<AcademicPosition> AcademicPositions { get; set; }
        public DbSet<AcademicPositionLocalization> AcademicPositionLocalizations { get; set; }

        public DbSet<CareerPosition> CareerPositions { get; set; }
        public DbSet<CareerPositionLocalization> CareerPositionLocalizations { get; set; }

        public DbSet<Term> Terms { get; set; }

        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<FacultyLocalization> FacultyLocalizations { get; set; }

        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentLocalization> DepartmentLocalizations { get; set; }

        public DbSet<Period> Periods { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<SlotCondition> SlotConditions { get; set; }

        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseLocalization> CourseLocalizations { get; set; }

        public DbSet<CourseTopic> CourseTopics { get; set; }
        public DbSet<CourseTopicInstructor> CourseTopicInstructors { get; set; }
        public DbSet<CourseTopicLocalization> CourseTopicLocalizations { get; set; }

        public DbSet<Grade> Grades { get; set; }
        public DbSet<GradeMaintenance> GradeMaintenance { get; set; }
        public DbSet<GradeTemplate> GradeTemplates { get; set; }
        public DbSet<GradeTemplateDetail> GradeTemplateDetails { get; set; }

        public DbSet<Campus> Campuses { get; set; }
        public DbSet<CampusLocalization> CampusLocalizations { get; set; }

        public DbSet<Building> Buildings { get; set; }
        public DbSet<BuildingLocalization> BuildingLocalizations { get; set; }

        public DbSet<BuildingAvailableTime> BuildingAvailableTimes { get; set; }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomLocalization> RoomLocalizations { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<RoomFacility> RoomFacilities { get; set; }

        public DbSet<Facility> Facilities { get; set; }
        public DbSet<FacilityLocalization> FacilityLocalizations { get; set; }
        #endregion

        #region AUTHORIZATION
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        #endregion

        public DbSet<Student> Students { get; set; }
        public DbSet<StudentLocalization> StudentLocalizations { get; set; }
        public DbSet<StudentTerm> StudentTerms { get; set; }
        public DbSet<StudentCurriculumLog> StudentCurriculumLogs { get; set; }

        public DbSet<StudentAcademicStatus> StudentAcademicStatuses { get; set; }

        public DbSet<StudentGuardian> StudentGuardians { get; set; }
        public DbSet<StudentGuardianLocalization> StudentGuardianLocalizations { get; set; }

        public DbSet<StudentAddress> StudentAddresses { get; set; }

        public DbSet<Passport> Passports { get; set; }

        public DbSet<Bank> Banks { get; set; }

        public DbSet<StudentBankAccount> StudentBankAccounts { get; set; }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeLocalization> EmployeeLocalizations { get; set; }

        public DbSet<InstructorType> InstructorTypes { get; set; }
        public DbSet<InstructorTypeLocalization> InstructorTypeLocalizations { get; set; }

        public DbSet<InstructorAddress> InstructorAddresses { get; set; }

        public DbSet<EmployeeGroup> EmployeeGroups { get; set; }
        public DbSet<EmployeeGroupLocalization> EmployeeGroupLocalizations { get; set; }

        public DbSet<EmployeeWorkInformation> EmployeeWorkInformations { get; set; }

        public DbSet<EmployeeExpertise> EmployeeExpertises { get; set; }

        public DbSet<EmployeeEducationalBackground> EmployeeEducationalBackgrounds { get; set; }

        public DbSet<InstructorAcademicLevel> InstructorAcademicLevels { get; set; }

        public DbSet<Curriculum> Curriculums { get; set; }
        public DbSet<CurriculumLocalization> CurriculumLocalizations { get; set; }

        public DbSet<CurriculumVersion> CurriculumVersions { get; set; }
        public DbSet<CurriculumVersionLocalization> CurriculumVersionLocalizations { get; set; }

        public DbSet<CurriculumInstructor> CurriculumInstructors { get; set; }

        public DbSet<CurriculumCourseGroup> CurriculumCourseGroups { get; set; }
        public DbSet<CurriculumCourseGroupLocalization> CurriculumCourseGroupLocalizations { get; set; }

        public DbSet<CurriculumCourse> CurriculumCourses { get; set; }
        public DbSet<CurriculumCourseBlackList> CurriculumCourseBlackLists { get; set; }

        public DbSet<CurriculumCourseGroupIgnoreCourse> CurriculumCourseGroupIgnoreCourses { get; set; }

        public DbSet<AcademicSpecialization> AcademicSpecializations { get; set; }
        public DbSet<AcademicSpecializationLocalization> AcademicSpecializationLocalizations { get; set; }

        public DbSet<SpecializationCourse> SpecializationCourses { get; set; }

        public DbSet<CurriculumVersionSpecialization> CurriculumVersionSpecializations { get; set; }

        public DbSet<Section> Sections { get; set; }
        public DbSet<SectionInstructor> SectionInstructors { get; set; }
        public DbSet<SectionClassPeriod> SectionClassPeriods { get; set; }
        public DbSet<SectionSchedule> SectionSchedules { get; set; }
        public DbSet<SectionClassPeriodInstructor> SectionClassPeriodInstructors { get; set; }
        public DbSet<SectionDetail> SectionDetails { get; set; }
        public DbSet<SectionExamination> SectionExaminations { get; set; }
        public DbSet<ExclusionCondition> ExclusionConditions { get; set; }
        public DbSet<SectionSeat> SectionSeats { get; set; }
        public DbSet<SectionSeatUsage> SectionSeatUsages { get; set; }

        public DbSet<TeachingType> TeachingTypes { get; set; }
        public DbSet<TeachingTypeLocalization> TeachingTypeLocalizations { get; set; }

        //public DbSet<FlaggedCourse> FlaggedCourses { get; set; }

        //public DbSet<Plan> Plans { get; set; }
        //public DbSet<PlanCourse> PlanCourses { get; set; }
        //public DbSet<PlanSchedule> PlanSchedules { get; set; }
        //public DbSet<PlanScheduleSection> PlanScheduleSections { get; set; }

        //public DbSet<SlotCondition> SlotConditions { get; set; }
        //public DbSet<Condition> Conditions { get; set; }

        public DbSet<CoursePrerequisite> CoursePrerequisites { get; set; }
        public DbSet<CurriculumCoursePrerequisite> CurriculumCoursePrerequisites { get; set; }


        public DbSet<Corequisite> Corequisites { get; set; }

        public DbSet<EquivalentCourse> EquivalentCourses { get; set; }

        public DbSet<CourseRecommendation> CourseRecommendations { get; set; }
        public DbSet<StudyCourse> StudyCourses { get; set; }
        public DbSet<RegistrationLog> RegistrationLogs { get; set; }
        public DbSet<RegistrationLogCourse> RegistrationLogCourses { get; set; }

        public DbSet<WithdrawalRequest> WithdrawalRequests { get; set; }
        public DbSet<GradeLog> GradeLogs { get; set; }

        public DbSet<FeeItem> FeeItems { get; set; }
        public DbSet<FeeItemLocalization> FeeItemLocalizations { get; set; }

        public DbSet<FeeGroup> FeeGroups { get; set; }

        public DbSet<TermFeePackage> TermFeePackages { get; set; }
        public DbSet<TermFeePackageLocalization> TermFeePackageLocalizations { get; set; }

        public DbSet<TermFeeItem> TermFeeItems { get; set; }

        public DbSet<CourseRate> CourseRates { get; set; }
        public DbSet<CourseRateIndex> CourseRateIndexes { get; set; }
        public DbSet<CourseRateIndexTransaction> CourseRateIndexTransactions { get; set; }

        public DbSet<RateType> RateTypes { get; set; }

        public DbSet<RateTypeLocalization> RateTypeLocalizations { get; set; }

        public DbSet<CourseFee> CourseFees { get; set; }

        public DbSet<StudentFeeType> StudentFeeTypes { get; set; }
        public DbSet<StudentFeeTypeLocalization> StudentFeeTypeLocalizations { get; set; }

        public DbSet<ScholarshipType> ScholarshipTypes { get; set; }

        public DbSet<Scholarship> Scholarships { get; set; }

        public DbSet<ScholarshipFeeItem> ScholarshipFeeItems { get; set; }
        public DbSet<ScholarshipFeeItemTransaction> ScholarshipFeeItemTransactions { get; set; }

        public DbSet<ScholarshipReserveBudget> ScholarshipReservedBudgets { get; set; }

        public DbSet<StudentScholarship> StudentScholarships { get; set; }
        public DbSet<StudentScholarshipReserveBudget> StudentScholarshipReserveBudgets { get; set; }
        public DbSet<StudentScholarshipUsage> StudentScholarshipUsages { get; set; }

        public DbSet<CourseTrack> CourseTracks { get; set; }
        public DbSet<CourseTrackDetail> CourseTrackDetails { get; set; }
        public DbSet<StudentCourseTrack> StudentCourseTracks { get; set; }
        public DbSet<CourseTrackLocalization> CourseTrackLocalizations { get; set; }

        public DbSet<RoomReserveRequest> RoomReserveRequests { get; set; }
        public DbSet<RoomReserveSlot> RoomReserveSlots { get; set; }

        public DbSet<AudienceGroup> AudienceGroups { get; set; }

        public DbSet<ArticleType> ArticleTypes { get; set; }
        public DbSet<ArticleTypeLocalization> ArticleTypeLocalizations { get; set; }

        public DbSet<Publication> Publications { get; set; }

        public DbSet<ResearchCategory> ResearchCategories { get; set; }
        public DbSet<ResearchCategoryLocalization> ResearchCategoryLocalizations { get; set; }

        public DbSet<ResearchTemplate> ResearchTemplates { get; set; }
        public DbSet<ResearchTemplateLocalization> ResearchTemplateLocalizations { get; set; }

        public DbSet<ResearchTemplateSequence> ResearchTemplateSequences { get; set; }
        public DbSet<ResearchTemplateSequenceLocalization> ResearchTemplateSequenceLocalizations { get; set; }

        public DbSet<ResearchProfile> ResearchProfiles { get; set; }

        public DbSet<ResearchProcess> ResearchProcesses { get; set; }
        public DbSet<ResearchProcessLocalization> ResearchProcessLocalizations { get; set; }

        public DbSet<ResearchMember> ResearchMembers { get; set; }

        public DbSet<ResearchResource> ResearchResources { get; set; }

        public DbSet<ResearchCommittee> ResearchCommittees { get; set; }

        public DbSet<Deformation> Deformations { get; set; }

        public DbSet<AdvisingSlot> AdvisingSlots { get; set; }

        public DbSet<InstructorRole> InstructorRoles { get; set; }
        public DbSet<InstructorRoleLocalization> instructorRoleLocalizations { get; set; }

        //public DbSet<FeeItem> FeeItems { get; set; }
        //public DbSet<CourseFee> CourseFees { get; set; }
        //public DbSet<CourseRate> CourseRates { get; set; }
        //public DbSet<TermFee> TermFees { get; set; }
        //public DbSet<TermFeeGroup> TermFeeGroups { get; set; }

        public DbSet<NewsCategory> NewsCategories { get; set; }
        public DbSet<NewsCategoryLocalization> NewsCategoryLocalizations { get; set; }

        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<PublisherLocalization> PublisherLocalizations { get; set; }

        public DbSet<News> News { get; set; }
        public DbSet<NewsLocalization> NewsLocalization { get; set; }
        public DbSet<BookmarkNews> BookmarkNews { get; set; }

        public DbSet<StudyPlan> StudyPlans { get; set; }
        public DbSet<StudyPlanDetail> StudyPlanDetails { get; set; }

        public DbSet<AcademicCalendar> AcademicCalendars { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationStudent> NotificationStudents { get; set; }
        public DbSet<NotificationImage> NotificationImages { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // IF NOT GIVEN CONFIGURATION SET TO LOCAL DB
                optionsBuilder.UseSqlServer("Server=OS_NARUEDON;Initial Catalog=develop;Integrated Security=True;");
                //optionsBuilder.UseSqlServer("Server=tcp:localhost,1433;Initial Catalog=Plexus;Persist Security Info=False;User ID=sa;Password=reallyStrongPwd123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Faculty>()
                        .HasIndex(x => x.Code)
                        .IsUnique();

            modelBuilder.Entity<Department>()
                        .HasIndex(x => new { x.FacultyId, x.Code })
                        .IsUnique();

            modelBuilder.Entity<Grade>()
                        .HasIndex(x => x.Letter)
                        .IsUnique();

            modelBuilder.Entity<GradeTemplate>()
                        .HasIndex(x => x.Name);

            modelBuilder.Entity<GradeTemplateDetail>()
                        .HasKey(x => new { x.GradeTemplateId, x.GradeId });

            modelBuilder.Entity<Term>()
                        .HasIndex(x => new { x.AcademicLevelId, x.Year, x.Number })
                        .IsUnique();

            modelBuilder.Entity<BuildingAvailableTime>()
                        .HasKey(x => new { x.BuildingId, x.Day });

            modelBuilder.Entity<CurriculumCourse>()
                        .HasKey(x => new { x.CourseGroupId, x.CourseId });

            modelBuilder.Entity<CurriculumCourseGroupIgnoreCourse>()
                        .HasKey(x => new { x.CourseGroupId, x.CourseId });

            modelBuilder.Entity<CurriculumCourseBlackList>()
                        .HasKey(x => new { x.CurriculumVersionId, x.CourseId });

            modelBuilder.Entity<AcademicLevelLocalization>()
                        .HasKey(x => new { x.AcademicLevelId, x.Language });

            modelBuilder.Entity<AcademicPositionLocalization>()
                        .HasKey(x => new { x.AcademicPositionId, x.Language });

            modelBuilder.Entity<CareerPositionLocalization>()
                        .HasKey(x => new { x.CareerPositionId, x.Language });

            modelBuilder.Entity<FacultyLocalization>()
                        .HasKey(x => new { x.FacultyId, x.Language });

            modelBuilder.Entity<DepartmentLocalization>()
                        .HasKey(x => new { x.DepartmentId, x.Language });

            modelBuilder.Entity<CampusLocalization>()
                        .HasKey(x => new { x.CampusId, x.Language });

            modelBuilder.Entity<BuildingLocalization>()
                        .HasKey(x => new { x.BuildingId, x.Language });

            modelBuilder.Entity<RoomLocalization>()
                        .HasKey(x => new { x.RoomId, x.Language });

            modelBuilder.Entity<CourseLocalization>()
                        .HasKey(x => new { x.CourseId, x.Language });

            modelBuilder.Entity<CourseTopicLocalization>()
                        .HasKey(x => new { x.CourseTopicId, x.Language });

            modelBuilder.Entity<TeachingTypeLocalization>()
                        .HasKey(x => new { x.TeachingTypeId, x.Language });

            modelBuilder.Entity<CurriculumLocalization>()
                        .HasKey(x => new { x.CurriculumId, x.Language });

            modelBuilder.Entity<CurriculumVersionLocalization>()
                        .HasKey(x => new { x.CurriculumVersionId, x.Language });

            modelBuilder.Entity<CurriculumCourseGroupLocalization>()
                        .HasKey(x => new { x.CurriculumCourseGroupId, x.Language });

            modelBuilder.Entity<AcademicSpecializationLocalization>()
                        .HasKey(x => new { x.AcademicSpecializationId, x.Language });

            modelBuilder.Entity<SpecializationCourse>()
                        .HasKey(x => new { x.AcademicSpecializationId, x.CourseId });

            modelBuilder.Entity<CurriculumVersionSpecialization>()
                        .HasKey(x => new { x.CurriculumVersionId, x.AcademicSpecializationId });

            modelBuilder.Entity<Corequisite>()
                        .HasKey(x => new { x.CurriculumVersionId, x.CourseId, x.CorequisiteCourseId });

            modelBuilder.Entity<EquivalentCourse>()
                        .HasKey(x => new { x.CurriculumVersionId, x.CourseId, x.EquivalenceCourseId });

            modelBuilder.Entity<StudentLocalization>()
                        .HasKey(x => new { x.StudentId, x.Language });

            modelBuilder.Entity<Section>()
                        .HasIndex(x => new { x.TermId, x.CourseId, x.SectionNo })
                        .IsUnique();

            modelBuilder.Entity<StudentTerm>()
                        .HasKey(x => new { x.StudentId, x.TermId });

            modelBuilder.Entity<FeeItemLocalization>()
                        .HasKey(x => new { x.FeeItemId, x.Language });

            modelBuilder.Entity<CourseRateIndex>()
                        .HasKey(x => new { x.CourseRateId, x.RateTypeId, x.Index });

            modelBuilder.Entity<ScholarshipFeeItem>()
                        .HasKey(x => new { x.ScholarshipId, x.FeeItemId });

            modelBuilder.Entity<ScholarshipReserveBudget>()
                        .HasKey(x => new { x.ScholarshipId, x.Name });

            modelBuilder.Entity<CourseTrack>()
                        .HasIndex(x => x.Code)
                        .IsUnique();

            modelBuilder.Entity<CourseTrackDetail>()
                        .HasKey(x => new { x.CourseTrackId, x.CourseId });

            modelBuilder.Entity<StudentCourseTrack>()
                        .HasKey(x => new { x.StudentId, x.CourseTrackId });

            modelBuilder.Entity<CourseTrackLocalization>()
                        .HasKey(x => new { x.CourseTrackId, x.Language });

            modelBuilder.Entity<EmployeeLocalization>()
                        .HasKey(x => new { x.EmployeeId, x.Language });

            modelBuilder.Entity<AcademicProgramLocalization>()
                        .HasKey(x => new { x.AcademicProgramId, x.Language });

            modelBuilder.Entity<FacilityLocalization>()
                        .HasKey(x => new { x.FacilityId, x.Language });

            modelBuilder.Entity<RoomFacility>()
                        .HasKey(x => new { x.RoomId, x.FacilityId });

            modelBuilder.Entity<StudentFeeTypeLocalization>()
                        .HasKey(x => new { x.StudentFeeTypeId, x.Language });

            modelBuilder.Entity<TermFeePackageLocalization>()
                        .HasKey(x => new { x.TermFeePackageId, x.Language });

            modelBuilder.Entity<RateTypeLocalization>()
                        .HasKey(x => new { x.RateTypeId, x.Language });

            modelBuilder.Entity<InstructorTypeLocalization>()
                        .HasKey(x => new { x.InstructorTypeId, x.Language });

            modelBuilder.Entity<EmployeeGroupLocalization>()
                        .HasKey(x => new { x.EmployeeGroupId, x.Language });

            modelBuilder.Entity<InstructorAcademicLevel>()
                        .HasKey(x => new { x.InstructorId, x.AcademicLevelId });

            modelBuilder.Entity<ArticleTypeLocalization>()
                        .HasKey(x => new { x.ArticleTypeId, x.Language });

            modelBuilder.Entity<ResearchCategoryLocalization>()
                        .HasKey(x => new { x.ResearchCategoryId, x.Language });

            modelBuilder.Entity<ResearchTemplateLocalization>()
                        .HasKey(x => new { x.ResearchTemplateId, x.Language });

            modelBuilder.Entity<ResearchTemplateSequenceLocalization>()
                        .HasKey(x => new { x.SequenceId, x.Language });

            modelBuilder.Entity<ResearchProcessLocalization>()
                        .HasKey(x => new { x.ProcessId, x.Language });

            modelBuilder.Entity<StudentGuardianLocalization>()
                        .HasKey(x => new { x.StudentGuardianId, x.Language });

            modelBuilder.Entity<CourseRecommendation>()
                        .HasKey(x => new { x.StudentId, x.TermId, x.CourseId });

            modelBuilder.Entity<NewsCategoryLocalization>()
                        .HasKey(x => new { x.NewsCategoryId, x.Language });

            modelBuilder.Entity<PublisherLocalization>()
                        .HasKey(x => new { x.PublisherId, x.Language });

            modelBuilder.Entity<NewsLocalization>()
                        .HasKey(x => new { x.NewsId, x.Language });

            modelBuilder.Entity<CoursePrerequisite>()
                        .Property(x => x.Conditions)
                        .HasConversion(
                            x => JsonSerializer.Serialize(x, new JsonSerializerOptions()),
                            x => JsonSerializer.Deserialize<IEnumerable<string>>(x, new JsonSerializerOptions()));

            modelBuilder.Entity<EmployeeWorkInformation>()
                        .HasOne(x => x.Faculty)
                        .WithMany()
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

            modelBuilder.Entity<EmployeeWorkInformation>()
                        .HasOne(x => x.Department)
                        .WithMany()
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

            modelBuilder.Entity<SectionInstructor>()
                        .HasKey(x => new { x.InstructorId, x.SectionId });

            modelBuilder.Entity<Section>()
                        .HasOne(x => x.AcademicLevel)
                        .WithMany()
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

            modelBuilder.Entity<Section>()
                        .HasOne(x => x.Term)
                        .WithMany()
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

            modelBuilder.Entity<Section>()
                        .HasOne(x => x.Course)
                        .WithMany()
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

            modelBuilder.Entity<Section>()
                        .HasOne(x => x.Campus)
                        .WithMany()
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

            modelBuilder.Entity<StudentCurriculumLog>()
                        .HasOne(x => x.Faculty)
                        .WithMany()
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentCurriculumLog>()
                        .HasOne(x => x.Department)
                        .WithMany()
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentCurriculumLog>()
                        .HasOne(x => x.CurriculumVersion)
                        .WithMany()
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<StudentCurriculumLog>()
                        .HasOne(x => x.StudyPlan)
                        .WithMany()
                        .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<InstructorRoleLocalization>()
                        .HasKey(x => new { x.InstructorRoleId, x.Language });

            modelBuilder.Entity<CurriculumCourseGroup>()
                        .Property(x => x.Type)
                        .HasDefaultValue(CourseGroupType.REQUIRED_COURSE);

            modelBuilder.Entity<CourseRate>()
                        .HasOne(x => x.RateType)
                        .WithMany()
                        .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
