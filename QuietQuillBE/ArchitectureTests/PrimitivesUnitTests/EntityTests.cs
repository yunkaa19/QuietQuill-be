using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Domain.Primitives;

namespace ArchitectureTests.PrimitivesUnitTests
{
    public record TestDomainEvent : DomainEvent
    {
        public string Name { get; init; }

        public TestDomainEvent(Guid id, string name) : base(id)
        {
            Name = name;
        }
    }

    public class TestEntity : Entity
    {
        public void AddDomainEvent(DomainEvent domainEvent)
        {
            Raise(domainEvent);
        }
    }

    public class EntityTests
    {
        [Fact]
        public void Raise_ShouldAddDomainEvent()
        {
            // Arrange
            var entity = new TestEntity();
            var domainEvent = new TestDomainEvent(Guid.NewGuid(), "Test Event");

            // Act
            entity.AddDomainEvent(domainEvent);

            // Assert
            var domainEvents = entity.GetDomainEvents();
            Assert.Contains(domainEvent, domainEvents);
        }

        [Fact]
        public void GetDomainEvents_ShouldReturnAllRaisedEvents()
        {
            // Arrange
            var entity = new TestEntity();
            var domainEvent1 = new TestDomainEvent(Guid.NewGuid(), "Test Event 1");
            var domainEvent2 = new TestDomainEvent(Guid.NewGuid(), "Test Event 2");

            // Act
            entity.AddDomainEvent(domainEvent1);
            entity.AddDomainEvent(domainEvent2);
            var domainEvents = entity.GetDomainEvents();

            // Assert
            Assert.Contains(domainEvent1, domainEvents);
            Assert.Contains(domainEvent2, domainEvents);
            Assert.Equal(2, domainEvents.Count);
        }

        [Fact]
        public void ClearDomainEvents_ShouldRemoveAllEvents()
        {
            // Arrange
            var entity = new TestEntity();
            var domainEvent1 = new TestDomainEvent(Guid.NewGuid(), "Test Event 1");
            var domainEvent2 = new TestDomainEvent(Guid.NewGuid(), "Test Event 2");

            // Act
            entity.AddDomainEvent(domainEvent1);
            entity.AddDomainEvent(domainEvent2);
            entity.ClearDomainEvents();
            var domainEvents = entity.GetDomainEvents();

            // Assert
            Assert.Empty(domainEvents);
        }
    }
}
