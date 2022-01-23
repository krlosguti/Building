using AutoMapper;
using Building.OwnersAPI.Core.Application;
using Building.OwnersAPI.Core.Context;
using Building.OwnersAPI.Core.Entities;
using Building.OwnersAPI.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MediatR;
using Bulding.Tests.Mocks;
using Building.OwnersAPI.Repository;
using AutoFixture;
using static Building.OwnersAPI.Core.Application.QueryOwner;
using Building.OwnersAPI.Core.DTO;

namespace Bulding.Tests.OwnerTest
{
    public class OwnerServicesTest
    {
        [Fact]
        public async void QueryOwner_Is_Not_Null()
        {
            //Arrange.
            var appContextMock = new Mock<OwnerContext>();
            var mapperMock = new Mock<IMapper>();
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<HandlerOwner>>();
            var unitofworkMock = new Mock<IUnitofWork>();

            var commandHandler = new HandlerOwner(mapperMock.Object, loggerMock.Object, unitofworkMock.Object);
            
            var _mockrepo = MockOwnerRepository.GetOwners();
            unitofworkMock.Setup(x => x.Owners).Returns(() =>
            {
                return _mockrepo.Object;
            });

            var ownersDTO = new List<OwnerDTO>
            {
                new OwnerDTO
                {
                    IdOwner =   Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    Name = "Name1",
                    Address = "Address1",
                    Birthday = Convert.ToDateTime("01/05/2005"),
                    Photo = "Photo1.jpeg"
                },
                new OwnerDTO
                {
                    IdOwner =   Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    Name = "Name2",
                    Address = "Address2",
                    Birthday = Convert.ToDateTime("02/05/2005"),
                    Photo = "Photo2.jpeg"
                }
            };
            RequestParameters requestParameters = new RequestParameters();
            var request = new GetOwner(requestParameters);

            mediatorMock.Setup(x => x.Send(request, new System.Threading.CancellationToken()))
                .ReturnsAsync(ownersDTO);

            //Act.
            var result = await mediatorMock.Object.Send(request);
            //var result = await HandlerOwner.Handle(request, new System.Threading.CancellationToken());

            //Assert.
            Assert.NotNull(result);
        }

        [Fact]
        public void QueryOwner_Error_Message()
        {
            //Arrange.
            var appContextMock = new Mock<OwnerContext>();
            var mapperMock = new Mock<IMapper>();
            var mediatorMock = new Mock<IMediator>();
            var loggerMock = new Mock<ILogger<HandlerOwner>>();
            var unitofworkMock = new Mock<IUnitofWork>();

            var commandHandler = new HandlerOwner(mapperMock.Object, loggerMock.Object, unitofworkMock.Object);

            var _mockrepo = MockOwnerRepository.GetOwners();
            unitofworkMock.Setup(x => x.Owners).Returns(() =>
            {
                return _mockrepo.Object;
            });

            RequestParameters requestParameters = new RequestParameters();
            var request = new GetOwner(requestParameters);

            mediatorMock.Setup(x => x.Send(null, new System.Threading.CancellationToken()))
                .ThrowsAsync(new Exception());

            var result =  mediatorMock.Object.Send(request);
            Func<Task> send = () =>  mediatorMock.Object.Send(request);
            //Assert
            Assert.NotNull(result);
            Assert.IsType<Exception>(result);
        }
    }
}
 