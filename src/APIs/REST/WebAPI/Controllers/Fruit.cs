namespace WebAPI.Controllers;

public record Fruit()
{
    public string Name { get; set; } = default!;
    public Guid Id { get; init; } = Guid.NewGuid();

    public Fruit(string name) : this()
    {
        Name = name;
    }
};