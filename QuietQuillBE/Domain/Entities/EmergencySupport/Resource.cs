namespace Domain.Entities.MentalHealthSupport;

public class Resource
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string ContactInformation { get; private set; }
    public ResourceType Type { get; private set; } // Could be an enum representing the type of resource
    public bool IsAvailable { get; private set; } // To indicate if the resource is currently available

    // A private constructor for EF
    private Resource()
    {
        // Used by EF Core
    }

    // Public constructor for creating a new Resource
    public Resource(string name, string description, string contactInformation, ResourceType type, bool isAvailable = true)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        ContactInformation = contactInformation ?? throw new ArgumentNullException(nameof(contactInformation));
        Type = type;
        IsAvailable = isAvailable;
    }

    // Additional domain logic could include methods for updating the resource details,
    // marking it as available/unavailable, etc.
        
    public void UpdateDetails(string newName, string newDescription, string newContactInformation)
    {
        Name = newName ?? throw new ArgumentNullException(nameof(newName));
        Description = newDescription;
        ContactInformation = newContactInformation ?? throw new ArgumentNullException(nameof(newContactInformation));
    }

    public void SetAvailability(bool availability)
    {
        IsAvailable = availability;
    }

    // ... other methods as needed
}