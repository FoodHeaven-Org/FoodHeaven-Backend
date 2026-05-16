using food_heaven_backend.Security.Domain.Comands;
using food_heaven_backend.Security.Domain.Entities;
using food_heaven_backend.Security.Domain.Exceptions;
using food_heaven_backend.Security.Domain.Repositories;
using food_heaven_backend.Security.Domain.Service;
using food_heaven_backend.Shared.Domain.Repositories;

namespace food_heaven_backend.Security.Application;

public class UserCommandService : IUserCommandService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashService _hashService;
    private readonly IJwtEncryptService _jwtEncryptService;

    public UserCommandService(IUserRepository userRepository, IUnitOfWork unitOfWork, IHashService hashService, IJwtEncryptService jwtEncryptService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _hashService = hashService;
        _jwtEncryptService = jwtEncryptService;
    }

    public async Task<User> Handle(SignUpCommand command)
    {
        // Verificar si el nombre de usuario ya está registrado
        var existingUser = await _userRepository.GetByUsernamelAsync(command.Username);
        if (existingUser != null)
            throw new UsernameAlreadyTakenException();

        // Crear un nuevo usuario con los datos proporcionados en el comando
        var user = new User
        {
            Username = command.Username,
            PasswordHashed = _hashService.HashPassword(command.Password), // Cifrar la contraseña
            Subscription = command.Subscription,
            Phone = command.Phone,  // Asignar el teléfono
            City = command.City     // Asignar la ciudad
        };

        // Guardar el usuario en la base de datos
        await _userRepository.AddAsync(user);
        await _unitOfWork.CompleteAsync();  // Completar la unidad de trabajo

        return user;
    }

    public async Task<string> Handle(LoginCommand command)
    {
        // Verificar si el usuario existe en la base de datos
        var user = await _userRepository.GetByUsernamelAsync(command.Username);
        if (user == null || !_hashService.VerifyPassword(command.Password, user.PasswordHashed))  // Verificar la contraseña
            throw new InvalidCredentialsException();

        // Generar el token JWT si las credenciales son válidas
        var jwtToken = _jwtEncryptService.Encrypt(user);

        return jwtToken;
    }
}
