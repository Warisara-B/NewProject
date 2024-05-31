using Microsoft.AspNetCore.Http;
using Plexus.Client.ViewModel;
using Plexus.Client.ViewModel.DropDown;
using Plexus.Client.ViewModel.SearchFilter;
using Plexus.Entity.DTO;
using Plexus.Utility.ViewModel;

namespace Plexus.Client
{
    public interface IEmployeeManager
    {
        /// <summary>
        /// Create new instructor.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        EmployeeViewModel Create(CreateEmployeeViewModel request, Guid userId);

        /// <summary>
        /// Search employee by given parameter as paged.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        PagedViewModel<EmployeeInformationViewModel> Search(SearchEmployeeCriteriaViewModel parameters, int page = 1, int pageSize = 25);

        /// <summary>
        /// Search instructor by given parameter as drop down.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<InstructorDropDownViewModel> GetDropDownList(SearchCriteriaViewModel parameters);

        /// <summary>
        /// Get instructor's emails by given parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<string> GetEmails(SearchCriteriaViewModel parameters);

        /// <summary>
        /// Get instructor by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EmployeeViewModel GetById(Guid id);

        /// <summary>
        /// Update instructor information.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="requester"></param>
        /// <returns></returns>
        EmployeeViewModel Update(EmployeeViewModel request, Guid userId);

        /// <summary>
        /// Delete instructor by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Guid id);

        /// <summary>
        /// Upload instructor card image.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cardImage"></param>
        /// <returns></returns>
        Task UploadCardImageAsync(Guid id, IFormFile cardImage);

        /// <summary>
        /// Update employee's general information by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        EmployeeInformationViewModel UpdateEmployeeGeneralInformation(Guid id, UpdateEmployeeGeneralInformationViewModel request);

        /// <summary>
        /// Update employee's work information by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        EmployeeInformationViewModel UpdateEmployeeWorkInformation(Guid id, UpdateEmployeeWorkInformationViewModel request);

        /// <summary>
        /// Update employee's educational background by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        EmployeeInformationViewModel UpdateEmployeeEducationalBackground(Guid id, IEnumerable<EmployeeEducationalBackgroundViewModel> request);
    }
}