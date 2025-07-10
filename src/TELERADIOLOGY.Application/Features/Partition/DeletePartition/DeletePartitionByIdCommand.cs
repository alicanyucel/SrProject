using MediatR;
using TS.Result;

public record DeletePartitionCommand(Guid PartitionId) : IRequest<Result<string>>;
