using GenericRepository;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TELERADIOLOGY.Infrastructure.Context;

namespace TELERADIOLOGY.Infrastructure.Repositories;

internal class ReportRepository : Repository<Report, PostgreSqlDbContext>, IReportRepository
{
    public ReportRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}
