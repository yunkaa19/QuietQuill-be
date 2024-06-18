using System;
using System.Reflection;
using Xunit;
using Domain.Entities.PaperPlane;

namespace ArchitectureTests.EntitiesUnitTests
{
    public class PaperPlaneEntityTests
    {
        public PaperPlaneEntityTests()
        {
            ResetIdSeed(1);
        }

        private void ResetIdSeed(int value)
        {
            
            typeof(PaperPlaneEntity)
                .GetField("id_seed", BindingFlags.Static | BindingFlags.NonPublic)
                .SetValue(null, value);
        }

        [Fact]
        public void CreatePaperPlaneEntity_ShouldInitializeProperties()
        {
            // Arrange
            var content = "This is a paper plane content.";
            var userId = Guid.NewGuid();

            // Act
            var paperPlane = new PaperPlaneEntity(content, userId);

            // Assert
            Assert.Equal(1, paperPlane.id); // Since this is the first instance created in the test scope
            Assert.Equal(content, paperPlane.content);
            Assert.Equal(DateTime.Now.Date, paperPlane.date.Date); // Comparing only dates to avoid time mismatch
            Assert.Equal(userId, paperPlane.userId);
        }

        [Fact]
        public void CreateMultiplePaperPlaneEntities_ShouldIncrementId()
        {
            // Arrange
            var content1 = "Content for first paper plane.";
            var content2 = "Content for second paper plane.";
            var userId = Guid.NewGuid();

            // Act
            var paperPlane1 = new PaperPlaneEntity(content1, userId);
            var paperPlane2 = new PaperPlaneEntity(content2, userId);

            // Assert
            Assert.Equal(1, paperPlane1.id);
            Assert.Equal(2, paperPlane2.id);
        }
    }
}