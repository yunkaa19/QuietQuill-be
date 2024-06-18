using Domain.Exceptions.Base;
using Domain.Exceptions.Meditation;

namespace ArchitectureTests.ExceptionUnitTests
{
    public class MeditationNotFoundExceptionTests
    {
        [Fact]
        public void MeditationNotFoundException_ShouldInitializeMessage()
        {
            // Arrange
            var id = "12345";
            var expectedMessage = $"The session with the identifier {id} was not found.";

            // Act
            var exception = new MeditationNotFoundException(id);

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void MeditationNotFoundException_ShouldInheritFromNotFoundException()
        {
            // Arrange
            var id = "12345";

            // Act
            var exception = new MeditationNotFoundException(id);

            // Assert
            Assert.IsAssignableFrom<NotFoundException>(exception);
        }
    }
}