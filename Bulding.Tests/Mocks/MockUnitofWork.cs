using Building.OwnersAPI.Core.Context;
using Building.OwnersAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;

namespace Bulding.Tests.Mocks
{
    public static class MockUnitofWork
    {
        public static Mock<Building.OwnersAPI.Repository.UnitofWork> GetUnitofWorkOwner()
        {
            var options = new DbContextOptionsBuilder<OwnerContext>()
                .UseInMemoryDatabase(databaseName: $"OwnerDB_{Guid.NewGuid()}")
                .Options;

            //Create instance of the context and in memory database
            var ownerContextFake = new OwnerContext(options);
            var mockRepository = MockOwnerRepository.GetOwnerRepository(ownerContextFake);

            var mockUnitofWork = new  Mock<UnitofWork>(ownerContextFake,mockRepository);

            return mockUnitofWork;
        }
    }
}
