using food_heaven_backend.Security.Domain.Model.Commands;
using food_heaven_backend.Security.Domain.Model.Entities;
using food_heaven_backend.Security.Domain.Model.Exceptions;
using food_heaven_backend.Security.Domain.Repositories;
using food_heaven_backend.Security.Domain.Services;
using food_heaven_backend.Shared.Domain.Repositories;
using food_heaven_backend.PlanComidas.Domain.Repositories;

namespace food_heaven_backend.Security.Application.Internal.CommandServices;

public class UserCommandServiceImpl : IUserCommandService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashService _hashService;
    private readonly IJwtEncryptService _jwtEncryptService;
    private readonly IPlanComidaRepository _planComidaRepository;

    public UserCommandServiceImpl(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IHashService hashService,
        IJwtEncryptService jwtEncryptService,
        IPlanComidaRepository planComidaRepository)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _hashService = hashService;
        _jwtEncryptService = jwtEncryptService;
        _planComidaRepository = planComidaRepository;
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
            City = command.City,
            Address = command.Address,
            PaymentMethod = command.PaymentMethod
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

    public async Task<User> Handle(UpdateUserProfileCommand command, int userId)
    {
        var user = await GetExistingUserAsync(userId);
        var existingUser = await _userRepository.GetByUsernameExceptIdAsync(command.Username, userId);
        if (existingUser != null)
            throw new UsernameAlreadyTakenException();

        user.FullName = command.FullName;
        user.Username = command.Username;
        user.Phone = command.Phone;
        user.City = command.City;
        user.Address = command.Address;
        user.PaymentMethod = command.PaymentMethod;

        _userRepository.Update(user);
        await _unitOfWork.CompleteAsync();

        return user;
    }

    public async Task<User> Handle(ChangeUserSubscriptionCommand command, int userId)
    {
        var user = await GetExistingUserAsync(userId);

        user.Subscription = command.Subscription;

        _userRepository.Update(user);
        await _unitOfWork.CompleteAsync();

        return user;
    }

    public async Task Handle(ChangeUserPasswordCommand command, int userId)
    {
        var user = await GetExistingUserAsync(userId);
        if (!_hashService.VerifyPassword(command.CurrentPassword, user.PasswordHashed))
            throw new InvalidCredentialsException();

        user.PasswordHashed = _hashService.HashPassword(command.NewPassword);

        _userRepository.Update(user);
        await _unitOfWork.CompleteAsync();
    }

    public async Task Handle(DeleteUserAccountCommand command)
    {
        var user = await GetExistingUserAsync(command.UserId);

        await _planComidaRepository.RemoveByUserIdAsync(command.UserId);
        _userRepository.Remove(user);

        await _unitOfWork.CompleteAsync();
    }

    private async Task<User> GetExistingUserAsync(int userId)
    {
        var user = await _userRepository.FindByIdAsync(userId);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {userId} not found.");

        return user;
    }
}
