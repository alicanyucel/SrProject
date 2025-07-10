using GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;
using TELERADIOLOGY.Infrastructure.Context;

namespace TELERADIOLOGY.Infrastructure.Repositories
{
    internal class PartitionRepository : Repository<Partition, PostgreSqlDbContext>, IPartitionRepository
    {
        private readonly PostgreSqlDbContext _context;

        public PartitionRepository(PostgreSqlDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Partition?> GetByIdAsync(Guid partitionId)
        {
            return await _context.Set<Partition>()
                                 .FirstOrDefaultAsync(p => p.Id == partitionId);
        }
    }
}
