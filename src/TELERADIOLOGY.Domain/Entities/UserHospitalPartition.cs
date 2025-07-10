using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TELERADIOLOGY.Domain.Abstractions;

namespace TELERADIOLOGY.Infrastructure.Repositories
{
    public sealed class UserHospitalPartition:Entity
    {
      
        public required Guid UserId { get; set; }
    
        public required Guid PartitionId { get; set; }
    }
}
