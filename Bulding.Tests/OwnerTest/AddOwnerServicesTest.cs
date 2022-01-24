using AutoMapper;
using Building.OwnersAPI.Core.Application;
using Building.OwnersAPI.Core.Entities;
using Building.OwnersAPI.Repository;
using Bulding.Tests.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Building.OwnersAPI.Core.Application.NewOwner;

namespace Bulding.Tests.OwnerTest
{
    public class AddOwnerServicesTest
    {
        private readonly IMapper _mapper;
        private readonly Mock<UnitofWork> _unitofWork;
        private readonly Mock<ILogger<HandlerOwner>> _logger;
        private readonly Mock<IWebHostEnvironment> _hostingEnvironment;

        public AddOwnerServicesTest()
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
                Address = "Address Test 1",
                Birthday = Convert.ToDateTime("2001-05-05"),
                Name = "Test 1",
                PhotoFile = fileMock.Object
            };

            var handler = new NewOwner.HandlerOwner(_logger.Object,  _unitofWork.Object, _hostingEnvironment.Object);
            //Act
            var result = await handler.Handle(request, System.Threading.CancellationToken.None);

            //Assert
            _unitofWork.Object.Owners.GetContext().Owners.Count().ShouldBe(32);

        }
    }
}
