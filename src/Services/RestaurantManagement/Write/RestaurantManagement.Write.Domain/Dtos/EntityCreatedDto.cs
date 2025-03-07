namespace RestaurantManagement.Write.Domain.Dtos;

public record EntityCreatedDto(Guid Id)
{
    public static EntityCreatedDto From(Guid id) => new EntityCreatedDto(id);
}
