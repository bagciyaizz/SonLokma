using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SonLokma.Api.Common;
using SonLokma.Application.Handlers.Businesses.Commands.ApplyForBusiness;
using SonLokma.Application.Handlers.Businesses.Commands.UpdateBusiness;
using SonLokma.Application.Handlers.Businesses.Common;
using SonLokma.Application.Handlers.Businesses.Queries.GetMyBusiness;
using SonLokma.Domain.Enums;

namespace SonLokma.Api.Controllers;

[ApiController]
[Authorize(Roles = "Business,Admin")]
[Route("api/[controller]")]
public sealed class BusinessesController(
    ApplyForBusinessCommandHandler applyHandler,
    UpdateBusinessCommandHandler updateHandler,
    GetMyBusinessQueryHandler getMyBusinessHandler) : ControllerBase
{
    [HttpGet("me")]
    public async Task<ActionResult<BusinessResponse>> GetMine(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        var business = await getMyBusinessHandler.Handle(new GetMyBusinessQuery(userId.Value), cancellationToken);
        return business is null ? NotFound(new { message = "Isletme basvurusu bulunamadi." }) : Ok(business);
    }

    [HttpPost]
    public async Task<ActionResult<BusinessResponse>> Apply(UpsertBusinessRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        try
        {
            var business = await applyHandler.Handle(
                new ApplyForBusinessCommand(
                    userId.Value,
                    request.Name,
                    request.Description,
                    request.Category,
                    request.Address,
                    request.Latitude,
                    request.Longitude),
                cancellationToken);

            return CreatedAtAction(nameof(GetMine), business);
        }
        catch (InvalidOperationException exception)
        {
            return BadRequest(new { message = exception.Message });
        }
    }

    [HttpPut("me")]
    public async Task<ActionResult<BusinessResponse>> UpdateMine(UpsertBusinessRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        if (userId is null)
        {
            return Unauthorized();
        }

        try
        {
            var business = await updateHandler.Handle(
                new UpdateBusinessCommand(
                    userId.Value,
                    request.Name,
                    request.Description,
                    request.Category,
                    request.Address,
                    request.Latitude,
                    request.Longitude),
                cancellationToken);

            return Ok(business);
        }
        catch (InvalidOperationException exception)
        {
            return NotFound(new { message = exception.Message });
        }
    }
}

public sealed record UpsertBusinessRequest(
    string Name,
    string Description,
    BusinessCategory Category,
    string Address,
    double Latitude,
    double Longitude);
