using System.ComponentModel.DataAnnotations.Schema;

namespace food_heaven_backend.Shared.Domain.Model.Entities;

public class BaseEntity
{
    [NotMapped] // evitar duplicar con el override en entidades como Comida
    public virtual int Id { get; set; }
    
}