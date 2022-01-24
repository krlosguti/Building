using AutoMapper;
using Building.OwnersAPI.Core.Application;
using Building.OwnersAPI.Repository;
using Bulding.Tests.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Building.OwnersAPI.Core.Application.UpdateOwner;

namespace Bulding.Tests.OwnerTest
{
    public class UpdateOwnerServiceTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitofWork> _unitofWork;
        private readonly Mock<ILogger<HandlerOwner>> _logger;
        private readonly Mock<IWebHostEnvironment> _hostingEnvironment;

        public UpdateOwnerServiceTest()
        {
            _logger = new Mock<ILogger<HandlerOwner>>();
            _unitofWork = MockUnitofWork.GetUnitofWorkOwner();
            var mapperConfig = new MapperConfiguration(c =>
            {
                c.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();
            _hostingEnvironment = new Mock<IWebHostEnvironment>();
            _hostingEnvironment.Setup(m => m.EnvironmentName)
                                    .Returns("Hosting:UnitTestEnvironment");
        }

        [Fact]
        public async Task Add_Owner_Test()
        {
            //Arrange
            var fileMock = new Mock<IFormFile>();
            var fileName = "test1.pdf";
            fileMock.Setup(_ => _.FileName).Returns(fileName);

            var request = new ExecuteOwner
            {
                IdOwner = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Address = "Address Test 1",
                Birthday = Convert.ToDateTime("2001-05-05"),
                Name = "TestNameChanged 1",
                PhotoFile = fileMock.Object
            };

            var handler = new UpdateOwner.HandlerOwner(_logger.Object, _unitofWork.Object, _hostingEnvironment.Object);
            //Act
            var result = await handler.Handle(request, System.Threading.CancellationToken.None);

            //Assert
            Assert.Equal("TestNameChanged 1", _unitofWork.Object.Owners.Get(Guid.Parse("00000000-0000-0000-0000-000000000001")).Result.Name);

        }
    }
}
