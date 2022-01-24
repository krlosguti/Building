using AutoMapper;
using Building.OwnersAPI.Core.Application;
using Building.OwnersAPI.Core.DTO;
using Building.OwnersAPI.Repository;
using Bulding.Tests.Mocks;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Building.OwnersAPI.Core.Application.QueryOwnerById;

namespace Bulding.Tests.OwnerTest
{
    public class QueryOwnerByIdServiceTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitofWork> _unitofWork;
        private readonly Mock<ILogger<HandlerOwner>> _logger;

        public QueryOwnerByIdServiceTest()
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
        public async Task Get_Owner_By_Id_Test()
        {
            //Arrange
            var handler = new QueryOwnerById.HandlerOwner(_mapper, _logger.Object, _unitofWork.Object);
            var request = new GetOwner();
            request.Id = Guid.Parse("00000000-0000-0000-0000-000000000001");
            //Act
            var result = await handler.Handle(request, System.Threading.CancellationToken.None);
            //Assert
            Assert.NotNull(result);
            result.ShouldBeOfType<OwnerDTO>();
            result.IdOwner.ShouldBe(request.Id);
        }
    }
}
