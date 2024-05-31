using Plexus.Database.Enum;
using Plexus.Service.ViewModel.Term;
using Plexus.Utility.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plexus.Service
{
    public interface ITermService
    {
        /// <summary>
        /// Get list of Terms.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        List<TermViewModel> GetAllTerms(Guid studentId, LanguageCode language);
    }
}
