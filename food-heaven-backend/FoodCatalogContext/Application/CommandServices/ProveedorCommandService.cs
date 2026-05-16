using System.Data;
using FluentValidation;
using food_heaven_backend.FoodCatalogContext.Domain;
using food_heaven_backend.FoodCatalogContext.Domain.Models.Commands;
using food_heaven_backend.FoodCatalogContext.Domain.Models.Entities;
using food_heaven_backend.FoodCatalogContext.Domain.Models.Exceptions;
using food_heaven_backend.FoodCatalogContext.Domain.Services;
using food_heaven_backend.Shared.Domain;
using System;
using System.Threading.Tasks;
using food_heaven_backend.Shared.Domain.Repositories;

namespace food_heaven_backend.FoodCatalogContext.Application.CommandServices
{
    public class ProveedorCommandService(
        IProveedorRepository proveedorRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreateProveedorCommand> validator)
        : IProveedorCommandService
    {
        private readonly IProveedorRepository _proveedorRepository = proveedorRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IValidator<CreateProveedorCommand> _validator = validator;

        public async Task<Proveedor> Handle(CreateProveedorCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            var validationResult = await _validator.ValidateAsync(command);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errors);
            }

            var proveedor = new Proveedor(command.Nombre, command.Distrito, command.Contacto, command.TipoProveedorId);
            await _proveedorRepository.AddAsync(proveedor);
            await _unitOfWork.CompleteAsync();

            return proveedor;
        }

        public async Task<bool> Handle(DeleteProveedorCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            var proveedor = await _proveedorRepository.FindByIdAsync(command.Id);
            if (proveedor == null) return false;

            _proveedorRepository.Remove(proveedor);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> Handle(UpdateProveedorCommand command, int id)
        {
            var proveedor = await _proveedorRepository.FindByIdAsync(id);
            if (proveedor == null)
                throw new ProveedorNotFoundException($"Proveedor with ID {id} not found");

            proveedor.Nombre = command.Nombre;
            proveedor.Distrito = command.Distrito;
            proveedor.Contacto = command.Contacto;
            proveedor.TipoProveedorId = command.TipoProveedorId;

            _proveedorRepository.Update(proveedor);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
