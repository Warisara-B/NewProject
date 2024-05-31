using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Plexus.Client;
using Plexus.Client.src;
using Plexus.Client.src.Academic;
using Plexus.Client.src.Academic.Section;

// using Plexus.Client.src.Academic.Section;
using Plexus.Client.src.Facility;
using Plexus.Client.src.Facility.Reservation;
using Plexus.Client.src.Payment;
using Plexus.Client.src.Payment.Scholarship;
using Plexus.Client.src.Registration;
using Plexus.Client.src.Research;
using Plexus.Database;
using Plexus.Database.Model.Academic.Section;
using Plexus.Database.Repositories;
using Plexus.Entity.Provider;
using Plexus.Entity.Provider.src;
using Plexus.Entity.Provider.src.Academic;
using Plexus.Entity.Provider.src.Academic.Curriculum;
using Plexus.Entity.Provider.src.Academic.Section;
using Plexus.Entity.Provider.src.Facility;
using Plexus.Entity.Provider.src.Facility.Reservation;
using Plexus.Entity.Provider.src.Payment;
using Plexus.Entity.Provider.src.Payment.Scholarship;
using Plexus.Entity.Provider.src.Registration;
using Plexus.Entity.Provider.src.Research;
using Plexus.Integration;
using Plexus.Integration.config;
using Plexus.Integration.src;
using Plexus.Service;
using Plexus.Service.Config;
using Plexus.Service.src;
using Plexus.Utility.Exception;
using ServiceStack;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers()
                .AddNewtonsoftJson(option => option.SerializerSettings.DateFormatString = "yyyy'-'MM'-'dd'T'HH':'mm':'ss");

builder.Services.AddApiVersioning(
                options => options.AssumeDefaultVersionWhenUnspecified = true);

// Register JWT AUthentication Config
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        // ALLOW READ "sub" on Default .NET JWT
                        ValidateIssuer = true,
                        ValidIssuer = configuration["JWT:ValidIssuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["JWT:ValidAudience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                    };
                });

