
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TELERADIOLOGY.Application.Features.Partitions.CreatePartition;
using TELERADIOLOGY.WebAPI.Abstractions;

namespace TELERADIOLOGY.WebAPI.Controllers
{
    [AllowAnonymous]
    public class PartitionsController : ApiController
    {
        public PartitionsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompanyHospitalPartition([FromBody] CreatePartitionCommand command, CancellationToken cancellationToken) 
        {
            var response = await _mediator.Send(command, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        public async Task<IActionResult> GetByIdCompanyHospitalParttion([FromBody] GetPartitionByIdQuery command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        public async Task<IActionResult> GetAllCompanyHospitalParttion([FromBody] GetAllPartitionsQuery command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCompanyHospitalParttion([FromBody] DeletePartitionCommand command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        public async Task<IActionResult> UpdataCompanyHospitalParttion([FromBody] UpdatePartitionCommand command, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(command, cancellationToken);
            return StatusCode(200, response);
        }
    }

}
