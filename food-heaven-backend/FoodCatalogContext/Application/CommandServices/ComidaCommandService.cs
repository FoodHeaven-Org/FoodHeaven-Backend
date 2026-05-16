using System.Data;
using FluentValidation;
using food_heaven_backend.FoodCatalogContext.Domain.Models.Commands;
using food_heaven_backend.FoodCatalogContext.Domain.Services;
using food_heaven_backend.Shared.Domain.Repositories;
using food_heaven_backend.FoodCatalogContext.Domain.Models.Entities;

namespace food_heaven_backend.FoodCatalogContext.Application.CommandServices;

public class ComidaCommandService(
    IComidaRepository comidaRepository,
    IUnitOfWork unitOfWork,
    IValidator<CreateComidaCommand> validator)
    : IComidaCommandService
{
    private readonly IComidaRepository _comidaRepository = comidaRepository ?? throw new ArgumentNullException(nameof(comidaRepository));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    private readonly IValidator<CreateComidaCommand> _validator = validator ?? throw new ArgumentNullException(nameof(validator));

    public async Task<Comida> Handle(CreateComidaCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var validationResult = await _validator.ValidateAsync(command);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ValidationException(string.Join(", ", errors));
        }

        var comida = new Comida(
            nombre: command.Nombre,
            complemento: command.Complemento,
            url: command.Url,
            cal: command.Calorias,
            prote: command.Proteina,
            carbo: command.Carbohidrato,
            grasa: command.Grasa,
            idProveedor: command.Id_proveedor,
            idTipoComida: command.id_tipo_comida,
            es_especial: command.es_especial ? 1 : 0
        );

        try
        {
            await _comidaRepository.AddAsync(comida);
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"[ERROR AL GUARDAR COMIDA] {ex.Message} | INNER: {ex.InnerException?.Message}");
        }

        return comida;
    }


    public async Task<bool> Handle(DeleteComidaCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var comida = await _comidaRepository.FindByIdAsync(command.Id);
        if (comida is null) return false;

        _comidaRepository.Remove(comida);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    public async Task<bool> Handle(UpdateComidaCommand command, int id)
    {
        var comida = await _comidaRepository.FindByIdAsync(id);
        if (comida == null)
            throw new DataException("Comida no encontrada.");

        comida.Nombre = command.Nombre;
        comida.Complemento = command.Complemento;
        comida.Url = command.Url;
        comida.Cal = command.Calorias;
        comida.Prote = command.Proteina;
        comida.Carbo = command.Carbohidrato;
        comida.Grasa = command.Grasa;
        comida.id_tipo_comida = command.id_tipo_comida;
        comida.Id_Proveedor = command.Id_proveedor;
        comida.es_especial = command.es_especial ? 1 : 0;

        _comidaRepository.Update(comida);
        await _unitOfWork.CompleteAsync();

        return true;
    }
}