using food_heaven_backend.Security.Domain.Comands;
using food_heaven_backend.Security.Domain.Entities;

namespace food_heaven_backend.Security.Domain.Service;

public interface IUserCommandService
{
    Task<User> Handle(SignUpCommand command);
    Task<string> Handle(LoginCommand loginCommand);
}