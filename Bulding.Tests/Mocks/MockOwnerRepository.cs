using AutoFixture;
using Building.OwnersAPI.Core.Context;
using Building.OwnersAPI.Core.Entities;
using Building.OwnersAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Linq;

namespace Bulding.Tests.Mocks
{
    public static class MockOwnerRepository
    {
        public static void AddOwnersRepository(OwnerContext ownerContextFake)
        {

            var fixture = new Fixture();
            var owners = fixture.CreateMany<Owner>(30).ToList();

            owners.Add(fixture.Build<Owner>()
                .With(tr => tr.IdOwner, Guid.Parse("00000000-0000-0000-0000-000000000001"))
                .Create()
            );

            ownerContextFake.Owners!.AddRange(owners);
            ownerContextFake.SaveChanges();
        }

        public static OwnerRepository GetOwnerRepository(OwnerContext ownerContextFake)
        {
            ownerContextFake.Database.EnsureCreated();
            AddOwnersRepository(ownerContextFake);

            return new OwnerRepository(ownerContextFake);
        }
    }
}
