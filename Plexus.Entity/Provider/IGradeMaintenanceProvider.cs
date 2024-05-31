using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IGradeMaintenanceProvider
    {
        GradeMaintenanceDTO Create(CreateGradeMaintenanceDTO request, string requester);
        PagedViewModel<GradeMaintenanceDTO> Search(SearchGradeMaintenanceViewModel parameters, int page, int pageSize);
        PagedViewModel<CourseGradeMaintenanceDTO> SearchCouse(SearchGradeMaintenanceViewModel parameters, int page, int pageSize);
        GradeMaintenanceDTO GetById(Guid gradeId);
        IEnumerable<GradeMaintenanceDTO> GetByName(string gradeName);
        GradeMaintenanceDTO Update(GradeMaintenanceDTO request, string requester);
        void Delete(Guid id);

    }
}
