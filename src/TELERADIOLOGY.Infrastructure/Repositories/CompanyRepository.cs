using GenericRepository;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TELERADIOLOGY.Infrastructure.Context;

namespace TELERADIOLOGY.Infrastructure.Repositories;

internal class CompanyRepository : Repository<Company, PostgreSqlDbContext>, ICompanyRepository
{
    public CompanyRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}
