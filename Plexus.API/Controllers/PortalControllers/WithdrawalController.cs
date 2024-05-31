using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Plexus.Client;
using Plexus.Client.src;
using Plexus.Client.ViewModel.Academic;
using Plexus.Database.Enum.Academic;
using Plexus.Entity.DTO;
using Plexus.Utility;

namespace Plexus.API.Controllers
{
    // TODO : VALIDATE USERS
    [ApiController]
    [Route(_pathPrefix + "[controller]")]
    public class WithdrawalController : BaseController
    {
        private readonly IWithdrawalManager _withdrawalManager;

        public WithdrawalController(IWithdrawalManager withdrawManager)
        {
            _withdrawalManager = withdrawManager;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateWithdrawalRequestViewModel request)
        {
            _withdrawalManager.Create(request, Guid.Empty);

            return StatusCode(201, ResponseWrapper.Success(HttpStatusCode.Created));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var withdrawalRequest = _withdrawalManager.GetById(id);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, withdrawalRequest));
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchCriteriaViewModel parameters, int page = 1, int pageSize = 25)
        {
            var searchResult = _withdrawalManager.Search(parameters, 1, 25);

            if (searchResult is null)
                return StatusCode(204);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK, searchResult));
        }

        [HttpPut("status")]
        public IActionResult UpdateStatus([FromBody] UpdateWithdrawalStatusViewModel request)
        {
            _withdrawalManager.UpdateWithdrawalStatus(request, Guid.Empty);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }

        [HttpPut("{id}/cancel")]
        public IActionResult CancelApprovedStatus(Guid id)
        {
            var request = new UpdateWithdrawalStatusViewModel
            {
                Ids = new[] { id },
                Status = WithdrawalStatus.CANCELED
            };

            _withdrawalManager.UpdateWithdrawalStatus(request, Guid.Empty);

            return StatusCode(200, ResponseWrapper.Success(HttpStatusCode.OK));
        }
    }
}

