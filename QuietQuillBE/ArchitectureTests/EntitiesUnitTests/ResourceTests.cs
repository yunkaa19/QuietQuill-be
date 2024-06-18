using Domain.Entities.MentalHealthSupport;

namespace ArchitectureTests.EntitiesUnitTests
{
    public class ResourceTests
    {
        [Fact]
        public void CreateResource_ShouldInitializeProperties()
        {
            // Arrange
            var name = "Sample Resource";
            var description = "This is a sample resource description.";
            var contactInformation = "contact@resource.com";
            var type = new ResourceType(); // Assume ResourceType has a parameterless constructor
            var isAvailable = true;

            // Act
            var resource = new Resource(name, description, contactInformation, type, isAvailable);

            // Assert
            Assert.Equal(name, resource.Name);
            Assert.Equal(description, resource.Description);
            Assert.Equal(contactInformation, resource.ContactInformation);
            Assert.Equal(type, resource.Type);
            Assert.Equal(isAvailable, resource.IsAvailable);
            Assert.NotEqual(Guid.Empty, resource.Id);
        }

        [Fact]
        public void CreateResource_ShouldThrowArgumentNullException_WhenNameIsNull()
        {
            // Arrange
            string name = null;
            var description = "This is a sample resource description.";
            var contactInformation = "contact@resource.com";
            var type = new ResourceType();
            var isAvailable = true;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Resource(name, description, contactInformation, type, isAvailable));
        }

        [Fact]
        public void CreateResource_ShouldThrowArgumentNullException_WhenContactInformationIsNull()
        {
            // Arrange
            var name = "Sample Resource";
            var description = "This is a sample resource description.";
            string contactInformation = null;
            var type = new ResourceType();
            var isAvailable = true;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Resource(name, description, contactInformation, type, isAvailable));
        }

        [Fact]
        public void UpdateDetails_ShouldUpdateResourceDetails()
        {
            // Arrange
            var resource = new Resource("Sample Resource", "Initial description", "contact@resource.com", new ResourceType(), true);
            var newName = "Updated Resource Name";
            var newDescription = "Updated resource description.";
            var newContactInformation = "newcontact@resource.com";

            // Act
            resource.UpdateDetails(newName, newDescription, newContactInformation);

            // Assert
            Assert.Equal(newName, resource.Name);
            Assert.Equal(newDescription, resource.Description);
            Assert.Equal(newContactInformation, resource.ContactInformation);
        }

        [Fact]
        public void UpdateDetails_ShouldThrowArgumentNullException_WhenNewNameIsNull()
        {
            // Arrange
            var resource = new Resource("Sample Resource", "Initial description", "contact@resource.com", new ResourceType(), true);
            string newName = null;
            var newDescription = "Updated resource description.";
            var newContactInformation = "newcontact@resource.com";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => resource.UpdateDetails(newName, newDescription, newContactInformation));
        }

        [Fact]
        public void UpdateDetails_ShouldThrowArgumentNullException_WhenNewContactInformationIsNull()
        {
            // Arrange
            var resource = new Resource("Sample Resource", "Initial description", "contact@resource.com", new ResourceType(), true);
            var newName = "Updated Resource Name";
            var newDescription = "Updated resource description.";
            string newContactInformation = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => resource.UpdateDetails(newName, newDescription, newContactInformation));
        }

        [Fact]
        public void SetAvailability_ShouldUpdateAvailability()
        {
            // Arrange
            var resource = new Resource("Sample Resource", "Initial description", "contact@resource.com", new ResourceType(), true);

            // Act
            resource.SetAvailability(false);

            // Assert
            Assert.False(resource.IsAvailable);
        }
    }
}
