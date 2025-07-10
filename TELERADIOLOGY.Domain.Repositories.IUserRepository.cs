public interface IUserRepository : IRepository<AppUser>
{
    Task<AppUser?> GetByIdentityNumberAsync(string identityNumber, CancellationToken cancellationToken);
}
