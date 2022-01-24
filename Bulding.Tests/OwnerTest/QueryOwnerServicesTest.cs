using AutoMapper;
using Building.OwnersAPI.Core.Application;
using Building.OwnersAPI.Core.Context;
using Building.OwnersAPI.Core.DTO;
using Building.OwnersAPI.Core.Entities;
using Building.OwnersAPI.Repository;
using Bulding.Tests.Mocks;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using static Building.OwnersAPI.Core.Application.QueryOwner;
namespace Bulding.Tests.OwnerTest
{
    public class QueryOwnerServicesTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitofWork> _unitofWork;
        private readonly Mock<ILogger<HandlerOwner>> _logger;

        public QueryOwnerServicesTest()
        {
            _logger = new Mock<ILogger<HandlerOwner>>();
            _unitofWork = MockUnitofWork.GetUnitofWorkOwner();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public async Task Get_Owners_List_Test()
        {
            //Arrange
            var handler = new QueryOwner.HandlerOwner(_mapper, _logger.Object, _unitofWork.Object);
            //Act
            var result = await handler.Handle(new GetOwner(new RequestParameters()), System.Threading.CancellationToken.None);
            //Assert
            Assert.NotNull(result);
            result.ShouldBeOfType<List<OwnerDTO>>();
            result.Count.ShouldBe(31);
        }
    }
}
 