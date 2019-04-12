using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ZERO.Material.Command;
using ZERO.Material.Dal.Factory;
using ZERO.Material.IBll;
using ZERO.Material.IDal;

namespace ZERO.Material.Bll
{
    public abstract class BasicBll<T> : IBasicBll<T> where T : class, new()
    {
        protected IDbSession DbSession = DbSessionFactory.GetDbSession();

        protected IBasicDal<T> BasicDal;

        protected BasicBll() => SetBasicDal();

        public abstract void SetBasicDal();

        public List<T> GetPageEntities<TKey>(int pageIndex, int pageCount, Expression<Func<T, TKey>> orderLambda, Expression<Func<T, bool>> whereLambda, out int total)
        {
            return BasicDal.GetPageEntities(pageIndex, pageCount, orderLambda, whereLambda, out total);
        }

        public virtual bool AddEntities(List<T> ts)
        {
            return BasicDal.AddEntities(ts);
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public virtual bool UpdateEntities(List<T> ts)
        {
            return BasicDal.UpdateEntities(ts);
        }

        public bool DeleteEntity(List<T> ts)
        {
            return BasicDal.DeleteEntity(ts);
        }

        public T Find(params object[] keyValues)
        {
            return BasicDal.Find(keyValues);
        }

        public T GetEntity(Expression<Func<T, bool>> whereLambda)
        {
            var models = BasicDal.GetEntities(whereLambda);
            if (models == null || models.Count == 0)
            {
                return null;
            }
            return models.First();
        }

        public List<T> GetEntities(Expression<Func<T, bool>> whereLambda)
        {
            try
            {
                return BasicDal.GetEntities(whereLambda);
            }
            catch (Exception)
            {

                return BasicDal.GetEntities(whereLambda);
            }
        }

        public List<TB> ExecuteSqlCommand<TB>(string sql)
        {
            return BasicDal.ExecuteSqlCommand<TB>(sql);
        }
    }
}