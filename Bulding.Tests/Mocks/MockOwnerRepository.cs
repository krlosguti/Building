using Building.OwnersAPI.Core.Entities;
using Building.OwnersAPI.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulding.Tests.Mocks
{
    public static class MockOwnerRepository
    {
        public static Mock<IOwnerRepository> GetOwners()
        {
            var owners = new List<Owner>
            {
                new Owner
                {
                    IdOwner =   Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Name = "Name1",
                    Address = "Address1",
                    Birthday = Convert.ToDateTime("01/05/2005"),
                    Photo = "Photo1.jpeg"
                },
                new Owner
                {
                    IdOwner =   Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Name = "Name2",
                    Address = "Address2",
                    Birthday = Convert.ToDateTime("02/05/2005"),
                    Photo = "Photo2.jpeg"
                },
                new Owner
                {
                    IdOwner =   Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    Name = "Name3",
                    Address = "Address3",
                    Birthday = Convert.ToDateTime("03/05/2005"),
                    Photo = "Photo3.jpeg"
                },
                new Owner
                {
                    IdOwner =   Guid.Parse("00000000-0000-0000-0000-000000000004"),
                    Name = "Name4",
                    Address = "Address4",
                    Birthday = Convert.ToDateTime("04/05/2005"),
                    Photo = "Photo4.jpeg"
                },
                new Owner
                {
                    IdOwner =   Guid.Parse("00000000-0000-0000-0000-000000000005"),
                    Name = "Name5",
                    Address = "Address5",
                    Birthday = Convert.ToDateTime("05/05/2005"),
                    Photo = "Photo5.jpeg"
                }
            };

            var mockRepo = new Mock<IOwnerRepository>();
            mockRepo.Setup(x => x.GetAll(new RequestParameters())).ReturnsAsync(owners);

            mockRepo.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync((Guid id) =>
            {
                var owner = owners.Find(x => x.IdOwner == id);
                return owner;
            });

            mockRepo.Setup(x => x.Insert(It.IsAny<Owner>())).Returns((Owner owner) =>
            {
                owners.Add(owner);
                return System.Threading.Tasks.Task.CompletedTask;
            });

            return mockRepo;
        }
    }
}
