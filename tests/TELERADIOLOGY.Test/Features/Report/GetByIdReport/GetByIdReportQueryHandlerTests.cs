using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Linq.Expressions;
using TELERADIOLOGY.Application.Features.Reports.GetReportById;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class GetReportByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReportExists_ReturnsReport()
    {
        var reportId = Guid.NewGuid();
        var reports = new List<Report>
        {
            new Report { Id = reportId, ReportName = "Test Report" },
            new Report { Id = Guid.NewGuid(), ReportName = "Other Report" }
        }.AsQueryable();

        var mockSet = new Mock<DbSet<Report>>();
        mockSet.As<IAsyncEnumerable<Report>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<Report>(reports.GetEnumerator()));

        mockSet.As<IQueryable<Report>>().Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<Report>(reports.Provider));
        mockSet.As<IQueryable<Report>>().Setup(m => m.Expression).Returns(reports.Expression);
        mockSet.As<IQueryable<Report>>().Setup(m => m.ElementType).Returns(reports.ElementType);
        mockSet.As<IQueryable<Report>>().Setup(m => m.GetEnumerator()).Returns(reports.GetEnumerator());

        var repoMock = new Mock<IReportRepository>();
        repoMock.Setup(r => r.GetAll()).Returns(mockSet.Object);

        var handler = new GetReportByIdQueryHandler(repoMock.Object);
        var result = await handler.Handle(new GetReportByIdQuery(reportId), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(reportId, result.Id);
        Assert.Equal("Test Report", result.ReportName);
    }

    internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;
        internal TestAsyncQueryProvider(IQueryProvider inner) => _inner = inner;

        public IQueryable CreateQuery(Expression expression) => new TestAsyncEnumerable<TEntity>(expression);
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new TestAsyncEnumerable<TElement>(expression);

        public object Execute(Expression expression) => _inner.Execute(expression)!;
        public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            var expectedResultType = typeof(TResult);
            var executedResult = Execute(expression);

            if (executedResult is TResult correctResult)
                return correctResult;

            // Wrap result in Task if TResult is Task<T>
            var resultType = expectedResultType.IsGenericType ? expectedResultType.GetGenericTypeDefinition() : null;
            if (resultType == typeof(Task<>))
            {
                var taskResultType = expectedResultType.GenericTypeArguments[0];
                var taskFromResultMethod = typeof(Task).GetMethod(nameof(Task.FromResult))!.MakeGenericMethod(taskResultType);
                return (TResult)taskFromResultMethod.Invoke(null, new[] { executedResult })!;
            }

            throw new InvalidCastException($"Unable to cast result to {expectedResultType}");
        }
    }

    internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }
        public TestAsyncEnumerable(Expression expression) : base(expression) { }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            => new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

        IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
    }

    internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;
        public TestAsyncEnumerator(IEnumerator<T> inner) => _inner = inner;

        public T Current => _inner.Current;

        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return ValueTask.CompletedTask;
        }

        public ValueTask<bool> MoveNextAsync() => ValueTask.FromResult(_inner.MoveNext());
    }
}
