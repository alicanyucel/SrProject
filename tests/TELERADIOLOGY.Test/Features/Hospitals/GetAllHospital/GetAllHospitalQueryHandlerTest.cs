using FluentAssertions;
using Moq;
using TELERADIOLOGY.Application.Features.Hospitals.GetAllHospital;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class GetAllHospitalQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnHospitalsOrderedByFullTitle()
    {
        var data = new List<Hospital>
        {
            new Hospital { Id = Guid.NewGuid(), FullTitle = "Beta Hospital" },
            new Hospital { Id = Guid.NewGuid(), FullTitle = "Alpha Hospital" }
        };

        var hospitalRepositoryMock = new Mock<IHospitalRepository>();
        hospitalRepositoryMock.Setup(r => r.GetAll()).Returns(data.AsQueryable());

        var handler = new GetAllHospitalQueryHandler(hospitalRepositoryMock.Object);
        var result = await handler.Handle(new GetAllHospitalQuery(), CancellationToken.None);

        result.IsSuccessful.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Data[0].FullTitle.Should().Be("Alpha Hospital");
        result.Data[1].FullTitle.Should().Be("Beta Hospital");
    }
}
