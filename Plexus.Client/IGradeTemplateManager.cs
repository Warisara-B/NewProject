using Plexus.Client.ViewModel.Academic;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IGradeTemplateManager
    {        
        GradeTemplateViewModel Create(CreateGradeTemplateViewModel request, Guid userId);
        GradeTemplateViewModel GetById(Guid gradeId);
        PagedViewModel<GradeTemplateViewModel> Search(SearchGradeTemplateViewModel parameters, int page, int pageSize);
        GradeTemplateViewModel Update(GradeTemplateViewModel request, Guid userId);
        void Delete(Guid id);

    }
}
