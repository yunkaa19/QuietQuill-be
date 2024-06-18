using Domain.Exceptions.Base;

namespace ArchitectureTests.ExceptionUnitTests
{
    public class TestBadRequestException : BadRequestException
    {
        public TestBadRequestException(string message) : base(message)
        {
        }
    }

    public class BadRequestExceptionTests
    {
        [Fact]
        public void BadRequestException_ShouldInitializeMessage()
        {
            // Arrange
            var message = "This is a bad request exception";

            // Act
            var exception = new TestBadRequestException(message);

            // Assert
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public void BadRequestException_ShouldInheritFromException()
        {
            // Arrange
            var message = "This is a bad request exception";

            // Act
            var exception = new TestBadRequestException(message);

            // Assert
            Assert.IsAssignableFrom<Exception>(exception);
        }
    }
}