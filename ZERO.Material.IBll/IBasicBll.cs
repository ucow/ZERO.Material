﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ZERO.Material.IBll
{
    public interface IBasicBll<T> where T : class, new()
    {
        bool AddEntities(List<T> ts);

        bool UpdateEntities(List<T> ts);

        bool DeleteEntity(List<T> ts);

        T Find(params object[] keyValues);

        T GetEntity(Expression<Func<T, bool>> whereLambda);

        List<T> GetPageEntities<TKey>(int pageIndex, int pageCount, Expression<Func<T, TKey>> orderLambda, Expression<Func<T, bool>> whereLambda,
            out int total,bool isAsc = true);

        List<T> GetEntities(Expression<Func<T, bool>> whereLambda);

        List<TB> ExecuteSqlCommand<TB>(string sql);
    }
}