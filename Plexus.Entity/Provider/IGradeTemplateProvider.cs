using Plexus.Entity.DTO.Academic;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Entity.Provider
{
    public interface IGradeTemplateProvider
    {
        GradeTemplateDTO Create(CreateGradeTemplateDTO request, string requester);
        IEnumerable<GradeTemplateDTO> GetAll();
        GradeTemplateDTO GetById(Guid gradeId);
        IEnumerable<GradeTemplateDTO> GetByName(string gradeName);
        PagedViewModel<GradeTemplateDTO> Search(SearchGradeTemplateViewModel parameters, int page, int pageSize);
        GradeTemplateDTO Update(GradeTemplateDTO request, string requester);
        void Delete(Guid gradeId);
    }
}
