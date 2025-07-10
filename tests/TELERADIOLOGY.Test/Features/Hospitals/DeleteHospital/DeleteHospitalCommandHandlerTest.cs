using GenericRepository;
using Moq;
using TELERADIOLOGY.Application.Features.Hospitals.DeleteHospital;
using TELERADIOLOGY.Domain.Repositories;

public class DeleteHospitalCommandHandlerTests
{
    private readonly Mock<IHospitalRepository> _hospitalRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPermissionRepository> _permissionRepositoryMock;
    private readonly DeleteHospitalByIdCommandHandler _handler;

    public DeleteHospitalCommandHandlerTests()
    {
        _hospitalRepositoryMock = new Mock<IHospitalRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _permissionRepositoryMock = new Mock<IPermissionRepository>();
        _handler = new DeleteHospitalByIdCommandHandler(
            _hospitalRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

}