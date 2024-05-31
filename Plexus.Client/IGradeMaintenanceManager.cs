using Microsoft.AspNetCore.Http;
using Plexus.Client.ViewModel.Academic;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Client
{
    public interface IGradeMaintenanceManager
    {
        GradeMaintenanceViewModel Create(CreateGradeMaintenanceViewModel request,Guid userId);
        GradeMaintenanceViewModel GetById(Guid gradeId);
        GradeMaintenanceViewModel Update(GradeMaintenanceViewModel request, Guid userId);
        PagedViewModel<GradeMaintenanceViewModel> Search(SearchGradeMaintenanceViewModel parameters, int page, int pageSize);
        PagedViewModel<CourseGradeMaintenViewModel> SearchCouse(SearchGradeMaintenanceViewModel parameters, int page, int pageSize);
        void Delete(Guid id); 
    }
}
