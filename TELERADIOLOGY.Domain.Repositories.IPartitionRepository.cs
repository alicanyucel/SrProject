public interface IPartitionRepository : IRepository<Partition>
{
    Task<Partition?> GetByIdAsync(Guid partitionId);
}