// Register Authorize Policy
builder.Services.AddAuthorization(options =>
{
    // Add Default Policy for [Authorization] property
    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireAuthenticatedUser()
                                                                                                  .Build();
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Handle DbContext and configuration section
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(configuration.GetConnectionString("DatabaseContext")));
builder.Services.Configure<BlobStorageConfiguration>(configuration.GetSection("Blob"));
builder.Services.Configure<JWTConfiguration>(configuration.GetSection("JWT"));

// Dependency injection
builder.Services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IPeriodAndSlotManager, PeriodAndSlotManager>();
builder.Services.AddScoped<IPeriodAndSlotProvider, PeriodAndSlotProvider>();
builder.Services.AddScoped<IAcademicLevelManager, AcademicLevelManager>();
builder.Services.AddScoped<IAcademicLevelProvider, AcademicLevelProvider>();
builder.Services.AddScoped<ITermManager, TermManager>();
builder.Services.AddScoped<ITermProvider, TermProvider>();
builder.Services.AddScoped<IFacultyManager, FacultyManager>();
builder.Services.AddScoped<IFacultyProvider, FacultyProvider>();
builder.Services.AddScoped<IDepartmentManager, DepartmentManager>();
builder.Services.AddScoped<IDepartmentProvider, DepartmentProvider>();
builder.Services.AddScoped<IGradeManager, GradeManager>();
builder.Services.AddScoped<IGradeProvider, GradeProvider>();
builder.Services.AddScoped<IGradeMaintenanceManager, GradeMaintenanceManager>();
builder.Services.AddScoped<IGradeMaintenanceProvider, GradeMaintenanceProvider>();
builder.Services.AddScoped<IGradeTemplateManager, GradeTemplateManager>();
builder.Services.AddScoped<IGradeTemplateProvider, GradeTemplateProvider>();
builder.Services.AddScoped<IGradingManager, GradingManager>();
builder.Services.AddScoped<IGradingProvider, GradingProvider>();
builder.Services.AddScoped<ICourseManager, CourseManager>();
builder.Services.AddScoped<ICourseProvider, CourseProvider>();
builder.Services.AddScoped<ICampusManager, CampusManager>();
builder.Services.AddScoped<ICampusProvider, CampusProvider>();
builder.Services.AddScoped<IRoomManager, RoomManager>();
builder.Services.AddScoped<IRoomProvider, RoomProvider>();
builder.Services.AddScoped<IRoomTypeManager, RoomTypeManager>();
builder.Services.AddScoped<IRoomTypeProvider, RoomTypeProvider>();
builder.Services.AddScoped<IBuildingManager, BuildingManager>();
builder.Services.AddScoped<IBuildingProvider, BuildingProvider>();
builder.Services.AddScoped<ICurriculumManager, CurriculumManager>();
builder.Services.AddScoped<ICurriculumProvider, CurriculumProvider>();
builder.Services.AddScoped<ICurriculumVersionManager, CurriculumVersionManager>();
builder.Services.AddScoped<ICurriculumVersionProvider, CurriculumVersionProvider>();
builder.Services.AddScoped<ICurriculumCourseGroupManager, CurriculumCourseGroupManager>();
builder.Services.AddScoped<ICurriculumCourseGroupProvider, CurriculumCourseGroupProvider>();
builder.Services.AddScoped<IAcademicSpecializationProvider, AcademicSpecializationProvider>();
builder.Services.AddScoped<IAcademicSpecializationManager, AcademicSpecializationManager>();
builder.Services.AddScoped<IStudentProvider, StudentProvider>();
builder.Services.AddScoped<IStudentManager, StudentManager>();
builder.Services.AddScoped<IStudentGuardianProvider, StudentGuardianProvider>();
builder.Services.AddScoped<IStudentGuardianManager, StudentGuardianManager>();
builder.Services.AddScoped<IStudentAddressProvider, StudentAddressProvider>();
builder.Services.AddScoped<IStudentAddressManager, StudentAddressManager>();
builder.Services.AddScoped<ISectionProvider, SectionProvider>();
builder.Services.AddScoped<ISectionManager, SectionManager>();
builder.Services.AddScoped<IStudyCourseProvider, StudyCourseProvider>();
builder.Services.AddScoped<IStudyCourseManager, StudyCourseManager>();
builder.Services.AddScoped<IStudentTermProvider, StudentTermProvider>();
builder.Services.AddScoped<IStudentTermManager, StudentTermManager>();
builder.Services.AddScoped<IFeeItemProvider, FeeItemProvider>();
builder.Services.AddScoped<IFeeItemManager, FeeItemManager>();
builder.Services.AddScoped<ICourseRateProvider, CourseRateProvider>();
builder.Services.AddScoped<ICourseRateManager, CourseRateManager>();
builder.Services.AddScoped<ITermFeePackageProvider, TermFeePackageProvider>();
builder.Services.AddScoped<ITermFeePackageManager, TermFeePackageManager>();
builder.Services.AddScoped<ICourseFeeProvider, CourseFeeProvider>();
builder.Services.AddScoped<ICourseFeeManager, CourseFeeManager>();
builder.Services.AddScoped<IRateTypeProvider, RateTypeProvider>();
builder.Services.AddScoped<IRateTypeManager, RateTypeManager>();
builder.Services.AddScoped<IScholarshipTypeProvider, ScholarshipTypeProvider>();
builder.Services.AddScoped<IScholarshipTypeManager, ScholarshipTypeManager>();
builder.Services.AddScoped<IScholarshipProvider, ScholarshipProvider>();
builder.Services.AddScoped<IScholarshipManager, ScholarshipManager>();
builder.Services.AddScoped<IStudentScholarshipProvider, StudentScholarshipProvider>();
builder.Services.AddScoped<IStudentScholarshipManager, StudentScholarshipManager>();
builder.Services.AddScoped<ICourseTopicProvider, CourseTopicProvider>();
builder.Services.AddScoped<ICourseTopicManager, CourseTopicManager>();
builder.Services.AddScoped<ICourseTrackProvider, CourseTrackProvider>();
builder.Services.AddScoped<ICourseTrackManager, CourseTrackManager>();
builder.Services.AddScoped<IStudentCourseTrackProvider, StudentCourseTrackProvider>();
builder.Services.AddScoped<IStudentCourseTrackManager, StudentCourseTrackManager>();
builder.Services.AddScoped<IExclusionConditionProvider, ExclusionConditionProvider>();
builder.Services.AddScoped<IExclusionConditionManager, ExclusionConditionManager>();
builder.Services.AddScoped<IEmployeeProvider, EmployeeProvider>();
builder.Services.AddScoped<IEmployeeManager, EmployeeManager>();
builder.Services.AddScoped<ISectionSeatProvider, SectionSeatProvider>();
builder.Services.AddScoped<ISectionSeatManager, SectionSeatManager>();
builder.Services.AddScoped<IAcademicProgramProvider, AcademicProgramProvider>();
builder.Services.AddScoped<IAcademicProgramManager, AcademicProgramManager>();
// builder.Services.AddScoped<IOfferedCourseManager, OfferedCourseManager>();
// builder.Services.AddScoped<IOfferedCourseProvider, OfferedCourseProvider>();
builder.Services.AddScoped<IFacilityManager, FacilityManager>();
builder.Services.AddScoped<IFacilityProvider, FacilityProvider>();
builder.Services.AddScoped<IRoomReservationManager, RoomReservationManager>();
builder.Services.AddScoped<IRoomReservationProvider, RoomReservationProvider>();
builder.Services.AddScoped<IAudienceGroupManager, AudienceGroupManager>();
builder.Services.AddScoped<IAudienceGroupProvider, AudienceGroupProvider>();
builder.Services.AddScoped<ISlotConditionManager, SlotConditionManager>();
builder.Services.AddScoped<ISlotConditionProvider, SlotConditionProvider>();
builder.Services.AddScoped<IRegistrationManager, RegistrationManager>();
builder.Services.AddScoped<ITransferCourseManager, TransferCourseManager>();
builder.Services.AddScoped<IStudentFeeTypeManager, StudentFeeTypeManager>();
builder.Services.AddScoped<IStudentFeeTypeProvider, StudentFeeTypeProvider>();
builder.Services.AddScoped<IWithdrawalRequestProvider, WithdrawalRequestProvider>();
builder.Services.AddScoped<IWithdrawalManager, WithdrawManager>();
builder.Services.AddScoped<IPrerequisiteManager, PrerequisiteManager>();
builder.Services.AddScoped<IPrerequisiteProvider, PrerequisiteProvider>();
builder.Services.AddScoped<IFeeGroupManager, FeeGroupManager>();
builder.Services.AddScoped<ITermFeeItemProvider, TermFeeItemProvider>();
builder.Services.AddScoped<ITermFeeItemManager, TermFeeItemManager>();
builder.Services.AddScoped<IInstructorTypeProvider, InstructorTypeProvider>();
builder.Services.AddScoped<IInstructorTypeManager, InstructorTypeManager>();
builder.Services.AddScoped<IEmployeeGroupProvider, EmployeeGroupProvider>();
builder.Services.AddScoped<IEmployeeGroupManager, EmployeeGroupManager>();
builder.Services.AddScoped<IArticleTypeProvider, ArticleTypeProvider>();
builder.Services.AddScoped<IArticleTypeManager, ArticleTypeManager>();
builder.Services.AddScoped<IPublicationProvider, PublicationProvider>();
builder.Services.AddScoped<IPublicationManager, PublicationManager>();
builder.Services.AddScoped<IResearchCategoryProvider, ResearchCategoryProvider>();
builder.Services.AddScoped<IResearchCategoryManager, ResearchCategoryManager>();
builder.Services.AddScoped<ITeachingTypeProvider, TeachingTypeProvider>();
builder.Services.AddScoped<ITeachingTypeManager, TeachingTypeManager>();
builder.Services.AddScoped<IResearchTemplateProvider, ResearchTemplateProvider>();
builder.Services.AddScoped<IResearchTemplateManager, ResearchTemplateManager>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IGradeService, GradeService>();
builder.Services.AddScoped<IAdvisingService, AdvisingService>();
builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
builder.Services.AddScoped<IStudyPlanManager, StudyPlanManager>();
builder.Services.AddScoped<Plexus.Service.IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<ICourseTopicProvider, CourseTopicProvider>();
builder.Services.AddScoped<ICourseTopicManager, CourseTopicManager>();
builder.Services.AddScoped<IAcademicCalendarService, AcademicCalendarService>();
builder.Services.AddScoped<IAcademicPositionManager, AcademicPositionManager>();
builder.Services.AddScoped<ICareerPositionManager, CareerPositionManager>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<ITeachingTypeProvider, TeachingTypeProvider>();
builder.Services.AddScoped<ITeachingTypeManager, TeachingTypeManager>();
builder.Services.AddScoped<IInstructorRoleProvider, InstructorRoleProvider>();
builder.Services.AddScoped<IInstructorRoleManager, InstructorRoleManager>();
builder.Services.AddScoped<ICurriculumInstructorManager, CurriculumInstructorManager>();
builder.Services.AddScoped<ITermService, TermService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();

builder.Services.AddSingleton<IBlobStorageProvider, BlobStorageProvider>();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .SetIsOriginAllowed(x => true);
    });
});

var app = builder.Build();

app.UseMiddleware<CustomExceptionHandlerMiddleware>();

// migrate any database changes on startup (includes initial db creation)
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    dataContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
