using MediatR;
using TELERADIOLOGY.Domain.Entities;
using TS.Result;

public record GetAllPartitionsQuery() : IRequest<Result<List<Partition>>>;

