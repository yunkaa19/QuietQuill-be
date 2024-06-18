using Domain.Exceptions.Base;
using Domain.Exceptions.Users;

namespace ArchitectureTests.ExceptionUnitTests
{
    public class UserNotFoundExceptionTests
    {
        [Fact]
        public void UserNotFoundException_ShouldInitializeMessage()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var expectedMessage = $"The user with the identifier {userId} was not found.";

            // Act
            var exception = new UserNotFoundException(userId);

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void UserNotFoundException_ShouldInheritFromNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var exception = new UserNotFoundException(userId);

            // Assert
            Assert.IsAssignableFrom<NotFoundException>(exception);
        }
    }
}