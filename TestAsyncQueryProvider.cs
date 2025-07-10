 
internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider, IQueryProvider  
{  
    private readonly IQueryProvider _inner;  

    internal TestAsyncQueryProvider(IQueryProvider inner)  
    {  
        _inner = inner;  
    }  

    public IQueryable CreateQuery(Expression expression) => new TestAsyncEnumerable<TEntity>(expression);  
    public IQueryable<TElement> CreateQuery<TElement>(Expression expression) => new TestAsyncEnumerable<TElement>(expression);  

    public object Execute(Expression expression)  
    {  
        return _inner.Execute(expression) ?? throw new InvalidOperationException("Query execution returned null.");  
    }  

    public TResult Execute<TResult>(Expression expression)  
    {  
        return _inner.Execute<TResult>(expression) ?? throw new InvalidOperationException($"Query execution returned null for type {typeof(TResult).Name}.");  
    }  

    public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression) => new TestAsyncEnumerable<TResult>(expression);  

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)  
    {  
        return Execute<TResult>(expression);  
    }  
}  
