using System.Data;
using FluentValidation;
using food_heaven_backend.PlanComidas.Domain.Models.Commands;
using food_heaven_backend.PlanComidas.Domain.Models.Entities;
using food_heaven_backend.PlanComidas.Domain.Services;
using food_heaven_backend.Shared.Domain.Repositories;

namespace food_heaven_backend.PlanComidas.Application.CommandServices;

public class PlanComidaCommandService(
    IPlanComidaRepository repository,
    IUnitOfWork unitOfWork,
    IValidator<CreatePlanComidaCommand> validator)
    : IPlanComidaCommandService
{
    private readonly IPlanComidaRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IValidator<CreatePlanComidaCommand> _validator = validator ?? throw new ArgumentNullException(nameof(validator));

    public async Task<PlanComida> Handle(CreatePlanComidaCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var result = await _validator.ValidateAsync(command);
        if (!result.IsValid)
            throw new ValidationException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));

        var entity = new PlanComida(command.IdUsuario, command.FechaInicio, command.FechaFin, command.ListaComidas);

        await _repository.AddAsync(entity);
        await _unitOfWork.CompleteAsync();

        return entity;
    }

    public async Task<bool> Handle(UpdatePlanComidaCommand command, int id)
    {
        var entity = await _repository.FindByIdAsync(id);
        if (entity == null) throw new DataException("Plan no encontrado.");

        entity.IdUsuario = command.IdUsuario;
        entity.FechaInicio = command.FechaInicio;
        entity.FechaFin = command.FechaFin;
        entity.ListaComidas = command.ListaComidas;

        _repository.Update(entity);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    public async Task<bool> Handle(DeletePlanComidaCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        var entity = await _repository.FindByIdAsync(command.Id);
        if (entity == null) return false;

        _repository.Remove(entity);
        await _unitOfWork.CompleteAsync();

        return true;
    }
}
