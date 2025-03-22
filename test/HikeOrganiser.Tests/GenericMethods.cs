using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MockQueryable;

namespace HikeOrganiser.Tests;

public static class GenericMethods
{
    public static void IncludeDbSetAsync<TContext, TProperty>(this Mock<TContext> contextMock, Expression<Func<TContext, DbSet<TProperty>>> property, List<TProperty>? expectedReturn = null, Action<TProperty>? onAfterAdd = null)
        where TProperty : class
        where TContext : DbContext
    {
        List<TProperty> expected = expectedReturn ?? [];

        Mock<DbSet<TProperty>> dbSetMock = SetupQueryable(expected.BuildMock());
        contextMock.IncludeSet(dbSetMock, expected, onAfterAdd);

        contextMock.Setup(property).Returns(dbSetMock.Object);
    }
    
    private static Mock<DbSet<TProperty>> SetupQueryable<TProperty>(IEnumerable<TProperty> expected)
        where TProperty : class
    {
        Mock<DbSet<TProperty>> dbSetMock = new();

        IQueryable<TProperty> expectedReturn = expected as IQueryable<TProperty> ?? expected.AsQueryable();

        Mock<IQueryable<TProperty>> queryableMock = dbSetMock.As<IQueryable<TProperty>>();

        queryableMock.Setup(mock => mock.Provider).Returns(expectedReturn.Provider);
        queryableMock.Setup(mock => mock.Expression).Returns(expectedReturn.Expression);
        queryableMock.Setup(mock => mock.ElementType).Returns(expectedReturn.ElementType);
        queryableMock.Setup(mock => mock.GetEnumerator()).Returns(() => expectedReturn.GetEnumerator());

        return dbSetMock;
    }

    private static void IncludeSet<TContext, TProperty>(this Mock<TContext> contextMock, Mock<DbSet<TProperty>> dbSetMock, List<TProperty> expected, Action<TProperty>? onAfterAdd = null)
        where TProperty : class
        where TContext : DbContext
    {
        contextMock.Setup(x => x.Set<TProperty>()).Returns(dbSetMock.Object);

        contextMock.Setup(x => x.Set<TProperty>().Add(It.IsAny<TProperty>()))
            .Callback((TProperty x) => Add(x));

        contextMock.Setup(x => x.Set<TProperty>().AddRange(It.IsAny<IEnumerable<TProperty>>()))
            .Callback((IEnumerable<TProperty> x) =>
            {
                foreach (TProperty item in x)
                {
                    Add(item);
                }
            });

        contextMock.Setup(x => x.Set<TProperty>().AddRangeAsync(It.IsAny<IEnumerable<TProperty>>(), It.IsAny<CancellationToken>()))
            .Callback((IEnumerable<TProperty> x, CancellationToken token) =>
            {
                foreach (TProperty item in x)
                {
                    Add(item);
                }
            });

        contextMock.Setup(x => x.Set<TProperty>().AddAsync(It.IsAny<TProperty>(), It.IsAny<CancellationToken>()))
            .Callback((TProperty x, CancellationToken token) => Add(x));

        contextMock.Setup(x => x.Set<TProperty>().Remove(It.IsAny<TProperty>()))
            .Callback((TProperty x) => expected.Remove(x));

        contextMock.Setup(x => x.Set<TProperty>().RemoveRange(It.IsAny<IEnumerable<TProperty>>()))
            .Callback((IEnumerable<TProperty> x) =>
            {
                foreach (TProperty item in x)
                {
                    expected.Remove(item);
                }
            });

        contextMock.Setup(x => x.Update(It.IsAny<TProperty>()))
            .Callback((TProperty item) =>
            {
                int index = expected.FindIndex(y => ReferenceEquals(y, item));
                expected[index] = item;
            });
        
        return;

        void Add(TProperty x)
        {
            expected.Add(x);

            onAfterAdd?.Invoke(x);
        }
    }
}