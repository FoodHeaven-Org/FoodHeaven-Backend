using food_heaven_backend.Security.Domain.Model.Commands;
using food_heaven_backend.Security.Domain.Model.Entities;

namespace food_heaven_backend.Security.Domain.Services;

public interface IUserCommandService
{
    Task<User> Handle(SignUpCommand command);
    Task<string> Handle(LoginCommand loginCommand);
}