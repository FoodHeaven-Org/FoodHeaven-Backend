using System.Data;
using FluentValidation;
using food_heaven_backend.PlanComidas.Application.Internal.OutboundServices.Acl;
using food_heaven_backend.PlanComidas.Domain.Model.Commands;
using food_heaven_backend.PlanComidas.Domain.Model.Entities;
using food_heaven_backend.PlanComidas.Domain.Repositories;
using food_heaven_backend.PlanComidas.Domain.Services;
using food_heaven_backend.Shared.Domain.Repositories;

namespace food_heaven_backend.PlanComidas.Application.Internal.CommandServices;

public class PlanComidaCommandServiceImpl(
    IPlanComidaRepository repository,
    IUnitOfWork unitOfWork,
    IValidator<CreatePlanComidaCommand> validator,
    ExternalFoodCatalogService externalFoodCatalogService,
    ExternalUserSubscriptionService externalUserSubscriptionService)
    : IPlanComidaCommandService
{
    private readonly IPlanComidaRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IValidator<CreatePlanComidaCommand> _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    private readonly ExternalFoodCatalogService _externalFoodCatalogService = externalFoodCatalogService ?? throw new ArgumentNullException(nameof(externalFoodCatalogService));
    private readonly ExternalUserSubscriptionService _externalUserSubscriptionService = externalUserSubscriptionService ?? throw new ArgumentNullException(nameof(externalUserSubscriptionService));

    public async Task<PlanComida> Handle(CreatePlanComidaCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var result = await _validator.ValidateAsync(command);
        if (!result.IsValid)
            throw new ValidationException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));

        await EnsurePlanIsSchedulableAsync(command.IdUsuario, command.FechaInicio, command.FechaFin, command.ListaComidas);

        var entity = new PlanComida(command.IdUsuario, command.FechaInicio, command.FechaFin, command.ListaComidas);

        await _repository.AddAsync(entity);
        await _unitOfWork.CompleteAsync();

        return entity;
    }

    public async Task<bool> Handle(UpdatePlanComidaCommand command, int id)
    {
        ArgumentNullException.ThrowIfNull(command);

        var entity = await _repository.FindByIdAsync(id);
        if (entity == null) throw new DataException("Plan not found.");

        await EnsurePlanIsSchedulableAsync(command.IdUsuario, command.FechaInicio, command.FechaFin, command.ListaComidas, id);

        entity.UpdateDetails(command.IdUsuario, command.FechaInicio, command.FechaFin, command.ListaComidas);

        _repository.Update(entity);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    public async Task<bool> Handle(DeletePlanComidaCommand command, int userId)
    {
        ArgumentNullException.ThrowIfNull(command);
        var entity = await _repository.FindByIdAsync(command.Id);
        if (entity == null || entity.IdUsuario != userId) return false;

        _repository.Remove(entity);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    private async Task EnsurePlanIsSchedulableAsync(
        int userId,
        DateTime startDate,
        DateTime endDate,
        int[] mealIds,
        int? currentPlanId = null)
    {
        await _externalFoodCatalogService.EnsureMealsExistAsync(mealIds);
        await _externalUserSubscriptionService.EnsurePlanFitsSubscriptionAsync(userId, mealIds);

        if (await _repository.ExistsOverlappingPlanForUserAsync(userId, startDate, endDate, currentPlanId))
        {
            throw new InvalidOperationException("The user already has a meal plan that overlaps this date range.");
        }
    }
}
