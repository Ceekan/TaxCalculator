using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaxCalculator.API.Data.Models;

namespace TaxCalculator.Tests
{
    public static class TaxCalculatorDBContextMock
    {
        public static Mock<DbSet<T>> GetMockDbSet<T>(List<T> source) where T : class
        {
            var query = source.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IAsyncEnumerable<T>>().Setup(x => x.GetAsyncEnumerator(default)).Returns(new TestAsyncEnumerator<T>(query.GetEnumerator()));
            dbSet.As<IQueryable<T>>().Setup(x => x.Provider).Returns(new TestAsyncQueryProvider<T>(query.Provider));
            dbSet.As<IQueryable<T>>().Setup(x => x.Expression).Returns(query.Expression);
            dbSet.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(query.ElementType);
            dbSet.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(() => query.GetEnumerator());

            dbSet.Setup(x => x.AddAsync(It.IsAny<T>(), It.IsAny<CancellationToken>()))
                .Callback((T model, CancellationToken token) => { source.Add(model); })
                .Returns((T model, CancellationToken token) => ValueTask.FromResult((EntityEntry<T>)null));

            return dbSet;
        }
    }

    internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _inner;

        internal TestAsyncQueryProvider(IQueryProvider inner)
        {
            _inner = inner;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestAsyncEnumerable<TEntity>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestAsyncEnumerable<TElement>(expression);
        }

        public object Execute(Expression expression)
        {
            return _inner.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _inner.Execute<TResult>(expression);
        }

        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
        {
            return new TestAsyncEnumerable<TResult>(expression);
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            var expectedResultType = typeof(TResult).GetGenericArguments()[0];

            var executionResult = typeof(IQueryProvider)
                                 .GetMethod(
                                      name: nameof(IQueryProvider.Execute),
                                      genericParameterCount: 1,
                                      types: new[] { typeof(Expression) })
                                 .MakeGenericMethod(expectedResultType)
                                 .Invoke(this, new[] { expression });

            return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))
                                        ?.MakeGenericMethod(expectedResultType)
                                         .Invoke(null, new[] { executionResult });
        }
    }

    internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public TestAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new TestAsyncQueryProvider<T>(this); }
        }

        public IAsyncEnumerator<T> GetEnumerator()
        {
            return GetAsyncEnumerator(default);
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }
    }

    internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public T Current
        {
            get { return _inner.Current; }
        }

        public Task<bool> MoveNext()
        {
            return Task.FromResult(_inner.MoveNext());
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return ValueTask.FromResult(_inner.MoveNext());
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}
