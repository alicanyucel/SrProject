using GenericRepository;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TELERADIOLOGY.Infrastructure.Context;

namespace TELERADIOLOGY.Infrastructure.Repositories;

internal class CompanyUserRepository : Repository<CompanyUser, PostgreSqlDbContext>, ICompanyUserRepository
{
    public CompanyUserRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}
