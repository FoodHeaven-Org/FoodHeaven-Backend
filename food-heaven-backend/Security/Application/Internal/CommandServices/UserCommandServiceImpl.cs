using food_heaven_backend.Security.Domain.Model.Commands;
using food_heaven_backend.Security.Domain.Model.Entities;
using food_heaven_backend.Security.Domain.Model.Exceptions;
using food_heaven_backend.Security.Domain.Repositories;
using food_heaven_backend.Security.Domain.Services;
using food_heaven_backend.Shared.Domain.Repositories;

namespace food_heaven_backend.Security.Application.Internal.CommandServices;

public class UserCommandServiceImpl : IUserCommandService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashService _hashService;
    private readonly IJwtEncryptService _jwtEncryptService;

    public UserCommandServiceImpl(IUserRepository userRepository, IUnitOfWork unitOfWork, IHashService hashService, IJwtEncryptService jwtEncryptService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _hashService = hashService;
        _jwtEncryptService = jwtEncryptService;
    }

    public async Task<User> Handle(SignUpCommand command)
    {
        var existingUser = await _userRepository.GetByUsernameAsync(command.Username);
        if (existingUser != null)
            throw new UsernameAlreadyTakenException();

        var user = new User
        {
            FullName = command.FullName,
            Username = command.Username,
            PasswordHashed = _hashService.HashPassword(command.Password),
            Subscription = command.Subscription,
            Phone = command.Phone,
            City = command.City
        };

        await _userRepository.AddAsync(user);
        await _unitOfWork.CompleteAsync();

        return user;
    }

    public async Task<string> Handle(LoginCommand command)
    {
        var user = await _userRepository.GetByUsernameAsync(command.Username);
        if (user == null || !_hashService.VerifyPassword(command.Password, user.PasswordHashed))
            throw new InvalidCredentialsException();

        return _jwtEncryptService.Encrypt(user);
    }
}
