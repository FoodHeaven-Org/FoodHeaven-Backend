using food_heaven_backend.Security.Domain.Entities;
using food_heaven_backend.Security.Domain.Queries;
using food_heaven_backend.Security.Domain.Repositories;
using food_heaven_backend.Security.Domain.Service;

namespace food_heaven_backend.Security.Application;

public class UserQueryService : IUserQueryService
{
    private readonly IUserRepository _userRepository;

    // Constructor: Recibe el repositorio de usuarios que se encargará de acceder a los datos
    public UserQueryService(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
    }

    // Método que maneja la consulta para obtener un usuario por su ID
    public async Task<User> Handle(GetUserByIdQuery query)
    {
        if (query == null) throw new ArgumentNullException(nameof(query));

        // Buscamos el usuario en el repositorio por el ID proporcionado
        var user = await _userRepository.FindByIdAsync(query.UserId);

        // Si no se encuentra el usuario, lanzamos una excepción o podemos devolver null
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {query.UserId} not found.");
        }

        return user;
    }
}