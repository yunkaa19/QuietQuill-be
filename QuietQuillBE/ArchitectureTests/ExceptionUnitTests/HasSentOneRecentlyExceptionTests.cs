using Domain.Exceptions.Base;
using Domain.Exceptions.PaperPlane;

namespace ArchitectureTests.ExceptionUnitTests
{
    public class HasSentOneRecentlyExceptionTests
    {
        [Fact]
        public void HasSentOneRecentlyException_ShouldInitializeMessage()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var expectedMessage = $"The user with id {userId} has sent a paperplane in the last 10 minutes.";

            // Act
            var exception = new HasSentOneRecentlyException(userId);

            // Assert
            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void HasSentOneRecentlyException_ShouldInheritFromBadRequestException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var exception = new HasSentOneRecentlyException(userId);

            // Assert
            Assert.IsAssignableFrom<BadRequestException>(exception);
        }
    }
}