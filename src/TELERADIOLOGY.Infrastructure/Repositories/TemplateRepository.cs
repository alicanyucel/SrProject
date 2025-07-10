using GenericRepository;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TELERADIOLOGY.Infrastructure.Context;

namespace TELERADIOLOGY.Infrastructure.Repositories;

internal class TemplateRepository : Repository<Template, PostgreSqlDbContext>, ITemplateRepository
{
    public TemplateRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}
