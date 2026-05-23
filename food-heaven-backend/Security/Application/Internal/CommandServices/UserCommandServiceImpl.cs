using food_heaven_backend.Security.Domain.Model.Commands;
using food_heaven_backend.Security.Domain.Model.Entities;
using food_heaven_backend.Security.Domain.Model.ValueObjects;
using food_heaven_backend.Security.Domain.Model.Exceptions;
using food_heaven_backend.Security.Domain.Repositories;
using food_heaven_backend.Security.Domain.Services;
using food_heaven_backend.Shared.Domain.Repositories;
using food_heaven_backend.PlanComidas.Domain.Repositories;
using System.Text.Json;

namespace food_heaven_backend.Security.Application.Internal.CommandServices;

public class UserCommandServiceImpl : IUserCommandService
{
    private const int DaysInWeek = 7;
    private const int MealTypesPerDay = 3;
    private const int WeeklyMealSlots = DaysInWeek * MealTypesPerDay;

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
            DeliveryAddressesJson = SerializeDeliveryAddresses(command.DeliveryAddresses),
            PaymentMethod = command.PaymentMethod,
            PaymentCardBrand = command.PaymentCard.Brand,
            PaymentCardLastFour = command.PaymentCard.LastFour,
            PaymentCardExpiration = command.PaymentCard.Expiration
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
        user.DeliveryAddressesJson = SerializeDeliveryAddresses(command.DeliveryAddresses);

        if (command.PaymentCard != null)
        {
            user.PaymentMethod = command.PaymentMethod;
            user.PaymentCardBrand = command.PaymentCard.Brand;
            user.PaymentCardLastFour = command.PaymentCard.LastFour;
            user.PaymentCardExpiration = command.PaymentCard.Expiration;
        }

        _userRepository.Update(user);
        await _unitOfWork.CompleteAsync();

        return user;
    }

    public async Task<User> Handle(ChangeUserSubscriptionCommand command, int userId)
    {
        var user = await GetExistingUserAsync(userId);
        var subscriptionPlan = UserSubscriptionPlan.FromCode(command.Subscription);

        user.Subscription = subscriptionPlan.Code;

        await TrimExistingMealPlansToSubscriptionAsync(userId, subscriptionPlan.MealsPerDay);

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

    private async Task TrimExistingMealPlansToSubscriptionAsync(int userId, int mealsPerDayLimit)
    {
        var mealPlans = await _planComidaRepository.FindPlanComidasByUserIdAsync(userId);

        foreach (var mealPlan in mealPlans)
        {
            var trimmedMealSlots = TrimMealSlotsForSubscription(
                mealPlan.ListaComidas,
                mealsPerDayLimit);

            if (mealPlan.ListaComidas.SequenceEqual(trimmedMealSlots)) continue;

            mealPlan.UpdateDetails(
                mealPlan.IdUsuario,
                mealPlan.FechaInicio,
                mealPlan.FechaFin,
                trimmedMealSlots,
                mealPlan.HorariosEntrega);

            _planComidaRepository.Update(mealPlan);
        }
    }

    private static int[] TrimMealSlotsForSubscription(int[] mealSlots, int mealsPerDayLimit)
    {
        if (mealSlots.Length != WeeklyMealSlots) return mealSlots;

        var trimmedMealSlots = mealSlots.ToArray();

        for (var dayIndex = 0; dayIndex < DaysInWeek; dayIndex++)
        {
            var selectedMealsForDay = 0;

            for (var mealTypeIndex = 0; mealTypeIndex < MealTypesPerDay; mealTypeIndex++)
            {
                var slotIndex = mealTypeIndex * DaysInWeek + dayIndex;
                if (trimmedMealSlots[slotIndex] <= 0) continue;

                selectedMealsForDay++;
                if (selectedMealsForDay > mealsPerDayLimit)
                {
                    trimmedMealSlots[slotIndex] = 0;
                }
            }
        }

        return trimmedMealSlots;
    }

    private static string SerializeDeliveryAddresses(IReadOnlyCollection<DeliveryAddress> deliveryAddresses)
    {
        return JsonSerializer.Serialize(deliveryAddresses, new JsonSerializerOptions(JsonSerializerDefaults.Web));
    }
}
