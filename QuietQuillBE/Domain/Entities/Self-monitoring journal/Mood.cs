namespace Domain.Entities;

public class Mood
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    // Could include properties for associated colors, icons, etc.

    private Mood() { }

    public Mood(string name, string description)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
    }
}