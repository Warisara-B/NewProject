using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client.ViewModel.Academic;
using Plexus.Entity.DTO.Academic;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Client
{
    public interface IGradingManager
    {        
        List<GradingViewModel> ImportScore([FromForm] IFormFile file, Guid userId);

        List<GradingViewModel> Grading(List<CreateGradingViewModel> dtoList, string activity, string adjustmentValue, string grade,Guid userId);

        List<GradingViewModel> NewGrading(List<CreateGradingViewModel> dtoList,int format, string interval, string grades, string maxScore, string minScore, Guid userId);

    }
}
