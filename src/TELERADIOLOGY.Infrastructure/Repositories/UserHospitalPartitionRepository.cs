using GenericRepository;
using TELERADIOLOGY.Domain.Repositories;
using TELERADIOLOGY.Infrastructure.Context;

namespace TELERADIOLOGY.Infrastructure.Repositories;

internal class UserHospitalPartitionRepository : Repository<UserHospitalPartition, PostgreSqlDbContext>, IUserHospitalPartitionRepository
{
    public UserHospitalPartitionRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}
