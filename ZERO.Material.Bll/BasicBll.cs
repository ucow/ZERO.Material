using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Attributes;
using ZERO.Material.Command;
using ZERO.Material.IBll;
using ZERO.Material.IDal;

namespace ZERO.Material.Bll
{
    public abstract class BasicBll<T> : IBasicBll<T> where T : class, new()
    {
        protected IBasicDal<T> BasicDal;
        protected UnityContainerHelper UnityContainerHelper = new UnityContainerHelper();

        protected BasicBll() => SetBasicDal();

        public abstract void SetBasicDal();

        public bool AddEntity(List<T> ts)
        {
            return BasicDal.AddEntity(ts);
        }

        public List<T> GetPageEntities<TKey>(int pageIndex, int pageCount, Expression<Func<T, TKey>> orderLambda, out int total)
        {
            return BasicDal.GetPageEntities(pageIndex, pageCount, orderLambda, out total);
        }

        public bool UpdateEntity(List<T> ts)
        {
            return BasicDal.UpdateEntity(ts);
        }

        public bool DeleteEntity(List<T> ts)
        {
            return BasicDal.DeleteEntity(ts);
        }

        public T GetEntity(Expression<Func<T, bool>> whereLambda)
        {
            return BasicDal.GetEntity(whereLambda);
        }

        public List<T> GetEntities(Expression<Func<T, bool>> whereLambda)
        {
            return BasicDal.GetEntities(whereLambda);
        }
    }
}