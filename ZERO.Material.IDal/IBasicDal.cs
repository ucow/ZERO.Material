using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ZERO.Material.IDal
{
    public interface IBasicDal<T> where T : class, new()
    {
        bool AddOrUpdateEntity(List<T> ts);

        bool DeleteEntity(List<T> ts);

        T GetEntity(Expression<Func<T, bool>> whereLambda);

        List<T> GetEntities(Expression<Func<T, bool>> whereLambda);

        List<T> GetPageEntities<TKey>(int pageIndex, int pageCount, Expression<Func<T, TKey>> orderLambda, out int total);
    }
}