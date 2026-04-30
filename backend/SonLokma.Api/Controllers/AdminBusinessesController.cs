using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SonLokma.Application.Handlers.Businesses.Commands.ApproveBusiness;
using SonLokma.Application.Handlers.Businesses.Commands.RejectBusiness;
using SonLokma.Application.Handlers.Businesses.Common;
using SonLokma.Application.Handlers.Businesses.Queries.GetPendingBusinesses;

namespace SonLokma.Api.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin/businesses")]
public sealed class AdminBusinessesController(
    GetPendingBusinessesQueryHandler pendingHandler,
    ApproveBusinessCommandHandler approveHandler,
    RejectBusinessCommandHandler rejectHandler) : ControllerBase
{
    [HttpGet("pending")]
    public async Task<ActionResult<IReadOnlyList<BusinessResponse>>> GetPending(CancellationToken cancellationToken)
    {
        var businesses = await pendingHandler.Handle(new GetPendingBusinessesQuery(), cancellationToken);
        return Ok(businesses);
    }

    [HttpPost("{businessId:guid}/approve")]
    public async Task<ActionResult<BusinessResponse>> Approve(Guid businessId, CancellationToken cancellationToken)
    {
        try
        {
            var business = await approveHandler.Handle(new ApproveBusinessCommand(businessId), cancellationToken);
            return Ok(business);
        }
        catch (InvalidOperationException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }

    [HttpPost("{businessId:guid}/reject")]
    public async Task<ActionResult<BusinessResponse>> Reject(Guid businessId, CancellationToken cancellationToken)
    {
        try
        {
            var business = await rejectHandler.Handle(new RejectBusinessCommand(businessId), cancellationToken);
            return Ok(business);
        }
        catch (InvalidOperationException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }
}
