using Plexus.Entity.DTO.Academic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Entity.Provider
{
    public interface IGradingProvider
    {
        List<GradingDTO> ImportScore(List<CreateGradingDTO> file, string requester);
        List<GradingDTO> Grading(List<CreateGradingDTO> request, string activity, string adjustmentValue, string grade);
        List<GradingDTO> NewGrading(List<CreateGradingDTO> request, int format, string interval, string grades, string maxScore, string minScore, string rangeGrade, string median, string llf);
        IEnumerable<GradingDTO> GetByStudentCode(string studentCode);
    }
}
