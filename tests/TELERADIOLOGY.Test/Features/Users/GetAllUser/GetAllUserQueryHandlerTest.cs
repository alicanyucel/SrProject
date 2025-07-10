using AutoMapper;
using FluentAssertions;
using Moq;
using TELERADIOLOGY.Application.Dtos.User;
using TELERADIOLOGY.Application.Features.Users.GetAllUser;
using TELERADIOLOGY.Application.Services;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class GetAllUserQueryHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllUserQueryHandler _handler;

    public GetAllUserQueryHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _cacheServiceMock = new Mock<ICacheService>();
        _mapperMock = new Mock<IMapper>();

        _handler = new GetAllUserQueryHandler(
            _userRepositoryMock.Object,
            _cacheServiceMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Should_Return_Users_From_Cache_When_Cache_Exists()
    {
        var cachedUsers = new List<AppUser>
        {
            new AppUser
            {
                Id = Guid.NewGuid(),
                FirstName = "Ali",
                LastName = "Yılmaz",
                IsDeleted = false,
                LoginId = Guid.NewGuid()
            }
        };

        var cachedDtos = new List<UserResultDto>
        {
            new UserResultDto
            {
                FirstName = "Ali",
                LastName = "Yılmaz"
            }
        };

        _cacheServiceMock.Setup(x => x.Get<List<AppUser>>("users")).Returns(cachedUsers);
        _mapperMock.Setup(x => x.Map<List<UserResultDto>>(cachedUsers)).Returns(cachedDtos);

        var result = await _handler.Handle(new GetAllUserQuery(), CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.First().FirstName.Should().Be("Ali");

        _userRepositoryMock.Verify(x => x.GetAll(), Times.Never);
    }
}
