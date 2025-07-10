using GenericRepository;
using TELERADIOLOGY.Domain.Entities;

namespace TELERADIOLOGY.Domain.Repositories;

public interface IPartitionRepository : IRepository<Partition>
{
    Task<Partition> GetByIdAsync(Guid partitionId);
}
