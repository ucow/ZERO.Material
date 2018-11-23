using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ZERO.Material.IDal;

namespace ZERO.Material.IBll
{
    public interface IBasicBll<T> where T : class, new()
    {
        bool AddOrUpdateEntity(List<T> ts);

        bool DeleteEntity(List<T> ts);

        T GetEntity(Expression<Func<T, bool>> whereLambda);

        List<T> GetPageEntities<TKey>(int pageIndex, int pageCount, Expression<Func<T, TKey>> orderLambda,
            out int total);

        List<T> GetEntities(Expression<Func<T, bool>> whereLambda);
    }
}