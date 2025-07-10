using MediatR;
using TELERADIOLOGY.Domain.Entities;
using TS.Result;

public record GetPartitionByIdQuery(Guid Id) : IRequest<Result<Partition>>;
