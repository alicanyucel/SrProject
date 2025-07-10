using GenericRepository;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TELERADIOLOGY.Infrastructure.Context;

namespace TELERADIOLOGY.Infrastructure.Repositories;

internal class HospitalRepository : Repository<Hospital, PostgreSqlDbContext>, IHospitalRepository
{
    public HospitalRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}
