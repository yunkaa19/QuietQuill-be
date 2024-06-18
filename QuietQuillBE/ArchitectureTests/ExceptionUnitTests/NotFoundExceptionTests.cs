using Domain.Exceptions.Base;

namespace ArchitectureTests.ExceptionUnitTests
{
    public class TestNotFoundException : NotFoundException
    {
        public TestNotFoundException(string message) : base(message)
        {
        }
    }

    public class NotFoundExceptionTests
    {
        [Fact]
        public void NotFoundException_ShouldInitializeMessage()
        {
            // Arrange
            var message = "This is a not found exception";

            // Act
            var exception = new TestNotFoundException(message);

            // Assert
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public void NotFoundException_ShouldInheritFromException()
        {
            // Arrange
            var message = "This is a not found exception";

            // Act
            var exception = new TestNotFoundException(message);

            // Assert
            Assert.IsAssignableFrom<Exception>(exception);
        }
    }
}