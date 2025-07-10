using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Linq.Expressions;
using TELERADIOLOGY.Application.Features.Reports.GetAllReport;
using TELERADIOLOGY.Domain.Entities;
using TELERADIOLOGY.Domain.Repositories;

public class GetAllReportQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsOrderedReportList()
    {
        var reports = new List<Report>
        {
            new Report { Id = Guid.NewGuid(), ReportName = "Z Report" },
            new Report { Id = Guid.NewGuid(), ReportName = "A Report" },
            new Report { Id = Guid.NewGuid(), ReportName = "M Report" }
        }.AsQueryable();

        var mockSet = new Mock<DbSet<Report>>();

        mockSet.As<IAsyncEnumerable<Report>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<Report>(reports.GetEnumerator()));

        mockSet.As<IQueryable<Report>>()
            .Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<Report>(reports.Provider));
        mockSet.As<IQueryable<Report>>().Setup(m => m.Expression).Returns(reports.Expression);
        mockSet.As<IQueryable<Report>>().Setup(m => m.ElementType).Returns(reports.ElementType);
        mockSet.As<IQueryable<Report>>().Setup(m => m.GetEnumerator()).Returns(reports.GetEnumerator());

        var repoMock = new Mock<IReportRepository>();
        repoMock.Setup(r => r.GetAll()).Returns(mockSet.Object);

        var handler = new GetAllReportQueryHandler(repoMock.Object);

        var result = await handler.Handle(new GetAllReportQuery(), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal("A Report", result[0].ReportName);
        Assert.Equal("M Report", result[1].ReportName);
        Assert.Equal("Z Report", result[2].ReportName);
    }

    private class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;
        public TestAsyncQueryProvider(IQueryProvider inner) => _inner = inner ?? throw new ArgumentNullException(nameof(inner));

        public IQueryable CreateQuery(Expression expression) => new TestAsyncEnumerable<TEntity>(expression);

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new TestAsyncEnumerable<TElement>(expression);

        public object Execute(Expression expression) => _inner.Execute(expression) ?? throw new InvalidOperationException("Execution returned null.");

        public TResult Execute<TResult>(Expression expression)
        {
            var result = _inner.Execute<TResult>(expression);
            if (result == null)
                throw new InvalidOperationException("Execution returned null.");
            return result;
        }

        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
        {
            var result = Execute<TResult>(expression);
            return Task.FromResult(result);
        }

        TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    private class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) { }

        public TestAsyncEnumerable(Expression expression) : base(expression) { }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default) =>
            new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

        IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
    }

    private class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;
        public TestAsyncEnumerator(IEnumerator<T> inner) => _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        public T Current => _inner.Current;
        public ValueTask DisposeAsync()
        {
            _inner.Dispose();
            return ValueTask.CompletedTask;
        }
        public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext());
    }
}
