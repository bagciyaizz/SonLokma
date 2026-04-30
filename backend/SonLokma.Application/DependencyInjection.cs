using Microsoft.Extensions.DependencyInjection;
using SonLokma.Application.Handlers.Businesses.Commands.ApplyForBusiness;
using SonLokma.Application.Handlers.Businesses.Commands.ApproveBusiness;
using SonLokma.Application.Handlers.Businesses.Commands.RejectBusiness;
using SonLokma.Application.Handlers.Businesses.Commands.UpdateBusiness;
using SonLokma.Application.Handlers.Businesses.Queries.GetMyBusiness;
using SonLokma.Application.Handlers.Businesses.Queries.GetPendingBusinesses;

namespace SonLokma.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ApplyForBusinessCommandHandler>();
        services.AddScoped<UpdateBusinessCommandHandler>();
        services.AddScoped<ApproveBusinessCommandHandler>();
        services.AddScoped<RejectBusinessCommandHandler>();
        services.AddScoped<GetMyBusinessQueryHandler>();
        services.AddScoped<GetPendingBusinessesQueryHandler>();

        return services;
    }
}
