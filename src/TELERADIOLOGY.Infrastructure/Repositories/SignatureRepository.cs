using GenericRepository;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TELERADIOLOGY.Infrastructure.Context;

namespace TELERADIOLOGY.Infrastructure.Repositories;

internal class SignatureRepository : Repository<DoctorSignature, PostgreSqlDbContext>, ISignatureRepository
{
    public SignatureRepository(PostgreSqlDbContext context) : base(context)
    {
    }
}
